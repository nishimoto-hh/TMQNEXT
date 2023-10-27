SELECT
    COALESCE(stock.parts_location_detail_no,'') AS parts_location_detail_no,                                    -- 棚枝番
    COALESCE(stock.stock_quantity, 0) AS stock_quantity,                                                        -- 在庫数
    CAST(stock.parts_location_id AS nvarchar) + '_' + COALESCE(stock.parts_location_detail_no, '') AS nest_key, -- 入れ子キー
    parts.factory_id,                                                                                           -- 工場ID(結合文字列取得用)
    stock.parts_location_id,                                                                                    -- 棚ID(並び替え用)
    COALESCE(unit_digit.extension_data, 0) AS unit_digit,                                                       -- 小数点以下桁数(数量)
    COALESCE(round_division.extension_data, 1) AS unit_round_division,                                          -- 丸め処理区分(数量
         ---------------------------------- 以下は翻訳を取得 ----------------------------------
    rack_trans.rack_name AS location_nm,                                                                        -- 棚番
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
    ) AS unit_name                                                                                                -- 数量管理単位(名称)  
FROM
    (
        SELECT
            stock.parts_location_id,
            stock.parts_location_detail_no,
            COALESCE(SUM(inventory.stock_quantity), 0) AS stock_quantity
        FROM
            (
                SELECT DISTINCT
                    stock.parts_location_id,
                    COALESCE(stock.parts_location_detail_no, '') AS parts_location_detail_no
                FROM
                    pt_location_stock stock
                WHERE
                    stock.parts_id = @PartsId
            ) stock
            LEFT JOIN
                v_pt_location_inventory inventory
            -- 棚番名称
        ON  stock.parts_location_id = inventory.parts_location_id
        AND stock.parts_location_detail_no = COALESCE(inventory.parts_location_no, '')
        AND inventory.parts_id = @PartsId
        GROUP BY
            stock.parts_location_id,
            stock.parts_location_detail_no
    ) stock
    LEFT JOIN
        pt_parts parts
    ON  parts.parts_id = @PartsId
    LEFT JOIN
        unit_digit -- 数量管理単位
    ON  parts.unit_structure_id = unit_digit.structure_id
    LEFT JOIN
        round_division --丸め処理区分
    ON  parts.factory_id = round_division.factory_id
     LEFT JOIN -- 棚番名称
        rack_trans
    ON stock.parts_location_id = rack_trans.rack_id
WHERE
    -- 在庫があるものを抽出
    stock.stock_quantity > 0
ORDER BY
    parts_location_id,
    parts_location_detail_no