SELECT
    item.structure_id AS stroke_id,                                  -- 工程ID(構成ID)
    item.parent_structure_id AS stroke_parent_id,                    -- 工程の親構成ID
    item.item_translation_id AS stroke_item_translation_id,          -- 翻訳ID
    ROW_NUMBER() OVER(ORDER BY item.structure_id ASC) stroke_number, -- 工程番号
    item.translation_text AS stroke_name                             -- 工程名
FROM
    v_structure_item item
WHERE
    item.structure_group_id = 1000
AND item.structure_layer_no = 4
AND item.language_id = @LanguageId
AND item.factory_id in @FactoryIdList
