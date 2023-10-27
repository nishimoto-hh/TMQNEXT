--******************************************************************
--入庫入力(予備品情報)
--******************************************************************
WITH number_unit AS ( 
    -- 数量管理単位
    SELECT
        structure_id AS unit_id
        , translation_text AS unit_name
        , ex.extension_data AS unit_digit 
    FROM
        v_structure_item_all unit 
        LEFT JOIN ms_item_extension ex 
            ON unit.structure_item_id = ex.item_id 
            AND ex.sequence_no = 2 
    WHERE
        unit.structure_group_id = 1730 
        AND unit.language_id = @LanguageId
) 
, unit_round AS ( 
    --丸め処理区分
    SELECT
        ms.factory_id
        , ex.extension_data AS round_division 
    FROM
        ms_structure ms 
        LEFT JOIN ms_item item 
            ON ms.structure_item_id = item.item_id 
            AND item.delete_flg = 0 
        LEFT JOIN ms_item_extension ex 
            ON item.item_id = ex.item_id 
            AND ex.sequence_no = 1 
    WHERE
        ms.structure_group_id = 2050 
        --AND ms.delete_flg = 0
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
    , number_unit.unit_name                     --数量管理単位
    , pp.factory_id                             --工場ID
    , COALESCE(number_unit.unit_digit, 0) AS unit_digit     --小数点以下桁数(数量)
    , COALESCE(unit_round.round_division, 0) AS round_division  --丸め処理区分
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
