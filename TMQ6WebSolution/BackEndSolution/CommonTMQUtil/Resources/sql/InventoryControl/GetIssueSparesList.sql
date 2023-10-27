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
, manufacturer AS ( 
    --メーカー
    SELECT
        structure_id
        , translation_text 
    FROM
        v_structure_item si 
    WHERE
        structure_group_id = 1150 
        AND language_id = @LanguageId
) 
, number_unit AS(
    --数量管理単位
    SELECT
        structure_id AS unit_id,
        translation_text AS unit_name,
        ex.extension_data AS unit_digit
    FROM
        v_structure_item unit
        LEFT JOIN
            ms_item_extension ex
        ON  unit.structure_item_id = ex.item_id
        AND ex.sequence_no = 2
    WHERE
        unit.structure_group_id = 1730
    AND unit.language_id = @LanguageId
)
, unit_round AS(
    --丸め処理区分(数量管理単位)
    SELECT
        ms.factory_id,
        ex.extension_data AS unit_round_division
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item item
        ON  ms.structure_item_id = item.item_id
        AND item.delete_flg = 0
        LEFT JOIN
            ms_item_extension ex
        ON  item.item_id = ex.item_id
        AND ex.sequence_no = 1
    WHERE
        ms.structure_group_id = 2050
    AND ms.delete_flg = 0
)
SELECT DISTINCT
    pps.parts_no                                         --予備品No
    , pps.parts_name                                     --予備品名称
    , mft.translation_text AS maker                      --メーカーID
    , pps.model_type                                     --型式
    , pps.standard_size AS dimensions                    --規格・寸法
    , ivt.stock_quantity                                 --在庫数
    , number_unit.unit_name AS unit                      --(単位)
    , COALESCE(number_unit.unit_digit, 0) AS unit_digit  --小数点以下桁数(数量)
    , COALESCE(unit_round.unit_round_division, 0) AS unit_round_division  --丸め処理区分(数量)
FROM
    pt_parts pps 
    LEFT JOIN pt_lot plt
        ON pps.parts_id = plt.parts_id 
    LEFT JOIN inventory ivt 
        ON pps.parts_id = ivt.parts_id 
    LEFT JOIN manufacturer mft 
        ON pps.manufacturer_structure_id = mft.structure_id 
    LEFT JOIN number_unit --数量管理単位
        ON  plt.unit_structure_id = number_unit.unit_id
    LEFT JOIN unit_round --丸め処理区分(数量)
        ON  pps.factory_id = unit_round.factory_id
WHERE
    pps.parts_id = @PartsId
