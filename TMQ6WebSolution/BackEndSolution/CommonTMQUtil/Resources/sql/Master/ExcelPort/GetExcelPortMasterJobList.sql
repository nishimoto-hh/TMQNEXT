SELECT
    item.structure_id AS job_id,                                  -- 職種ID(構成ID)
    item.factory_id AS job_parent_id,                             -- 職種の親構成ID
    item.item_translation_id AS job_item_translation_id,          -- 翻訳ID
    ROW_NUMBER() OVER(ORDER BY item.structure_id ASC) job_number, -- 職種番号
    item.translation_text AS job_name,                            -- 職種名
    ex.extension_data AS job_code                                 -- 保全実績集計職種コード
FROM
    v_structure_item item
    LEFT JOIN
        ms_item_extension ex
    ON  item.structure_item_id = ex.item_id
    AND ex.sequence_no = 1
WHERE
    item.structure_group_id = 1010
AND item.structure_layer_no = 0
AND item.language_id = @LanguageId
AND item.factory_id IN @FactoryIdList