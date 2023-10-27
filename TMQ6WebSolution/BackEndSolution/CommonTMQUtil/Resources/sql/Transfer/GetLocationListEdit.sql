SELECT
    lot.receiving_datetime,                                                                                                                           -- 入庫日
    lot.lot_no,                                                                                                                                       -- 入庫No.
    stock.parts_location_detail_no,                                                                                                                   -- 枝番
    parts.factory_id,                                                                                                                                 -- 工場ID(棚番・枝番の結合文字列取得用)
    lot.old_new_structure_id,                                                                                                                         -- 新旧区分
    lot.unit_price,                                                                                                                                   -- 入庫単価
    history.inout_quantity AS stock_quantity,                                                                                                         -- 在庫数(受払数)
    lot.management_no,                                                                                                                                -- 管理No.
    lot.management_division,                                                                                                                          -- 管理区分
    COALESCE(unit_digit.extension_data, 0) AS unit_digit ,                                                                                            -- 小数点以下桁数(数量)
    COALESCE(currency_digit.currency_digit, 0) AS currency_digit,                                                                                     -- 小数点以下桁数(金額)
    COALESCE(round_division.extension_data, 1) AS unit_round_division,                                                                                -- 丸め処理区分(数量)
    COALESCE(round_division.extension_data, 1) AS currency_round_division,                                                                            -- 丸め処理区分(金額)
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
    pt_inout_history history
    LEFT JOIN
        pt_lot lot -- ロット情報
    ON  history.lot_control_id = lot.lot_control_id
    LEFT JOIN
        pt_location_stock stock -- 在庫データ
    ON  history.inventory_control_id = stock.inventory_control_id
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
        inout_div -- 受払区分
    ON  history.inout_division_structure_id = inout_div.inout_id
    LEFT JOIN
        work_div -- 作業区分
    ON  history.work_division_structure_id = work_div.work_id
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
    history.work_no = @WorkNo
    -- 「払出」のデータ
AND inout_div.inout_code = '2'
    -- 「棚番移庫」のデータ
AND work_div.work_code = '3'