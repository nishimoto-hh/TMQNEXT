-- 言語リスト
WITH language_list AS ( 
    SELECT
        ROW_NUMBER() OVER ( 
            ORDER BY
                IIF(ord.display_order IS NOT NULL, 0, 1)
                , ord.display_order
                , st.structure_id
        ) AS num                                      -- 表示順
        , tr.translation_text AS lan_trans_name       -- 言語名称
        , it_ex.extension_data AS language_id         -- 言語ID
    FROM
        ms_structure as st 
        LEFT JOIN ms_item AS it 
            ON st.structure_item_id = it.item_id 
        LEFT JOIN ms_item_extension AS it_ex 
            ON it.item_id = it_ex.item_id 
            AND it_ex.sequence_no = 1 
        LEFT JOIN ms_translation AS tr 
            ON it.item_translation_id = tr.translation_id 
            AND tr.location_structure_id = 0 
            AND tr.language_id = @LanguageId
        -- 表示順
        LEFT JOIN ms_structure_order AS ord 
            ON st.structure_id = ord.structure_id
            AND ord.factory_id = 0
    WHERE
        st.structure_group_id = 9020
        AND st.delete_flg = 0
)
-- アイテム
, item as (
    SELECT
        st.structure_id                                -- 構成ID
        , st.structure_group_id                        -- 構成グループID
        , st.factory_id                                -- 工場ID
        , st.structure_item_id AS item_id              -- アイテムID
        , it.item_translation_id AS translation_id     -- アイテム翻訳ID
        , it.update_serialid AS item_update_serialid   -- 更新シリアルID
    FROM
        ms_structure AS st
        LEFT JOIN ms_item AS it 
            ON st.structure_item_id = it.item_id
    WHERE
        st.structure_id = @StructureId
)

SELECT
    lan.num                                            -- 表示順
    , lan.lan_trans_name                               -- 言語名称
    , lan.language_id                                  -- 言語ID
    , it.structure_id                                  -- 構成ID
    , it.structure_group_id                            -- 構成グループID
    , tr.location_structure_id                         -- 翻訳工場ID
    , it.item_id                                       -- アイテムID
    , it.translation_id AS translation_id              -- アイテム翻訳ID
    , tr.translation_text AS translation_text          -- アイテム翻訳名称
    , tr.translation_text AS translation_text_bk       -- アイテム翻訳名称(初期表示)
    , tr.update_serialid                               -- 更新シリアルID(翻訳マスタ)
    , it.item_update_serialid                          -- 更新シリアルID(アイテムマスタ)
FROM
    language_list AS lan
    CROSS JOIN item AS it
    LEFT JOIN ms_translation AS tr
        ON it.translation_id = tr.translation_id 
        AND tr.location_structure_id = @FactoryId
        AND tr.language_id = lan.language_id
ORDER BY
    lan.num
