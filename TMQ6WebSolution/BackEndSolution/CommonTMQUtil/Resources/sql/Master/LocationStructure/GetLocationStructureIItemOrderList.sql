WITH max_order AS ( 
    SELECT
        MAX(update_datetime) AS order_update_datetime 
    FROM
        ms_structure_order 
    WHERE
        structure_group_id = @StructureGroupId 
        AND structure_id IN ( 
        SELECT
            st.structure_id 
        FROM
            ms_structure AS st 
        WHERE
            st.structure_group_id = @StructureGroupId
            AND structure_layer_no = @StructureLayerNo
            AND parent_structure_id = @ParentStructureId
    )
) 
, main_order AS ( 
    SELECT
        ord.factory_id
        , ord.structure_id
        , ord.display_order
        , max_ord.order_update_datetime
    FROM
        ms_structure_order AS ord 
        CROSS JOIN max_order AS max_ord 
    WHERE
        ord.structure_group_id = @StructureGroupId 
        AND structure_id IN ( 
        SELECT
            st.structure_id 
        FROM
            ms_structure AS st 
        WHERE
            st.structure_group_id = @StructureGroupId
            AND structure_layer_no = @StructureLayerNo
            AND parent_structure_id = @ParentStructureId
    )
)
SELECT
    st.structure_id                             -- 構成ID
    , st.structure_group_id                     -- 構成グループID
    , st.structure_item_id AS item_id           -- アイテムID
    , it.item_translation_id AS translation_id  -- アイテム翻訳ID
    , dbo.get_translation_text_all( 
        st.structure_id
        , 0
        , @StructureGroupId
        , @LanguageId
    ) AS translation_text                       -- アイテム翻訳名称
    , ord.display_order                         -- 表示順
    , CASE st.factory_id 
        WHEN 0
            THEN 1 
        ELSE 0 
        END standard_item_flg                   -- 標準アイテムフラグ
    , ord.order_update_datetime                 -- 最大更新日時
FROM
    ms_structure AS st 
    LEFT JOIN ms_item AS it 
        ON st.structure_item_id = it.item_id
    -- 表示順
    LEFT JOIN main_order AS ord 
        ON st.structure_id = ord.structure_id
        AND ord.factory_id = @FactoryId
WHERE
    st.structure_group_id = @StructureGroupId
    AND st.structure_layer_no = @StructureLayerNo
    AND st.parent_structure_id = @ParentStructureId
    -- 削除を除く
    AND st.delete_flg <> 1
ORDER BY
    IIF(ord.display_order IS NOT NULL, 0, 1)
    , ord.display_order
    , st.structure_id
