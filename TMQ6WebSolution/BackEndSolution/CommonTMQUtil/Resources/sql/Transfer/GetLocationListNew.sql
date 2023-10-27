SELECT
    lot.receiving_datetime,                                                                                                                         -- 入庫日
    lot.lot_no,                                                                                                                                     -- 入庫No.
    stock.parts_location_detail_no,                                                                                                                 -- 枝番
    parts.factory_id AS parts_factory_id,                                                                                                           -- 工場ID(棚番・枝番の結合文字列取得用)
    lot.old_new_structure_id,                                                                                                                       -- 新旧区分
    lot.unit_price,                                                                                                                                 -- 入庫単価
    stock.stock_quantity,                                                                                                                           -- 在庫数
    lot.management_no,                                                                                                                              -- 管理No.
    lot.management_division,                                                                                                                        -- 管理区分
    COALESCE(unit_digit.extension_data, 0) AS unit_digit,                                                                                           -- 小数点以下桁数(数量)
    COALESCE(currency_digit.currency_digit, 0) AS currency_digit,                                                                                   -- 小数点以下桁数(金額)
    COALESCE(round_division.extension_data, 1) AS unit_round_division,                                                                              -- 丸め処理区分(数量)
    COALESCE(round_division.extension_data, 1) AS currency_round_division,                                                                          -- 丸め処理区分(金額)
    stock.parts_location_id,                                                                                                                        -- 棚ID
    stock.parts_location_id AS parts_location_code,                                                                                                 -- 棚のコード
    lot.department_structure_id AS department_id,                                                                                                   -- 部門ID
    lot.account_structure_id,                                                                                                                       -- 勘定科目ID
    stock.lot_control_id,                                                                                                                           -- ロット管理ID
    stock.inventory_control_id,                                                                                                                     -- 在庫管理ID
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
    rack_trans.rack_name AS location_nm                                                                                                                            -- 棚番
FROM
    pt_location_stock stock
    LEFT JOIN
        pt_lot lot
    ON  stock.lot_control_id = lot.lot_control_id
    AND lot.delete_flg = 0
    LEFT JOIN
        pt_parts parts
    ON  lot.parts_id = parts.parts_id
    LEFT JOIN
        department -- 部門
    ON  lot.department_structure_id = department.department_id
    LEFT JOIN
        subject -- 勘定科目
    ON  lot.account_structure_id = subject.subject_id
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
    LEFT JOIN -- 棚番名称
        rack_trans
    ON stock.parts_location_id = rack_trans.rack_id
WHERE
    lot.parts_id = @PartsId
AND stock.stock_quantity > 0
ORDER BY
    lot.receiving_datetime,
    lot.lot_no,
    stock.inventory_control_id