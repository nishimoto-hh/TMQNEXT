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
ORDER BY
    IIF(ord.display_order IS NOT NULL, 0, 1)
    , ord.display_order
    , st.structure_id
