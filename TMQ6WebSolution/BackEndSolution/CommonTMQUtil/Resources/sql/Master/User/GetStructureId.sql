/*
 選択された構成IDに紐づく工場を取得する
*/
WITH st_com(structure_layer_no, structure_id, parent_structure_id, org_structure_id) AS(
    SELECT
        st.structure_layer_no,  --構成階層番号
        st.structure_id,        --構成ID
        st.parent_structure_id, --親構成ID
        st.structure_id         --構成ID
    FROM 
        ms_structure AS st
    WHERE
        st.structure_id IN @StructureIdList
        /*フロント側で取得した構造IDを列挙*/
),
rec_down(structure_layer_no, structure_id, parent_structure_id, org_structure_id) AS(
    SELECT
        com.structure_layer_no,
        com.structure_id,
        com.parent_structure_id,
        com.structure_id
    FROM
        st_com AS com
    UNION ALL
    SELECT
        b.structure_layer_no,
        b.structure_id,
        b.parent_structure_id,
        a.org_structure_id
    FROM
        rec_down AS a
        INNER JOIN
            ms_structure AS b
        ON  (
                a.structure_id = b.parent_structure_id
            )
),
rec_up(structure_layer_no, structure_id, parent_structure_id, org_structure_id) AS(
    SELECT
        com.structure_layer_no,
        com.structure_id,
        com.parent_structure_id,
        com.structure_id
    FROM
        st_com AS com
    UNION ALL
    SELECT
        b.structure_layer_no,
        b.structure_id,
        b.parent_structure_id,
        a.org_structure_id
    FROM
        rec_up AS a
        INNER JOIN
            ms_structure AS b
        ON  (
                b.structure_id = a.parent_structure_id
            )
),
rec(structure_layer_no, structure_id, parent_structure_id,org_structure_id) AS(
    SELECT
        up.structure_layer_no,
        up.structure_id,
        up.parent_structure_id,
        up.org_structure_id
    FROM
        rec_up AS up
    UNION
    SELECT
        down.structure_layer_no,
        down.structure_id,
        down.parent_structure_id,
        down.org_structure_id
    FROM
        rec_down AS down
)
SELECT
    com.org_structure_id,
    vs.structure_id,
    vs.parent_structure_id,
    vs.structure_layer_no,
    vs.translation_text

FROM
    rec AS com
    LEFT OUTER JOIN
        v_structure_item AS vs
    ON  (
            com.structure_id = vs.structure_id
        )
WHERE
    vs.language_id = @LanguageId
    AND vs.structure_layer_no = 1 --工場
ORDER BY
com.org_structure_id,vs.parent_structure_id,
    vs.structure_layer_no
