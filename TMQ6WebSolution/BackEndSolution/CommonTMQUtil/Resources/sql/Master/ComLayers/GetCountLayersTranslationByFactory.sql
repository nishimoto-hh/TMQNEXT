-- –|–óƒ}ƒXƒ^
WITH tr AS (  
    SELECT
        translation_id 
    FROM
        ms_translation 
    WHERE
        ( 
            location_structure_id = 0 
            OR location_structure_id = @LocationStructureId
        ) 
        AND language_id = @LanguageId 
        AND translation_text = @TranslationText COLLATE Japanese_BIN
        AND delete_flg = 0
)

SELECT
    COUNT(st.structure_id) 
FROM
    ms_structure AS st 
    INNER JOIN ms_item AS it 
        ON st.structure_item_id = it.item_id 
    INNER JOIN tr 
        ON it.item_translation_id = tr.translation_id 
WHERE
    st.structure_group_id = @StructureGroupId
    AND st.factory_id = @FactoryId
    AND ISNULL(st.structure_layer_no, 0) = ISNULL(@StructureLayerNo, 0)
    AND ISNULL(st.parent_structure_id, 0) = ISNULL(@ParentStructureId, 0)
