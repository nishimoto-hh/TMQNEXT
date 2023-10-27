/*
* 仕様項目選択肢一覧画面
* 仕様項目選択肢一覧を取得するSQL
*/
-- 選択肢の項目と、その拡張項目の仕様項目IDを取得
WITH selects AS(
     SELECT
         st.translation_text
        ,st.translation_item_description
        ,ord.display_order AS display_order
        ,st.delete_flg
        ,ex.extension_data AS spec_id
        ,st.structure_id
    FROM
        v_structure_item_all AS st
        INNER JOIN
            ms_item_extension AS ex
        ON  (
                st.structure_item_id = ex.item_id
            AND ex.sequence_no = 1
            )
        LEFT OUTER JOIN
            ms_structure_order AS ord
        ON  (
                ord.factory_id = st.factory_id
            AND ord.structure_id = st.structure_id
            )
    WHERE
        st.structure_group_id = 1340
    AND st.factory_id = @FactoryId
    AND st.language_id = @LanguageId
    AND NOT EXISTS(
            SELECT
                *
            FROM
                ms_structure_unused AS unused
            WHERE
                unused.structure_id = st.structure_id
            AND unused.factory_id = st.factory_id
        )
)
SELECT
     tra.translation_text AS spec_name
    ,sel.translation_text AS item_name
    ,sel.translation_item_description AS description
    ,sel.display_order
    ,sel.delete_flg
    ,sel.structure_id
FROM
    ms_spec AS sp
    INNER JOIN
        ms_translation AS tra
    ON  (
            sp.translation_id = tra.translation_id
        AND tra.location_structure_id = @FactoryId
        AND tra.language_id = @LanguageId
        )
    LEFT OUTER JOIN
        selects AS sel
    ON  (
            cast(sp.spec_id AS varchar) = sel.spec_id
        )
WHERE
    sp.spec_id = @SpecId