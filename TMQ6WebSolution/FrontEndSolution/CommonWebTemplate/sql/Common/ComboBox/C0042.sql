/*
 * 文書種類 コンボ用データリスト用SQL.(絞り込み条件付き）
 */
-- C0042
/*IF factoryIdList != null && factoryIdList.Count > 0*/
-- 工場IDで絞込を行う場合、指定された工場IDの表示順を取得する
-- 工場ID一覧を生成するための構成マスタより取得
WITH factory AS(
     SELECT
        st.structure_id AS factory_id
    FROM
        ms_structure AS st
    WHERE
        st.structure_id IN /*factoryIdList*/(0)
    UNION
    -- 共通工場は構成マスタに無いので追加
    SELECT
         0
)
/*END*/
SELECT
    factoryId
    ,translationFactoryId
    ,[values]
    ,labels
    ,deleteFlg
    /*IF factoryIdList != null && factoryIdList.Count > 0*/
    ,orderFactoryId
    /*END*/
FROM (
    SELECT
        item.factory_id as factoryId,
        item.location_structure_id as translationFactoryId,
        STRING_AGG(item.structure_id, ',') AS [values],
        item.translation_text as labels,
        0 AS deleteFlg
        /*IF factoryIdList != null && factoryIdList.Count > 0*/
        -- JavaScript側で画面の工場IDにより絞り込みを行うので、絞込用の工場IDと工場ID毎に表示順を取得する
        -- 表示順用工場ID
        ,coalesce(ft.factory_id, 0) AS orderFactoryId
       /*END*/
       ,MIN(item.structure_group_id) AS structure_group_id
       ,MIN(item.structure_id) AS structure_id
       ,coalesce(order_common.display_order,32768) AS display_order
    FROM
        v_structure_item_all item
        /*IF factoryIdList != null && factoryIdList.Count > 0*/
        -- 工場ごとに工場別表示順を取得する
        INNER JOIN factory AS ft
        ON item.factory_id = ft.factory_id
        /*END*/
        -- 全工場共通の表示順
        LEFT OUTER JOIN ms_structure_order AS order_common
        ON  item.structure_id = order_common.structure_id
        AND order_common.factory_id = 0
    WHERE
        item.structure_group_id IN(1600, 1610, 1620, 1630, 1640, 1650, 1660, 1670, 1680, 1690, 1700, 1750, 1780)
    AND item.language_id = /*languageId*/'ja'
    /*IF factoryIdList != null && factoryIdList.Count > 0*/
       -- 工場別未使用標準アイテムに工場が含まれていないものを表示
    AND
       NOT EXISTS(
            SELECT
               *
           FROM
               ms_structure_unused AS unused
           WHERE
               unused.factory_id = ft.factory_id
           AND unused.structure_id = item.structure_id
       )
    /*END*/
    GROUP BY 
        item.factory_id
        , item.location_structure_id
        , item.translation_text
        , order_common.display_order
        /*IF factoryIdList != null && factoryIdList.Count > 0*/
        , coalesce(ft.factory_id, 0)
        /*END*/
) tbl
ORDER BY
     tbl.structure_group_id
    ,tbl.display_order
    ,tbl.structure_id
