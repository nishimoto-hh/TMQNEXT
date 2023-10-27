SELECT
    item.structure_id AS department_id,                                  -- 部門ID(構成ID)
    item.factory_id AS department_parent_id,                             -- 部門の親構成ID
    item.item_translation_id AS department_item_translation_id,          -- 翻訳ID
    ROW_NUMBER() OVER(ORDER BY item.structure_id ASC) department_number, -- 部門番号
    item.translation_text AS department_name,                            -- 部門名
    ex1.extension_data AS department_code,                               -- 部門コード
    ex2.extension_data AS fix_division                                   -- 修理部門
FROM
    v_structure_item item
    LEFT JOIN
        ms_item_extension ex1 -- 部門コード
    ON  item.structure_item_id = ex1.item_id
    AND ex1.sequence_no = 1
    LEFT JOIN
        ms_item_extension ex2 -- 修理部門
    ON  item.structure_item_id = ex2.item_id
    AND ex2.sequence_no = 2
WHERE
    item.structure_group_id = 1760
AND item.language_id = @LanguageId
AND item.factory_id in @FactoryIdList