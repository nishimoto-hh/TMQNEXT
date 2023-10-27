/*
* 機種別仕様登録画面
* 言語一覧を取得するSQL
*/
WITH lang AS(
     SELECT
         st.translation_text AS language_name
        ,ex.extension_data AS language_id
        ,COALESCE(COALESCE(od.display_order, od_com.display_order),st.structure_id) AS display_order
    FROM
        v_structure_item AS st
        INNER JOIN
            ms_item_extension AS ex
        ON  (
                st.structure_item_id = ex.item_id
            AND ex.sequence_no = 1
            )
        LEFT OUTER JOIN
            ms_structure_order AS od
        ON  (
                od.structure_id = st.structure_id
            AND od.factory_id = @FactoryId
            )
        LEFT OUTER JOIN
            ms_structure_order AS od_com
        ON  (
                od_com.structure_id = st.structure_id
            AND od_com.factory_id = 0
            )
    WHERE
        st.structure_group_id = 9020
)
,tra AS(
     SELECT
         sp.translation_id
        ,tra.translation_text
        ,tra.language_id
        ,tra.translation_item_description
        ,tra.location_structure_id
        ,tra.update_serialid
    FROM
        ms_spec AS sp
        INNER JOIN
            ms_translation AS tra
        ON  (
                tra.translation_id = sp.translation_id
            )
    WHERE
        EXISTS(
            SELECT
                *
            FROM
                ms_machine_spec_relation AS msr
            WHERE
                msr.machine_spec_relation_id = @MachineSpecRelationId
            AND msr.spec_id = sp.spec_id
        )
    AND tra.location_structure_id = @FactoryId
)
SELECT
     lang.language_name
    ,tra.translation_text
    -- 翻訳文字列(変更前退避)
    ,tra.translation_text AS translation_text_bk
    ,tra.translation_item_description
    ,lang.language_id
    ,tra.translation_id
    ,@FactoryId AS location_structure_id
    ,tra.update_serialid
FROM
    lang
    LEFT OUTER JOIN
        tra
    ON  (
            lang.language_id = tra.language_id
        )
ORDER BY
    display_order