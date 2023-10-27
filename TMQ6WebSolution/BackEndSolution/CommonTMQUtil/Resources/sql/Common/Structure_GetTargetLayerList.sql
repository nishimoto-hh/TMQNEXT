/*
* 階層の検索用SQL
* 指定された構成IDのリストより、指定された上位階層のリストを取得する
*/
DROP TABLE IF EXISTS #temp_target_layer; 
-- 一時テーブルを作成
CREATE TABLE #temp_target_layer(structure_id int); 
-- 構成IDを一時テーブルへ保存
INSERT 
INTO #temp_target_layer 
SELECT
    * 
FROM
    STRING_SPLIT(@StructureIdList, ','); 

WITH st_com(structure_layer_no, structure_id, parent_structure_id, org_structure_id) AS(
    SELECT
        st.structure_layer_no,
        st.structure_id,
        st.parent_structure_id,
        st.structure_id
    FROM
        ms_structure AS st
    WHERE
        EXISTS ( 
            SELECT
                * 
            FROM
                #temp_target_layer temp 
            WHERE
                st.structure_id = temp.structure_id
        ) 
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
),
target AS (
SELECT
    vs.structure_id,
    vs.structure_group_id,
    vs.structure_layer_no,
    up.org_structure_id
FROM
    rec_up AS up
    LEFT OUTER JOIN
        v_structure_all AS vs
    ON  (
            up.structure_id = vs.structure_id
        )
)
SELECT DISTINCT
    structure_id
FROM
    target
WHERE
    structure_layer_no = @StructureLayerNo