--******************************************************************
--棚IDより倉庫を取得する
--******************************************************************
WITH factory AS ( 
    --予備品倉庫
    SELECT
        structure_id
        , translation_text 
    FROM
        v_structure_item 
    WHERE
        structure_group_id = 1040
        AND structure_layer_no = 2 --倉庫
        AND language_id = @LanguageId
) 
SELECT
    factory.translation_text 
FROM
    v_structure_item vsi 
    LEFT JOIN factory 
        ON vsi.parent_structure_id = factory.structure_id 
WHERE
    vsi.structure_group_id = 1040
    AND vsi.structure_layer_no = 3 --棚
    AND vsi.structure_id = @PartsLocationId