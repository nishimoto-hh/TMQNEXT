SELECT
    st.structure_id                             -- 構成ID
    , st.factory_id AS location_structure_id    -- 工場ID
    , st.structure_item_id AS item_id           -- アイテムID
    , tr.translation_id AS translation_id       -- アイテム翻訳ID
    , tr.translation_text AS translation_text   -- アイテム翻訳名称
    , tr.update_serialid                        -- 更新シリアルID(翻訳マスタ)
    , it.update_serialid AS item_update_serialid   -- 更新シリアルID(アイテムマスタ)
FROM
    ms_structure AS st 
    LEFT JOIN ms_item AS it 
        ON st.structure_item_id = it.item_id
    -- 標準アイテム翻訳
    LEFT JOIN ms_translation AS tr
        ON it.item_translation_id = tr.translation_id 
        AND tr.location_structure_id = 0 
        AND tr.language_id = @LanguageId
WHERE
    st.structure_id = @StructureId
