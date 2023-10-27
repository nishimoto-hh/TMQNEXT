SELECT
     item.structure_item_id AS department_item_id,                       -- アイテムID
    item.structure_id AS department_id,                                  -- 部門ID(構成ID)
    item.factory_id AS department_parent_id,                             -- 部門の親構成ID
    item.item_translation_id AS department_item_translation_id,          -- 翻訳ID
    ROW_NUMBER() OVER(ORDER BY item.structure_id ASC) department_number, -- 部門番号
        (
            SELECT
                tra.translation_text
            FROM
                v_structure_item_all AS tra
            WHERE
                tra.language_id = @LanguageId
            AND tra.location_structure_id = (
                    SELECT
                        MAX(st_f.factory_id)
                    FROM
                        #temp_structure_factory AS st_f
                    WHERE
                        st_f.structure_id = item.structure_id
                    AND st_f.factory_id IN(0, item.factory_id)
                )
            AND tra.structure_id = item.structure_id
        ) AS department_name,                                             -- 部門名
        (
            SELECT
                tra.translation_text
            FROM
                v_structure_item_all AS tra
            WHERE
                tra.language_id = @LanguageId
            AND tra.location_structure_id = (
                    SELECT
                        MAX(st_f.factory_id)
                    FROM
                        #temp_structure_factory AS st_f
                    WHERE
                        st_f.structure_id = item.structure_id
                    AND st_f.factory_id IN(0, item.factory_id)
                )
            AND tra.structure_id = item.structure_id
        ) AS department_name_before,                                     -- 部門名(変更前)
    ex1.extension_data AS department_code,                               -- 部門コード
    ex2.extension_data AS fix_division_val,                              -- 修理部門
    ex2.extension_data AS fix_division                                   -- 修理部門
FROM
    v_structure item
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
AND item.factory_id in @FactoryIdList
AND EXISTS (SELECT * FROM ms_structure ms WHERE ms.structure_id = item.factory_id AND ms.delete_flg = 0)

ORDER BY department_number