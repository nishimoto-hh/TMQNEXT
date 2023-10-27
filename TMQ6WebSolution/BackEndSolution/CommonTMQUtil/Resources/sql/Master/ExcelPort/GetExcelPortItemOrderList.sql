WITH max_order AS(
    SELECT
        MAX(update_datetime) AS order_update_datetime
    FROM
        ms_structure_order
    WHERE
        structure_group_id = @StructureGroupId
    AND factory_id = @FactoryId
),
main_order AS(
    SELECT
        ord.factory_id,
        ord.structure_id,
        ord.display_order,
        max_ord.order_update_datetime
    FROM
        ms_structure_order AS ord
        CROSS JOIN
            max_order AS max_ord
    WHERE
        ord.structure_group_id = @StructureGroupId
    AND ord.factory_id = @FactoryId
),
master_name AS(
    SELECT
        ms.translation_text AS master_name
    FROM
        ms_translation ms
    WHERE
        ms.translation_id = @MasterTransLationId
    AND ms.location_structure_id = 0
    AND ms.language_id = @LanguageId
)
SELECT
    st.structure_id                                                                                         -- 構成ID
    ,st.structure_group_id                                                                                  -- 構成グループID
    ,master_name.master_name,                                                                               -- マスタ種類
    CASE st.factory_id
        WHEN 0 THEN ''
        ELSE '○'
    END factory_item_flg                                                                                    -- 工場アイテムフラグ
    ,dbo.get_translation_text_all(st.structure_id, @FactoryId, @StructureGroupId, @LanguageId) AS item_name -- アイテム翻訳名称 
    ,st.factory_id                                                                                          -- 工場ID
    ,dbo.get_translation_text_all(st.factory_id, @FactoryId, 1000, @LanguageId) AS factory_name             -- 工場名称 
    ,ord.display_order                                                                                      -- 表示順
FROM
    ms_structure AS st
    LEFT JOIN
        ms_item AS it
    ON  st.structure_item_id = it.item_id -- 表示順
    LEFT JOIN
        main_order AS ord
    ON  st.structure_id = ord.structure_id
    AND ord.factory_id = @FactoryId
    LEFT JOIN
        master_name
    ON  1 = 1 -- 結合条件はない
WHERE
    st.structure_group_id = @StructureGroupId
AND (
        st.factory_id = 0
    OR  st.factory_id = @FactoryId
    ) -- 削除を除く
AND st.delete_flg <> 1 -- 未使用を除く
AND NOT EXISTS(
        SELECT
            structure_id
        FROM
            ms_structure_unused unused
        WHERE
            unused.structure_group_id = @StructureGroupId
        AND unused.factory_id = @FactoryId
        AND unused.structure_id = st.structure_id
    )
ORDER BY
    IIF(ord.display_order IS NOT NULL, 0, 1),
    ord.display_order,
    st.structure_id