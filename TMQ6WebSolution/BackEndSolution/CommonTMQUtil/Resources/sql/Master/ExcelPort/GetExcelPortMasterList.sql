WITH structure_factory AS(
    SELECT
        structure_id,
        location_structure_id AS factory_id
    FROM
        v_structure_item_all
    WHERE
        structure_group_id IN(1000, @StructureGroupId)
    AND language_id = @LanguageId
),
target AS(
    SELECT
        ms.structure_id,
        ms.structure_group_id,
        ms.factory_id,
        mi.item_translation_id AS translation_id,
        ex1.extension_data AS ex_data1,-- 拡張項目1
        ex2.extension_data AS ex_data2,-- 拡張項目2
        ex3.extension_data AS ex_data3,-- 拡張項目3
        ---------------------------------- 以下は翻訳を取得 ----------------------------------
        (
            SELECT
                tra.translation_text
            FROM
                v_structure_item_all AS tra
            WHERE
                tra.language_id = @LanguageId
            AND tra.location_structure_id = (
                    SELECT
                        MIN(st_f.factory_id)
                    FROM
                        structure_factory AS st_f
                    WHERE
                        st_f.structure_id = ms.structure_id
                    AND st_f.factory_id IN(0, ms.factory_id)
                )
            AND tra.structure_id = ms.structure_id
        ) AS translation_text, -- アイテム翻訳
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
                        structure_factory AS st_f
                    WHERE
                        st_f.structure_id = ms.factory_id
                    AND st_f.factory_id IN(0, ms.factory_id)
                )
            AND tra.structure_id = ms.factory_id
        ) AS factory_name -- 工場名
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item mi
        ON  ms.structure_item_id = mi.item_id
        LEFT JOIN
            ms_item_extension ex1 -- 拡張項目1
        ON  ms.structure_item_id = ex1.item_id
        AND ex1.sequence_no = 1
        LEFT JOIN
            ms_item_extension ex2 -- 拡張項目2
        ON  ms.structure_item_id = ex2.item_id
        AND ex2.sequence_no = 2
        LEFT JOIN
            ms_item_extension ex3 -- 拡張項目3
        ON  ms.structure_item_id = ex3.item_id
        AND ex3.sequence_no = 3
    WHERE
        ms.structure_group_id = @StructureGroupId
    AND ms.delete_flg = 0
)
SELECT
    target.structure_id,                                -- 構成ID
    target.structure_group_id,                          -- 構成グループID
    target.factory_id,                                  -- 工場ID
    target.factory_id AS factory_id_before,             -- 初期表示時の工場ID(入力チェック時に使用)
    target.factory_name,                                -- 工場名
    target.translation_id,                              -- アイテム翻訳ID
    target.translation_text,                            -- アイテム翻訳
    target.translation_text AS translation_text_before, -- 初期表示時のアイテム翻訳(入力チェックに使用)
    target.ex_data1,                                    -- 拡張項目1
    target.ex_data2,                                    -- 拡張項目2
    target.ex_data3                                     -- 拡張項目3
FROM
    target