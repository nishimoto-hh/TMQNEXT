SELECT
    item.structure_id AS plant_id,                                  -- プラントID(構成ID)
    item.parent_structure_id AS plant_parent_id,                    -- プラントの親構成ID
    item.item_translation_id AS plant_item_translation_id,          -- 翻訳ID
    ROW_NUMBER() OVER(ORDER BY item.structure_id ASC) plant_number, -- プラント番号
    item.translation_text AS plant_name                             -- プラント名
FROM
    v_structure_item item
WHERE
    item.structure_group_id = 1000
AND item.structure_layer_no = 2
AND item.language_id = @LanguageId
AND item.factory_id in @FactoryIdList
