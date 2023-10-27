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
@AddItem*/
ORDER BY
    tr.translation_id