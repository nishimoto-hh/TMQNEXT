--******************************************************************
--出庫一覧(予備品情報)
--******************************************************************
WITH inventory AS ( 
    SELECT
        parts_id
        , stock_quantity 
    FROM
        v_pt_parts_inventory 
    WHERE
        parts_id = @PartsId
) 
,number_unit AS -- 小数点以下桁数(数値)
(
    SELECT
        ms.structure_id,
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
, unit_round AS(
    --丸め処理区分
    SELECT
        ms.factory_id,
        ex.extension_data AS unit_round_division
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
SELECT DISTINCT
    pps.parts_no                                         --予備品No
    , pps.parts_name                                     --予備品名称
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
                  st_f.structure_id = pps.manufacturer_structure_id
              AND st_f.factory_id IN(0, pps.factory_id)
           )
      AND tra.structure_id = pps.manufacturer_structure_id
    ) AS manufacturer_name                                 -- メーカー名
    --, mft.translation_text AS manufacturer_name          --メーカー名
    , pps.model_type                                     --型式
    , pps.standard_size AS dimensions                    --規格・寸法
    , ivt.stock_quantity AS inventry                     --在庫数
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
                  st_f.structure_id = pps.unit_structure_id
              AND st_f.factory_id IN(0, pps.factory_id)
           )
      AND tra.structure_id = pps.unit_structure_id
    ) AS unit  -- 数量管理単位
    --, number_unit.unit_name AS unit                      --(単位)
    , COALESCE(number_unit.unit_digit, 0) AS unit_digit  --小数点以下桁数(数量)
    , COALESCE(unit_round.unit_round_division, 0) AS unit_round_division  --丸め処理区分(数量)
FROM
    pt_parts pps 
    LEFT JOIN pt_lot plt
        ON pps.parts_id = plt.parts_id 
    LEFT JOIN inventory ivt 
        ON pps.parts_id = ivt.parts_id 
    LEFT JOIN number_unit
        ON plt.unit_structure_id = number_unit.structure_id
    LEFT JOIN unit_round --丸め処理区分
        ON  pps.factory_id = unit_round.factory_id
WHERE
    pps.parts_id = @PartsId
