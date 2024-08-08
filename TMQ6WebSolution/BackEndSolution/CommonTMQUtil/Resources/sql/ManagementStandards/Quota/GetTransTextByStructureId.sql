-- 構成IDより翻訳テキストを取得(工場アイテム)
SELECT
    item.translation_text 
FROM
    v_structure_item_all item 
WHERE
    item.structure_id = @StructureId 
    AND item.language_id = @LanguageId
