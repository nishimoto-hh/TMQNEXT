--******************************************************************
--入庫入力(予備品情報)
--******************************************************************
WITH number_unit AS ( 
    -- 数量管理単位
    SELECT
        ms.structure_id AS unit_id,
        ex.extension_data AS unit_digit
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 2
    WHERE
        ms.structure_group_id = 1730
) 
, unit_round AS ( 
    --丸め処理区分
    SELECT
        ms.factory_id,
        ex.extension_data AS round_division 
    FROM
        (
            SELECT
                ms.factory_id,
                MAX(ms.structure_id) AS structure_id
            FROM
                ms_structure ms
            WHERE
                ms.structure_group_id = 2050
            GROUP BY
                ms.factory_id
        ) ms
        LEFT JOIN
            (
                SELECT
                    ms.structure_id,
                    ex.extension_data
                FROM
                    ms_structure ms
                    LEFT JOIN
                        ms_item_extension ex
                    ON  ms.structure_item_id = ex.item_id
                    AND ex.sequence_no = 1
                WHERE
                    ms.structure_group_id = 2050
            ) ex
        ON  ms.structure_id = ex.structure_id
) 
,structure_factory as( SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1150, 1730) 
        AND language_id = @LanguageId
)
SELECT
    pp.parts_location_id                        --標準棚ID
    , pp.parts_location_detail_no               --標準棚枝番
    , pp.parts_no                               --予備品No.
    , pp.parts_name                             --予備品名
    , pp.manufacturer_structure_id              --メーカー
    , pp.model_type                             --型式
    , pp.standard_size AS dimensions            --規格・寸法
    , ppi.stock_quantity                        --在庫数
    ,(
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
                  st_f.structure_id = pp.unit_structure_id
              AND st_f.factory_id IN(0, pp.factory_id)
           )
      AND tra.structure_id = pp.unit_structure_id
    ) AS unit_name                                -- 数量管理単位
    --, number_unit.unit_name                     --数量管理単位
    , pp.factory_id                             --工場ID
    , COALESCE(number_unit.unit_digit, 0) AS unit_digit     --小数点以下桁数(数量)
    , COALESCE(unit_round.round_division, 0) AS round_division  --丸め処理区分
    , COALESCE(
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
                  st_f.structure_id = pp.manufacturer_structure_id
              AND st_f.factory_id IN(0, pp.factory_id)
           )
      AND tra.structure_id = pp.manufacturer_structure_id
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
                  MIN(st_f.factory_id)
              FROM
                  structure_factory AS st_f
              WHERE
                  st_f.structure_id = pp.manufacturer_structure_id
              AND st_f.factory_id NOT IN(0, pp.factory_id)
           )
      AND tra.structure_id = pp.manufacturer_structure_id
    )) AS manufacturer_name                    -- メーカー
FROM
    pt_parts AS pp 
    LEFT JOIN v_pt_parts_inventory AS ppi 
        ON pp.parts_id = ppi.parts_id 
    LEFT JOIN number_unit 
        ON pp.unit_structure_id = number_unit.unit_id 
    LEFT JOIN unit_round 
        ON pp.factory_id = unit_round.factory_id 
WHERE
    pp.parts_id = @PartsId
