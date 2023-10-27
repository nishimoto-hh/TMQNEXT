SELECT
    item.structure_item_id AS job_item_id,                        -- アイテムID
    item.structure_id AS job_id,                                  -- 職種ID(構成ID)
    item.factory_id AS job_parent_id,                             -- 職種の親構成ID
    item.item_translation_id AS job_item_translation_id,          -- 翻訳ID
    ROW_NUMBER() OVER(ORDER BY item.structure_id ASC) job_number, -- 職種番号
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
        ) AS job_name,                                           -- 職種名
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
        ) AS job_name_before,                                     -- 職種名(変更前)
    ex.extension_data AS job_code_val,                            -- 保全実績集計職種コード
    ex.extension_data AS job_code                                 -- 保全実績集計職種コード
FROM
    v_structure_item_all item
    LEFT JOIN
        ms_item_extension ex
    ON  item.structure_item_id = ex.item_id
    AND ex.sequence_no = 1
WHERE
    item.structure_group_id = 1010
AND item.structure_layer_no = 0
AND item.delete_flg = 0
AND item.language_id = @LanguageId
AND item.factory_id IN @FactoryIdList
AND EXISTS (SELECT * FROM ms_structure ms WHERE (item.parent_structure_id = 0 OR item.parent_structure_id IS NULL OR COALESCE(ms.structure_id, 0) = COALESCE(item.parent_structure_id, 0)) AND ms.delete_flg = 0)

ORDER BY job_number