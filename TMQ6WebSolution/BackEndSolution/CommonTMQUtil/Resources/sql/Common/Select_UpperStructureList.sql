WITH st_com(structure_layer_no, structure_id, parent_structure_id, org_structure_id) AS(
    SELECT
        st.structure_layer_no,
        st.structure_id,
        st.parent_structure_id,
        st.structure_id
    FROM
        ms_structure AS st
    WHERE
        @StructureIdList
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
        ON  (
                b.structure_id = a.parent_structure_id
            )
)
SELECT
    vs.structure_id,
    vs.translation_text,
    vs.structure_group_id,
    vs.structure_layer_no,
    up.org_structure_id
FROM
    rec_up AS up
    LEFT OUTER JOIN
        v_structure_item_all AS vs
    ON  (
            up.structure_id = vs.structure_id
        )
WHERE
    vs.language_id = @LanguageId
ORDER BY
    up.org_structure_id,
    vs.structure_layer_no
