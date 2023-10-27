/*
 選択された構成IDより上位の工場を取得する(保全実績一覧)
*/

DROP TABLE IF EXISTS #temp_structure; 
-- 一時テーブルを作成
CREATE TABLE #temp_structure(structure_id int); 
-- 構成IDを一時テーブルへ保存
INSERT 
INTO #temp_structure 
SELECT
    * 
FROM
    STRING_SPLIT(@StructureIdList, ','); 

WITH st_com(structure_layer_no, structure_id, parent_structure_id, org_structure_id) AS(
    SELECT
        st.structure_layer_no,      --構成階層番号
        st.structure_id,            --構成ID
        st.parent_structure_id,     --親構成ID
        st.structure_id             --構成ID
    FROM
        ms_structure AS st
    WHERE
        EXISTS ( 
            SELECT
                * 
            FROM
                #temp_structure temp 
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
        ON  b.structure_id = a.parent_structure_id
            
),
exdata AS ( 
    --拡張データを取得
    SELECT
        structure_id
        , ie.extension_data 
    FROM
        ms_structure st 
        INNER JOIN ms_item_extension ie 
            ON st.structure_item_id = ie.item_id 
            AND st.structure_group_id = 1000
            AND st.structure_layer_no = 1
            AND ie.sequence_no = 1
)
SELECT DISTINCT
    st.structure_id,
    st.structure_group_id,
    ex.extension_data
FROM
    rec_up AS up
    LEFT OUTER JOIN
        ms_structure AS st
    ON up.structure_id = st.structure_id
    LEFT JOIN
        exdata AS ex
    ON up.structure_id = ex.structure_id

WHERE
    up.structure_layer_no = 1--工場
ORDER BY
    st.structure_id
