-- 翻訳マスタ
WITH tr AS ( 
    SELECT
        translation_id    -- 翻訳ID
    FROM
        ms_translation 
    WHERE
        location_structure_id = @LocationStructureId 
        AND language_id = @LanguageId 
        AND translation_text = @TranslationText COLLATE Japanese_BIN
        AND ( 
            translation_id >= 200000000 
            AND translation_id < 600000000
        ) 
        AND delete_flg = 0
)

SELECT DISTINCT
    tr.translation_id    -- 翻訳ID
FROM
    tr
/*@AddItem
    INNER JOIN  ms_item AS it 
        ON tr.translation_id = it.item_translation_id
    INNER JOIN ms_structure AS ms
        ON it.item_id = ms.structure_item_id
        AND ms.structure_group_id = @StructureGroupId
        AND ms.factory_id = @FactoryId
        AND ms.structure_layer_no = @StructureLayerNo
@AddItem*/
/*@AddItem2
        AND ms.parent_structure_id = @ParentStructureId
@AddItem2*/
ORDER BY
    tr.translation_id