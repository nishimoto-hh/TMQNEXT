/*
 選択された構成IDから上位下位全ての場所階層情報を取得する(保全実績一覧)
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
        st.structure_layer_no,  --構成階層番号
        st.structure_id,        --構成ID
        st.parent_structure_id, --親構成ID
        st.structure_id         --構成ID
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
    vs.translation_text,
    vs.factory_id,
    vs.structure_group_id,
    vs.structure_item_id,
    vs.item_translation_id,
    coalesce(coalesce(order_factory.display_order, order_common.display_order), 32768) AS display_order,
    vs.location_structure_id,
    vs.language_id,
    vs.translation_item_description
FROM
    rec AS com
    LEFT OUTER JOIN
        v_structure_item_all AS vs
    ON  (
            com.structure_id = vs.structure_id
        )
            LEFT OUTER JOIN
        ms_structure_order AS order_factory
    ON  (
            com.structure_id = order_factory.structure_id
        AND order_factory.factory_id = @FactoryId
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
com.org_structure_id,vs.parent_structure_id,
    vs.structure_layer_no
