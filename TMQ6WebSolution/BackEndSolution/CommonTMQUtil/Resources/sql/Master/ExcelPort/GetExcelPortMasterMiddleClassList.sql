SELECT
    item.structure_id AS middle_class_id,                                  -- 機種中分類ID(構成ID)
    item.parent_structure_id AS middle_class_parent_id,                    -- 機種中分類の親構成ID
    item.item_translation_id AS middle_class_item_translation_id,          -- 翻訳ID
    ROW_NUMBER() OVER(ORDER BY item.structure_id ASC) middle_class_number, -- 機種中分類番号
    item.translation_text AS middle_class_name                             -- 機種中分類名
FROM
    v_structure_item item
WHERE
    item.structure_group_id = 1010
AND item.structure_layer_no = 2
AND item.language_id = @LanguageId
AND item.factory_id in @FactoryIdList
