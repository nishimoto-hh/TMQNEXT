/*
 選択された構成IDより上位の工場を取得する(保全実績一覧)
*/
WITH st_com(structure_layer_no, structure_id, parent_structure_id, org_structure_id) AS(
    SELECT
        st.structure_layer_no,      --構成階層番号
        st.structure_id,            --構成ID
        st.parent_structure_id,     --親構成ID
        st.structure_id             --構成ID
    FROM
        ms_structure AS st
    WHERE
        st.structure_id IN @StructureIdList
),
rec_up(structure_layer_no, structure_id, parent_structure_id, org_structure_id) AS(
    SELECT
        st.structure_layer_no,
        st.structure_id,
        st.parent_structure_id,
        st.structure_id
    FROM
        st_com AS st
    UNION ALL
    SELECT
        b.structure_layer_no,
        b.structure_id,
        b.parent_structure_id,
        a.org_structure_id
    FROM
        rec_up a
        INNER JOIN
            ms_structure b
        ON  b.structure_id = a.parent_structure_id
            
),
exdata AS ( 
    --拡張データを取得
    SELECT
        structure_id
        , ie.extension_data 
    FROM
        v_structure_item_all si 
        INNER JOIN ms_item_extension ie 
            ON si.structure_item_id = ie.item_id 
            AND si.structure_group_id = 1000
            AND si.structure_layer_no = 1
            AND ie.sequence_no = 1
            AND si.language_id = @LanguageId
)
SELECT DISTINCT
    vs.structure_id,
    vs.translation_text,
    vs.structure_group_id,
    ex.extension_data
FROM
    rec_up AS up
    LEFT OUTER JOIN
        v_structure_item_all AS vs
    ON up.structure_id = vs.structure_id
    LEFT JOIN
        exdata AS ex
    ON up.structure_id = ex.structure_id

WHERE
    vs.language_id = @LanguageId
AND
    up.structure_layer_no = 1--工場
AND 
    ex.extension_data <> ''
ORDER BY
    vs.structure_id
