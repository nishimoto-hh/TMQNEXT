SELECT
    item.structure_id AS facility_id,                                  -- 設備ID(構成ID)
    item.parent_structure_id AS facility_parent_id,                    -- 設備の親構成ID
    item.item_translation_id AS facility_item_translation_id,          -- 翻訳ID
    ROW_NUMBER() OVER(ORDER BY item.structure_id ASC) facility_number, -- 設備番号
    item.translation_text AS facility_name                             -- 設備名
FROM
    v_structure_item item
WHERE
    item.structure_group_id = 1000
AND item.structure_layer_no = 5
AND item.language_id = @LanguageId
AND item.factory_id in @FactoryIdList
