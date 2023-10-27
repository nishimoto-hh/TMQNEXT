WITH master_name AS(
    SELECT
        ms.translation_text AS master_name
    FROM
        ms_translation ms
    WHERE
        ms.location_structure_id = 0
    AND ms.translation_id = @MasterTransLationId
    AND ms.language_id = @LanguageId
)
SELECT
    @FactoryId AS target_factory_id                                                                         -- 並び順対象工場ID 
    ,@ProcessId AS process_id                                                                               -- 送信時処理ID 
    ,pn.translation_text as process_name                                                                    -- 送信時処理名
    ,st.structure_id                                                                                        -- 構成ID
    ,st.structure_group_id                                                                                  -- 構成グループID
    ,master_name.master_name                                                                                -- マスタ種類
    ,ord.display_order                                                                                      -- 表示順
    ,st.factory_id                                                                                          -- 工場ID
    ,CASE st.factory_id
        WHEN 0 THEN ''
        ELSE '○'
    END factory_item_flg                                                                                    -- 工場アイテムフラグ
        ,(
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
                        st_f.structure_id = st.structure_id
                    AND st_f.factory_id IN(0, st.factory_id)
                )
            AND tra.structure_id = st.structure_id
        ) AS item_name                                                                                      -- アイテム翻訳名称 
        ,(
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
                        st_f.structure_id = st.factory_id
                    AND st_f.factory_id IN(0, st.factory_id)
                )
            AND tra.structure_id = st.factory_id
        ) AS factory_name                                                                                   -- 工場名称 

FROM
    ms_structure AS st
    LEFT JOIN
        ms_item AS it
    ON  st.structure_item_id = it.item_id
    -- 表示順
    LEFT JOIN
        ms_structure_order AS ord
    ON  st.structure_id = ord.structure_id
    AND ord.factory_id = @FactoryId
    LEFT JOIN
        master_name
    ON  1 = 1 -- 結合条件はない
    LEFT JOIN
        (
            SELECT
                item.translation_text
            FROM
                v_structure_item item
                LEFT JOIN
                    ms_item_extension ex
                ON  item.structure_item_id = ex.item_id
                AND ex.sequence_no = 1
            WHERE
                item.structure_group_id = 2120
            AND ex.extension_data = @ProcessId
            AND item.language_id = @LanguageId
        ) pn
    ON  1 = 1 -- 結合条件はない
WHERE
    st.structure_group_id = @StructureGroupId
AND (
        st.factory_id = 0
    OR  st.factory_id = @FactoryId
    )
-- 削除を除く
AND st.delete_flg <> 1

-- 未使用を除く
AND NOT EXISTS(
        SELECT
            structure_id
        FROM
            ms_structure_unused unused
        WHERE
            unused.structure_id = st.structure_id
        AND unused.factory_id = @FactoryId
    )
ORDER BY
    IIF(ord.display_order IS NOT NULL, 0, 1),
    ord.display_order,
    st.structure_id