SELECT
    item.structure_id,                                  -- 構成ID
    item.structure_group_id,                            -- 構成グループID
    item.factory_id,                                    -- 工場ID
    item.item_translation_id AS translation_id,         -- アイテム翻訳ID
    item.translation_text,                              -- アイテム翻訳
    item.parent_structure_id,                           -- 親構成アイテムID
    parent.translation_text AS parent_translation_text, -- 親構成アイテム翻訳
    ex1.extension_data AS ex_data1,                     -- 拡張項目1
    ex2.extension_data AS ex_data2,                     -- 拡張項目2
    ex3.extension_data AS ex_data3,                     -- 拡張項目3
    ---------------------------------- 以下は翻訳を取得 ----------------------------------
    COALESCE((
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
                        structure_factory AS st_f
                    WHERE
                        st_f.structure_id = item.factory_id
                    AND st_f.factory_id IN(0, item.factory_id)
                )
            AND tra.structure_id = item.factory_id
        ),(
            SELECT
                mt.translation_text
            FROM
                ms_translation mt
            WHERE
                mt.translation_id = 131070006
            AND mt.location_structure_id = 0
            AND mt.language_id = @LanguageId
        )) AS factory_name -- 工場名(アイテムの工場名が取得できる場合はアイテムの工場名、取得できない場合は「共通工場」)
FROM
    v_structure_item item
    LEFT JOIN
        v_structure_item parent
    ON  item.parent_structure_id = parent.structure_id
    LEFT JOIN
        ms_item_extension ex1 -- 拡張項目1
    ON  item.structure_item_id = ex1.item_id
    AND ex1.sequence_no = 1
    LEFT JOIN
        ms_item_extension ex2 -- 拡張項目2
    ON  item.structure_item_id = ex2.item_id
    AND ex2.sequence_no = 2
    LEFT JOIN
        ms_item_extension ex3 -- 拡張項目3
    ON  item.structure_item_id = ex3.item_id
    AND ex3.sequence_no = 3
WHERE
    item.structure_group_id = @StructureGroupId
AND item.language_id = @LanguageId