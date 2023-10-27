SELECT
    lot.receiving_datetime,                                                                                                                         -- 入庫日
    lot.lot_no,                                                                                                                                     -- 入庫No.
    lot.old_new_structure_id,                                                                                                                       -- 新旧区分
    COALESCE(stock.stock_quantity, 0) AS stock_quantity,                                                                                            -- 在庫数
    COALESCE(lot.unit_price, 0) AS unit_price,                                                                                                      -- 入庫単価
    COALESCE(stock.stock_quantity * lot.unit_price, 0) AS stock_amount,                                                                             -- 在庫金額
    lot.management_division,                                                                                                                        -- 管理区分
    lot.management_no,                                                                                                                              -- 管理No.
    CAST(stock.parts_location_id AS nvarchar) + '_' + COALESCE(stock.parts_location_detail_no,'') AS nest_key,                                      -- 入れ子キー
    COALESCE(unit_digit.extension_data, 0) AS unit_digit,                                                                                           -- 小数点以下桁数(数量)
    COALESCE(currency_digit.currency_digit, 0) AS currency_digit,                                                                                   -- 小数点以下桁数(金額)
    COALESCE(round_division.extension_data, 1) AS unit_round_division,                                                                              -- 丸め処理区分(数量)
    COALESCE(round_division.extension_data, 1) AS currency_round_division,                                                                          -- 丸め処理区分(金額)
    department.department_code AS department_cd_enter,                                                                                              -- 部門CD(ラベル出力用)
    subject.subject_code AS subject_cd_enter,                                                                                                       -- 勘定科目CD(ラベル出力用)
    parts.parts_id,                                                                                                                                 -- 予備品ID(ラベル出力用)
    stock.parts_location_id AS parts_location_id_enter,                                                                                             -- 棚ID(ラベル出力用)
    stock.parts_location_detail_no AS parts_location_detail_no_enter,                                                                               -- 棚枝番(ラベル出力用)
         ---------------------------------- 以下は翻訳を取得 ---------------------------------- 
    department.department_code + ' ' +
    COALESCE(
        (
          SELECT
          tra.translation_text
      FROM
         v_structure_item_all AS tra
      WHERE
          tra.language_id = @LanguageId
      AND tra.location_structure_id = (
              SELECT
                  MAX(st_f.factory_id)
              FROM
                  structure_factory AS st_f
              WHERE
                  st_f.structure_id = lot.department_structure_id
              AND st_f.factory_id IN(0, @UserFactoryId)
           )
      AND tra.structure_id = lot.department_structure_id
    ) , 
        (
          SELECT
          tra.translation_text
      FROM
         v_structure_item_all AS tra
      WHERE
          tra.language_id = @LanguageId
      AND tra.location_structure_id = (
              SELECT
                  MAX(st_f.factory_id)
              FROM
                  structure_factory AS st_f
              WHERE
                  st_f.structure_id = lot.department_structure_id
              AND st_f.factory_id NOT IN(0, @UserFactoryId)
           )
      AND tra.structure_id = lot.department_structure_id
    )
    ) AS department_nm,                                                                                                                           -- 部門
    subject.subject_code + ' ' +
    COALESCE(
        (
          SELECT
          tra.translation_text
      FROM
         v_structure_item_all AS tra
      WHERE
          tra.language_id = @LanguageId
      AND tra.location_structure_id = (
              SELECT
                  MAX(st_f.factory_id)
              FROM
                  structure_factory AS st_f
              WHERE
                  st_f.structure_id = lot.account_structure_id
              AND st_f.factory_id IN(0, @UserFactoryId)
           )
      AND tra.structure_id = lot.account_structure_id
    ),
        (
          SELECT
          tra.translation_text
      FROM
         v_structure_item_all AS tra
      WHERE
          tra.language_id = @LanguageId
      AND tra.location_structure_id = (
              SELECT
                  MAX(st_f.factory_id)
              FROM
                  structure_factory AS st_f
              WHERE
                  st_f.structure_id = lot.account_structure_id
              AND st_f.factory_id NOT IN(0, @UserFactoryId)
           )
      AND tra.structure_id = lot.account_structure_id
    )
    ) AS subject_nm,                                                                                                                               -- 勘定科目
    
    (
      SELECT
          tra.translation_text
      FROM
         v_structure_item_all AS tra
      WHERE
          tra.language_id = @LanguageId
      AND tra.location_structure_id = (
              SELECT
                  MAX(st_f.factory_id)
              FROM
                  structure_factory AS st_f
              WHERE
                  st_f.structure_id = parts.unit_structure_id
              AND st_f.factory_id IN(0,@UserFactoryId)
           )
      AND tra.structure_id = parts.unit_structure_id
    ) AS unit_name,                                                                                                                                -- 数量管理単位(名称)
        (
      SELECT
          tra.translation_text
      FROM
         v_structure_item_all AS tra
      WHERE
          tra.language_id = @LanguageId
      AND tra.location_structure_id = (
              SELECT
                  MAX(st_f.factory_id)
              FROM
                  structure_factory AS st_f
              WHERE
                  st_f.structure_id = parts.currency_structure_id
              AND st_f.factory_id IN(0, @UserFactoryId)
           )
      AND tra.structure_id = parts.currency_structure_id
    ) AS currency_name                                                                                                                            -- 金額管理単位(名称)
FROM
    pt_location_stock stock
    LEFT JOIN
        pt_lot lot
    ON  stock.lot_control_id = lot.lot_control_id
    LEFT JOIN
        department -- 部門
    ON  lot.department_structure_id = department.department_id
    LEFT JOIN
        subject -- 勘定科目
    ON  lot.account_structure_id = subject.subject_id
    LEFT JOIN
        pt_parts parts
    ON  lot.parts_id = parts.parts_id
    LEFT JOIN
        unit_digit -- 数量管理単位
    ON  parts.unit_structure_id = unit_digit.structure_id
    LEFT JOIN
        currency_digit -- 金額管理単位
    ON  parts.currency_structure_id = currency_digit.currency_id
    LEFT JOIN
        round_division --丸め処理区分
    ON  parts.factory_id = round_division.factory_id
WHERE
    lot.parts_id = @PartsId -- 在庫があるものを抽出
AND COALESCE(stock.stock_quantity, 0) > 0
ORDER BY
    stock.parts_location_id,
    stock.parts_location_detail_no,
    lot.receiving_datetime