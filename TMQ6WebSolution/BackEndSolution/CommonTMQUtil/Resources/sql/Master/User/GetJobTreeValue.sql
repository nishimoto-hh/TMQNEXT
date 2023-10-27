/*
 構成ID(工場、職種)より、工場と職種を取得するSQL
*/
WITH factory AS ( 
    --工場
    SELECT
        structure_id
        , translation_text 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id = 1000 
        AND structure_layer_no = 1 
        AND structure_id IN @locationIdList
        AND language_id = @LanguageId
) 
SELECT
    COALESCE(factory.structure_id, 0) AS location_structure_id
    , vsi.structure_id AS job_structure_id
FROM
    v_structure_item_all vsi 
    LEFT JOIN factory 
        ON vsi.factory_id = factory.structure_id 
WHERE
    vsi.structure_group_id = 1010               --職種
    AND vsi.structure_id IN @jobIdList
ORDER BY
    vsi.factory_id
    , vsi.structure_id