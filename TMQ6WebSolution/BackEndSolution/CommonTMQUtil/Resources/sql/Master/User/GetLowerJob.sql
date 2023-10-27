--******************************************************************
--構成IDより所属マスタに登録する構成IDを取得する(職種)
--******************************************************************
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
        AND language_id = @LanguageId
) 
SELECT
    vsi.structure_id AS location_structure_id
FROM
    v_structure_item vsi 
    LEFT JOIN factory 
        ON vsi.factory_id = factory.structure_id 
WHERE
    vsi.structure_group_id = 1010               --職種
    AND vsi.structure_layer_no = 0
    AND (factory.structure_id IN @jobIdList    --工場もしくは職種の構成ID
    OR vsi.structure_id IN @jobIdList)
ORDER BY
    vsi.factory_id
    , vsi.structure_id