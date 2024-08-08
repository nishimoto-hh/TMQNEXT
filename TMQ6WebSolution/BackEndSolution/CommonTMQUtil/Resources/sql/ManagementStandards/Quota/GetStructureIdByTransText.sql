-- 翻訳から指定された工場アイテムの構成IDを逆引きする
SELECT
    item.structure_id 
FROM
    v_structure_item_all item 
WHERE
    item.structure_group_id = @StructureGroupId
    AND item.language_id = @LanguageId
    AND item.factory_id = @FactoryId 
    AND item.translation_text = @TranslationText
