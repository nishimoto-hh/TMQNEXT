SELECT
    item.structure_id AS warehouse_id,                                  -- 予備品倉庫ID(構成ID)
    item.parent_structure_id AS warehouse_parent_id,                    -- 予備品倉庫の親構成ID
    item.item_translation_id AS warehouse_item_translation_id,          -- 翻訳ID
    ROW_NUMBER() OVER(ORDER BY item.structure_id ASC) warehouse_number, -- 予備品倉庫番号
    item.translation_text AS warehouse_name                             -- 予備品倉庫名
FROM
    v_structure_item item
WHERE
    item.structure_group_id = 1040
AND item.structure_layer_no = 2
AND item.language_id = @LanguageId
AND item.factory_id in @FactoryIdList
