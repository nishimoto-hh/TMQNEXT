--******************************************************************
--予備品　勘定科目の初期表示値(B4140:設備貯蔵品)を取得する
--******************************************************************
SELECT
    st.structure_id AS account_structure_id
    , ex.extension_data AS account_cd
    , ex2.extension_data AS account_old_new_division
    , st.translation_text AS account_name 
FROM
    v_structure_item AS st 
    INNER JOIN ms_item_extension AS ex 
        ON ex.item_id = st.structure_item_id 
        AND ex.sequence_no = 1 
    INNER JOIN ms_item_extension AS ex2 
        ON ex2.item_id = st.structure_item_id 
        AND ex2.sequence_no = 2 
WHERE
    st.structure_group_id = 1770 
    AND ex.extension_data = 'B4140'
    AND st.language_id = @LanguageId 
