SELECT
    st.structure_id                             -- 構成ID
    , st.structure_item_id AS item_id           -- アイテムID
    , ISNULL( 
        tr_item_factory.translation_text
        , tr_item_std.translation_text
    ) AS item_name                              -- アイテム翻訳名称
    , ord.display_order                         -- 表示順
FROM
    ms_structure AS st 
    LEFT JOIN ms_item AS it 
        ON st.structure_item_id = it.item_id
    -- 標準アイテム翻訳
    LEFT JOIN ms_translation AS tr_item_std 
        ON it.item_translation_id = tr_item_std.translation_id 
        AND tr_item_std.location_structure_id = 0 
        AND tr_item_std.language_id = @LanguageId
    -- 工場アイテム翻訳
    LEFT JOIN ms_translation AS tr_item_factory 
        ON it.item_translation_id = tr_item_factory.translation_id 
        AND tr_item_factory.location_structure_id = @FactoryId 
        AND tr_item_factory.language_id = @LanguageId
    -- 表示順
    LEFT JOIN ms_structure_order AS ord 
        ON st.structure_id = ord.structure_id 
        AND ord.factory_id = @FactoryId 
WHERE
    st.structure_group_id = @StructureGroupId 
    AND (st.factory_id = 0 OR st.factory_id = @FactoryId) 
    AND st.delete_flg = 0
    -- 未使用の標準アイテムを除く
    AND NOT EXISTS ( 
        SELECT
            structure_id 
        FROM
            ms_structure_unused unused
        WHERE
            st.structure_id = unused.structure_id
            AND unused.structure_group_id = @StructureGroupId 
            AND unused.factory_id = @FactoryId
    )
