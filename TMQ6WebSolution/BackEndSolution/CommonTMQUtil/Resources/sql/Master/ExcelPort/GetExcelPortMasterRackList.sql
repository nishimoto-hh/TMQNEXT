SELECT
    item.structure_item_id AS rack_item_id,                        -- アイテムID
    item.structure_id AS rack_id,                                  -- 棚ID(構成ID)
    item.parent_structure_id AS rack_parent_id,                    -- 棚の親構成ID
    item.item_translation_id AS rack_item_translation_id,          -- 翻訳ID
    ROW_NUMBER() OVER(ORDER BY item.structure_id ASC) rack_number, -- 棚番号
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
        ) AS rack_name,                                           -- 棚名
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
        ) AS rack_name_before                                      -- 棚名(変更前)
FROM
    v_structure_item_all item
WHERE
    item.structure_group_id = 1040
AND item.structure_layer_no = 3
AND item.delete_flg = 0
AND item.language_id = @LanguageId
AND item.factory_id in @FactoryIdList
AND EXISTS (SELECT * FROM ms_structure ms WHERE ms.structure_id = item.parent_structure_id AND ms.delete_flg = 0)

ORDER BY rack_number