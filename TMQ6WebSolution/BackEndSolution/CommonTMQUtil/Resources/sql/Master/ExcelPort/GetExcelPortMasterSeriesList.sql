SELECT
    item.structure_id AS series_id,                                  -- 系列ID(構成ID)
    item.parent_structure_id AS series_parent_id,                    -- 系列の親構成ID
    item.item_translation_id AS series_item_translation_id,          -- 翻訳ID
    ROW_NUMBER() OVER(ORDER BY item.structure_id ASC) series_number, -- 系列番号
    item.translation_text AS series_name                             -- 系列名
FROM
    v_structure_item item
WHERE
    item.structure_group_id = 1000
AND item.structure_layer_no = 3
AND item.language_id = @LanguageId
AND item.factory_id in @FactoryIdList
