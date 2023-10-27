--******************************************************************
--指定工場に紐づく職種の件数を取得するSQL
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
        AND structure_id = @LocationStructureId
        AND language_id = @LanguageId
) 
SELECT
    COUNT(*) AS count
FROM
    v_structure_item_all vsi 
    RIGHT JOIN factory 
        ON vsi.factory_id = factory.structure_id 
WHERE
    vsi.structure_group_id = 1010   --職種
AND vsi.structure_layer_no = 1
