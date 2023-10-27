SELECT
    history.inout_datetime AS relocation_date,                                                                 -- 移庫日
    stock.parts_location_id,                                                                                   -- 棚番
    stock.parts_location_id AS parts_location_code,                                                            -- 棚のコード
    stock.parts_location_detail_no,                                                                            -- 枝番
    parts.factory_id AS parts_factory_id,                                                                      -- 工場ID(棚番・枝番の結合文字列取得用)
    history.inout_quantity AS transfer_count,                                                                  -- 移庫数
    lot.unit_price * history.inout_quantity AS transfer_amount,                                                -- 入庫単価(移庫金額)
    lot.unit_price,                                                                                            -- 入庫単価
    COALESCE(unit_digit.extension_data, 0) AS unit_digit,                                                      -- 小数点以下桁数(数量)
    COALESCE(currency_digit.currency_digit, 0) AS currency_digit,                                              -- 小数点以下桁数(金額)
    COALESCE(round_division.extension_data, 1) AS unit_round_division,                                         -- 丸め処理区分(数量)
    COALESCE(round_division.extension_data, 1) AS currency_round_division,                                     -- 丸め処理区分(金額)
    stock.stock_quantity AS stock_quantity_exept_unit,                                                         -- 在庫数
    lot.department_structure_id AS department_id,                                                              -- 部門ID
    lot.lot_control_id,                                                                                        -- ロット管理ID
    stock.inventory_control_id,                                                                                -- 在庫管理ID
    update_serialid.update_serialid,                                                                           -- 更新シリアルID
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
    ) AS unit_name,                                                                                                     -- 数量管理単位(名称)   
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
    ) AS currency_name                                                                                                  -- 金額管理単位(名称)

FROM
    pt_inout_history history
    LEFT JOIN
        pt_lot lot
    ON  history.lot_control_id = lot.lot_control_id
     AND lot.delete_flg = 0
    LEFT JOIN
        pt_location_stock stock -- 在庫データ
    ON  history.inventory_control_id = stock.inventory_control_id
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
    LEFT JOIN
    (
       SELECT
           pih.work_no,
           MAX(pih.update_serialid) AS update_serialid
       FROM
           pt_inout_history pih
       WHERE
           pih.work_no = @WorkNo
       GROUP BY
           pih.work_no
    ) update_serialid
    ON  history.work_no = update_serialid.work_no
WHERE
    history.work_no = @WorkNo
-- 「受入」のデータ
AND inout_div.inout_code = '1'
-- 「棚番移庫」のデータ
AND work_div.work_code = '3'