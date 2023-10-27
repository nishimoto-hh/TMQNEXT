DROP TABLE IF EXISTS #temp_structure; 
-- 対象構成ID登録用一時テーブルを作成
CREATE TABLE #temp_structure(structure_id int); 

-- 対象の構成IDを一時テーブルへ保存
INSERT 
INTO #temp_structure 
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
                #temp_structure temp 
            WHERE
                st.structure_id = temp.structure_id
        ) 
    AND st.delete_flg = 0
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
        AND b.delete_flg = 0
),
/*@All
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
        AND b.delete_flg = 0   
),
@All*/
rec(structure_layer_no, structure_id, parent_structure_id, org_structure_id) AS(
/*@All
    SELECT
        up.structure_layer_no,
        up.structure_id,
        up.parent_structure_id,
        up.org_structure_id
    FROM
        rec_up AS up
    UNION
@All*/
    SELECT
        down.structure_layer_no,
        down.structure_id,
        down.parent_structure_id,
        down.org_structure_id
    FROM
        rec_down AS down
)

-- 指定構成IDの上下階層の全構成マスタデータを一時テーブルへ保存
INSERT
/*@All
INTO #temp_structure_all
@All*/
/*@Selected
INTO #temp_structure_selected
@Selected*/
SELECT DISTINCT
     vs.structure_id
    ,vs.factory_id
    ,vs.structure_group_id
    ,vs.parent_structure_id
    ,vs.structure_layer_no
    ,vs.translation_text
    ,coalesce(order_common.display_order, order_factory.display_order, vs.structure_id) AS display_order
FROM
    rec AS com
    LEFT OUTER JOIN
        v_structure_item AS vs
    ON  (
            com.structure_id = vs.structure_id
        )
    LEFT OUTER JOIN
        ms_structure_order AS order_factory
    ON  (
            com.structure_id = order_factory.structure_id
        AND order_factory.factory_id = vs.factory_id
        )
    LEFT OUTER JOIN
        ms_structure_order AS order_common
    ON  (
            com.structure_id = order_common.structure_id
        AND order_common.factory_id = 0
        )
WHERE
    vs.language_id = @LanguageId
ORDER BY
    vs.structure_group_id, vs.structure_layer_no
