/*
* 予備品の検索用SQL
* 画面左のツリーで選択された場所階層より、上位の地区・工場を取得し、
* それらの工場の配下の棚の一覧を取得する
* 共通工場についても、権限があれば結果に含む
*/
-- ↓ここからはSelect_StructureList.sqlと同じ
WITH st_com(structure_layer_no, structure_id, parent_structure_id, org_structure_id) AS(
     SELECT
         st.structure_layer_no
        ,st.structure_id
        ,st.parent_structure_id
        ,st.structure_id
    FROM
        ms_structure AS st
    WHERE
        st.structure_id IN /*StructureIdList*/(0)
    AND st.delete_flg = 0
)
,rec_down(structure_layer_no, structure_id, parent_structure_id, org_structure_id) AS(
     SELECT
         com.structure_layer_no
        ,com.structure_id
        ,com.parent_structure_id
        ,com.structure_id
    FROM
         st_com AS com
    UNION ALL
    SELECT
         b.structure_layer_no
        ,b.structure_id
        ,b.parent_structure_id
        ,a.org_structure_id
    FROM
         rec_down AS a
        INNER JOIN
            ms_structure AS b
        ON  (
                a.structure_id = b.parent_structure_id
            )
        AND b.delete_flg = 0
)
,rec_up(structure_layer_no, structure_id, parent_structure_id, org_structure_id) AS(
     SELECT
         com.structure_layer_no
        ,com.structure_id
        ,com.parent_structure_id
        ,com.structure_id
    FROM
         st_com AS com
    UNION ALL
    SELECT
         b.structure_layer_no
        ,b.structure_id
        ,b.parent_structure_id
        ,a.org_structure_id
    FROM
         rec_up AS a
        INNER JOIN
            ms_structure AS b
        ON  (
                b.structure_id = a.parent_structure_id
            )
        AND b.delete_flg = 0
)
,rec(structure_layer_no, structure_id, parent_structure_id, org_structure_id) AS(
     SELECT
         up.structure_layer_no
        ,up.structure_id
        ,up.parent_structure_id
        ,up.org_structure_id
    FROM
         rec_up AS up
    UNION
    SELECT
         down.structure_layer_no
        ,down.structure_id
        ,down.parent_structure_id
        ,down.org_structure_id
    FROM
         rec_down AS down
)
-- ↑ここまではSelect_StructureList.sqlと同じ
-- 予備品の場所階層と紐づけるため、地区と工場のみを指定
,spare_location AS(
     SELECT
         st.structure_id
        ,st.structure_layer_no
    FROM
        rec AS com
        LEFT OUTER JOIN
            ms_structure AS st
        ON  (
                com.structure_id = st.structure_id
            )
    WHERE
        st.structure_layer_no IN(0, 1)
    AND st.delete_flg = 0
)
-- 共通工場 場所階層の連番3の値が1
,com_factory AS(
    SELECT
         vs.structure_id
        ,vs.parent_structure_id
    FROM
        v_structure AS vs
        INNER JOIN
            ms_item_extension AS ex
        ON  (
                vs.structure_item_id = ex.item_id
            )
        INNER JOIN
            ms_user_belong AS ub
        ON  (
                dbo.get_target_layer_id(ub.location_structure_id, 1) = vs.structure_id
            )
    WHERE
        vs.structure_group_id = 1000
    AND ex.sequence_no = 3
    AND ex.extension_data = '1'
    AND ub.user_id = /*UserId*/0
    AND ub.delete_flg = 0
)
-- 予備品場所階層の工場と共通工場をマージ
,st_id AS(
    SELECT
        loc.structure_id
    FROM
        spare_location AS loc
    WHERE
        loc.structure_layer_no = 1
    UNION
    SELECT
        fac.structure_id
    FROM
         com_factory AS fac
        INNER JOIN
            spare_location AS loc
        ON  (
                fac.parent_structure_id = loc.structure_id
            AND loc.structure_layer_no = 0
            )
)
SELECT
     st.structure_id
FROM
     st_id AS st