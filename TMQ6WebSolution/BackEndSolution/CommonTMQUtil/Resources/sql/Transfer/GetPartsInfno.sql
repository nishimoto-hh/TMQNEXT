SELECT
    parts.parts_no,                                                                                      -- 予備品No.
    parts.parts_name,                                                                                    -- 予備品名
    parts.manufacturer_structure_id,                                                                     -- メーカー
    parts.model_type,                                                                                    -- 型式
    parts.standard_size,                                                                                 -- 規格・寸法
    coalesce(inv.stock_quantity, 0) AS stock_quantity,                                                   -- 在庫数
    coalesce(unit_digit.extension_data, 0) AS unit_digit,                                                -- 小数点以下桁数
    coalesce(round_division.extension_data, 1) AS unit_round_division,                                   -- 丸め処理区分
    parts.parts_id,                                                                                      -- 予備品ID
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
                  st_f.structure_id = parts.unit_structure_id
              AND st_f.factory_id IN(0, parts.factory_id)
           )
      AND tra.structure_id = parts.unit_structure_id
    ) AS unit_name                                                                                       -- 数量管理単位
FROM
    pt_parts parts
    LEFT JOIN
        v_pt_parts_inventory inv
    ON  parts.parts_id = inv.parts_id
    LEFT JOIN
        unit_digit
    ON  parts.unit_structure_id = unit_digit.structure_id
    LEFT JOIN
        round_division
    ON  parts.factory_id = round_division.factory_id
WHERE
    parts.parts_id = @PartsId