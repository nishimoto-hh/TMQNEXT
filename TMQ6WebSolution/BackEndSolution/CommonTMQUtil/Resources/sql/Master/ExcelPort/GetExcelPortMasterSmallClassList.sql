SELECT
    item.structure_id AS small_class_id,                                  -- 機種小分類ID(構成ID)
    item.parent_structure_id AS small_class_parent_id,                    -- 機種小分類の親構成ID
    item.item_translation_id AS small_class_item_translation_id,          -- 翻訳ID
    ROW_NUMBER() OVER(ORDER BY item.structure_id ASC) small_class_number, -- 機種小分類番号
    item.translation_text AS small_class_name                             -- 機種小分類名
FROM
    v_structure_item item
WHERE
    item.structure_group_id = 1010
AND item.structure_layer_no = 3
AND item.language_id = @LanguageId
AND item.factory_id in @FactoryIdList
