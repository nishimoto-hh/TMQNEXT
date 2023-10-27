/*
 構成ID(工場)より、上位の地区を取得するSQL
*/
WITH location AS ( 
    --地区
    SELECT
        structure_id
        , translation_text 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id = 1000 
        AND structure_layer_no = 0
        AND language_id = @LanguageId
) 
SELECT
    location.structure_id AS district_id
    , location.translation_text AS district_name
    , vsi.structure_id AS factory_id
    , vsi.translation_text AS factory_name
FROM
    v_structure_item_all vsi 
    INNER JOIN location 
        ON vsi.parent_structure_id = location.structure_id 
WHERE
    vsi.structure_group_id = 1000
    AND vsi.structure_layer_no = 1 --工場
    AND vsi.structure_id = @FactoryId
    AND vsi.language_id = @LanguageId
ORDER BY
    vsi.factory_id
    , vsi.structure_id