--******************************************************************
--予備品　新旧区分の初期表示値(0:新品)を取得する
--******************************************************************
SELECT
    st.structure_id AS old_new_structure_id
    , ex.extension_data AS old_new_division_cd
    , st.translation_text AS old_new_division_name 
FROM
    v_structure_item AS st 
    INNER JOIN ms_item_extension AS ex 
        ON ex.item_id = st.structure_item_id 
        AND ex.sequence_no = 1 
WHERE
    st.structure_group_id = 1940 
    AND ex.extension_data = '0' 
    AND st.language_id = @LanguageId