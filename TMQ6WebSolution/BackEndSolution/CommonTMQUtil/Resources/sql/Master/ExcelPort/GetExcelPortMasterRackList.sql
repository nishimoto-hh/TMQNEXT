SELECT
    item.structure_id AS rack_id,                                  -- 棚ID(構成ID)
    item.parent_structure_id AS rack_parent_id,                    -- 棚の親構成ID
    item.item_translation_id AS rack_item_translation_id,          -- 翻訳ID
    ROW_NUMBER() OVER(ORDER BY item.structure_id ASC) rack_number, -- 棚番号
    item.translation_text AS rack_name                             -- 棚名
FROM
    v_structure_item item
WHERE
    item.structure_group_id = 1040
AND item.structure_layer_no = 3
AND item.language_id = @LanguageId
AND item.factory_id in @FactoryIdList
