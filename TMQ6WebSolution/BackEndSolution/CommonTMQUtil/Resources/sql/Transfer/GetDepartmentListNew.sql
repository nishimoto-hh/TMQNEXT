SELECT
    lot.receiving_datetime,                                                                                                                         -- 入庫日
    lot.lot_no,                                                                                                                                     -- 入庫No.
    lot.old_new_structure_id,                                                                                                                       -- 新旧区分
    lot.unit_price,                                                                                                                                 -- 入庫単価
    stock.stock_quantity,                                                                                                                           -- 在庫数
    lot.management_no,                                                                                                                              -- 管理No.
    lot.management_division,                                                                                                                        -- 管理区分
    COALESCE(unit_digit.extension_data, 0) AS unit_digit,                                                                                           -- 小数点以下桁数(数量)
    COALESCE(currency_digit.currency_digit, 0) AS currency_digit,                                                                                   -- 小数点以下桁数(金額)
    COALESCE(round_division.extension_data, 1) AS unit_round_division,                                                                              -- 丸め処理区分(数量)
    COALESCE(round_division.extension_data, 1) AS currency_round_division,                                                                          -- 丸め処理区分(金額)
    lot.department_structure_id AS department_id,                                                                                                   -- 部門ID
    department.department_code,                                                                                                                     -- 部門コード
    lot.account_structure_id AS subject_id,                                                                                                         -- 勘定科目ID
    subject.subject_code,                                                                                                                           -- 勘定科目コード 
    COALESCE(department.department_flg, 0) AS department_flg,                                                                                       -- 部門フラグ
    lot.lot_control_id,                                                                                                                             -- ロット管理ID
    parts.factory_id AS parts_factory_id,                                                                                                           -- 管理工場ID
         ---------------------------------- 以下は翻訳を取得 ----------------------------------
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
                  st_f.structure_id = lot.unit_structure_id
              AND st_f.factory_id IN(0,@UserFactoryId)
           )
      AND tra.structure_id = lot.unit_structure_id
    ) AS unit_name,                                                                                                                                               -- 数量管理単位(名称)   
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
                  st_f.structure_id = lot.currency_structure_id
              AND st_f.factory_id IN(0, @UserFactoryId)
           )
      AND tra.structure_id = lot.currency_structure_id
    ) AS currency_name,                                                                                                                                           -- 金額管理単位(名称)
    department.department_code + ' ' + department_trans.department_name AS department_nm,                                                                         -- 部門
    subject.subject_code + ' ' + 
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
    ) AS subject_nm,                                                                                                                                               -- 勘定科目
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
    ) AS subject_translation                                                                                                                                       -- 勘定科目翻訳

FROM
    (
        SELECT
            lot.lot_no,
            SUM(coalesce(stock.stock_quantity, 0)) AS stock_quantity
        FROM
            pt_location_stock stock
            LEFT JOIN
                pt_lot lot
            ON  stock.lot_control_id = lot.lot_control_id
        WHERE
            stock.parts_id = @PartsId
        AND stock.stock_quantity > 0
        GROUP BY lot.lot_no
    ) stock
    LEFT JOIN
        pt_lot lot
    ON  stock.lot_no = lot.lot_no
    LEFT JOIN
        department -- 部門
    ON  lot.department_structure_id = department.department_id
    LEFT JOIN
        subject -- 勘定科目
    ON  lot.account_structure_id = subject.subject_id
    LEFT JOIN
        pt_parts parts -- 予備品仕様
    ON  lot.parts_id = parts.parts_id
    LEFT JOIN
        unit_digit -- 小数点以下桁数(数量)
    ON  lot.unit_structure_id = unit_digit.structure_id
    LEFT JOIN
        currency_digit -- 小数点以下桁数(金額)
    ON  lot.currency_structure_id = currency_digit.currency_id
    LEFT JOIN
        round_division -- 丸め処理区分
    ON  parts.factory_id = round_division.factory_id
    LEFT JOIN -- 部門名称
        department_trans
    ON  lot.department_structure_id = department_trans.department_id
ORDER BY
    lot.receiving_datetime,
    lot.lot_no