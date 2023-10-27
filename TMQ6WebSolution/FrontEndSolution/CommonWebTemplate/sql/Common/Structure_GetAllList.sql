WITH st_com(structure_layer_no, structure_id, parent_structure_id) AS (
    SELECT
        structure_layer_no,
        structure_id,
        parent_structure_id
    FROM
        ms_structure
    WHERE
        delete_flg != 1
    AND structure_group_id IN /*StructureGroupIdList*/(1000, 1010)
    /*IF FactoryIdList.Count!=0 */
        AND factory_id IN /*FactoryIdList*/(0, 5)
    /*END*/
    /*IF StructureIdList.Count!=0 */
        AND structure_id IN /*StructureIdList*/(11, 102)
    /*END*/
),
rec_down(structure_layer_no, structure_id, parent_structure_id) AS (
    SELECT
        *
    FROM
        st_com
    UNION ALL
    SELECT
        b.structure_layer_no,
        b.structure_id,
        b.parent_structure_id
    FROM
        rec_down a
        INNER JOIN
            ms_structure b
        ON
            a.structure_id = b.parent_structure_id
        AND
            b.delete_flg != 1
),
rec_up(structure_layer_no, structure_id, parent_structure_id) AS (
    SELECT
        *
    FROM
        st_com
    UNION ALL
    SELECT
        b.structure_layer_no,
        b.structure_id,
        b.parent_structure_id
    FROM
        rec_up a
        INNER JOIN
            ms_structure b
        ON
            b.structure_id = a.parent_structure_id
        AND
            b.delete_flg != 1
),
rec(structure_layer_no, structure_id, parent_structure_id) AS (
    SELECT
        *
    FROM
        rec_up
    UNION
    SELECT
        *
    FROM
        rec_down
),
-- 工場の表示順
factory_order AS(
    SELECT
        factory.factory_id
       ,coalesce(order_common.display_order, 32768) AS factory_order
    FROM
        ms_structure AS factory
        -- 表示順(工場共通)
        LEFT OUTER JOIN
            ms_structure_order AS order_common
        ON  (
                factory.structure_id = order_common.structure_id
            AND order_common.factory_id = 0
            )
    WHERE
        factory.structure_group_id = 1000
    AND factory.structure_layer_no = 1
    AND factory.delete_flg != 1
    /*IF FactoryIdList.Count!=0 */
    AND factory.factory_id IN /*FactoryIdList*/(0, 5)
    /*END*/
)
/*IF IsTransFactoryOrderTree*/
-- 工場個別の翻訳や並び替えが必要な場合の処理
, factory AS ( 
    -- 全工場のリストを作成
    SELECT
        structure_id AS factory_id 
    FROM
        ms_structure 
    /*IF FactoryIdList.Count!=0 */
    WHERE
        structure_id IN /*FactoryIdList*/(0, 5)
    /*END*/
    UNION SELECT 0 AS factory_id
) 
,structure_order AS(
     -- 全工場のそれぞれの表示順を作成
     SELECT
         st.structure_id
        ,coalesce(ft.factory_id, 0) AS orderFactoryId
        -- 工場IDごとに並び順を作成、個別工場(または共通工場)の表示順、構成IDの順
        ,ROW_NUMBER() OVER(PARTITION BY coalesce(ft.factory_id, 0) 
        ORDER BY iif(coalesce(ft.factory_id, 0) = 0, coalesce(order_common.display_order, 32768), coalesce(order_factory.display_order, 32768)), st.structure_id) AS display_order
    FROM
        ms_structure AS st
        -- 工場ごとに工場別表示順を取得する
        CROSS JOIN
            factory AS ft
        LEFT OUTER JOIN
            ms_structure_order AS order_factory
        ON  st.structure_id = order_factory.structure_id
        AND order_factory.factory_id = ft.factory_id
            -- 全工場共通の表示順
        LEFT OUTER JOIN
            ms_structure_order AS order_common
        ON  st.structure_id = order_common.structure_id
        AND order_common.factory_id = 0
    WHERE
        st.structure_group_id IN /*StructureGroupIdList*/(1000, 1010)
    AND st.delete_flg != 1
)
/*END*/
/*IF NarrowHistoryFactory>0 */
-- 変更履歴管理の工場を取得(対象の工場を取得する場合、対象外の工場を取得する場合がある)
,history_factory AS(
    SELECT
        vs.structure_id
    FROM
        v_structure AS vs
    WHERE
/*IF NarrowHistoryFactory==1001 */
-- 対象外の工場を取得する場合はNOT EXISTS
        NOT
/*END*/
        EXISTS(
            SELECT
                *
            FROM
                ms_item_extension AS ex
            WHERE
                ex.item_id = vs.structure_item_id
            -- 拡張項目4の値がNullでなければ承認者が設定されていて、変更履歴管理を行う工場となる。
            AND ex.sequence_no = 4
            AND coalesce(ex.extension_data, '') != ''
        )
    AND vs.structure_group_id = 1000
    AND vs.structure_layer_no = 1
-- 地区の取得用に共通工場が必要
    UNION
    SELECT
        0
)
, structure_list AS ( 
/*END*/
SELECT
     vs.structure_id AS structureId
     ,
/*IF !IsTransFactoryOrderTree*/
      vs.factory_id
/*END*/
/*IF IsTransFactoryOrderTree*/
      ft.factory_id
/*END*/
                    AS factoryId
    ,vs.location_structure_id AS locationStructureId
    ,vs.structure_group_id AS structureGroupId
    ,vs.structure_layer_no AS structureLayerNo
    ,vs.parent_structure_id AS parentStructureId
    ,vs.structure_item_id AS structureItemId
    ,coalesce(order_layer.display_order, 32768) AS displayOrder
    ,vs.translation_text AS translationText
    ,coalesce(fc_order.factory_order, 32768) AS factory_order
FROM
    rec
    INNER JOIN
        v_structure_item AS vs
    ON  
        rec.structure_id = vs.structure_id
    AND vs.language_id = /*LanguageId*/'ja'
    -- 工場の表示順を取得
    LEFT OUTER JOIN
        factory_order AS fc_order
    ON  (
            vs.factory_id = fc_order.factory_id
        )
/*IF !IsTransFactoryOrderTree*/
    -- 翻訳を行わない場合
    -- 構成アイテムの表示順
    LEFT OUTER JOIN
        ms_structure_order AS order_layer
    ON  (
            vs.structure_id = order_layer.structure_id
            -- 場所階層の地区、工場のみ0、他は自身の工場
        AND order_layer.factory_id = IIF( vs.structure_layer_no < 2 AND vs.structure_group_id = 1000 ,0 ,vs.factory_id)
        )
/*END*/
/*IF IsTransFactoryOrderTree*/
    -- 翻訳を行う場合、全工場のデータを作成する
    CROSS JOIN
        factory AS ft
        -- 構成アイテムの表示順
    LEFT OUTER JOIN
        structure_order AS order_layer
    ON  (
            vs.structure_id = order_layer.structure_id
        AND ft.factory_id = order_layer.orderFactoryId
        )
/*END*/
/*IF ExceptCommonFactory*/
-- 予備品の共通倉庫用の共通工場を除くアイテムマスタ拡張の連番=3の拡張データが'1'
-- 予備品のツリーを取得するときは、共通工場も表示
    LEFT JOIN
        ms_item_extension ie
    ON vs.structure_item_id = ie.item_id
    AND ie.sequence_no = 3
/*END*/

WHERE 1 = 1
/*IF ExceptCommonFactory*/
AND (ie.extension_data IS NULL OR ie.extension_data != '1')
/*END*/
/*IF NarrowHistoryFactory>0 */
AND EXISTS(
        SELECT
            *
        FROM
            history_factory AS h_factory
        WHERE
            vs.factory_id = h_factory.structure_id
    )
/*END*/
/*IF IsTransFactoryOrderTree*/
   -- 翻訳を行う場合は未使用アイテムも考慮する
   AND
       NOT EXISTS(
            SELECT
               *
           FROM
               ms_structure_unused AS unused
           WHERE
               unused.factory_id = ft.factory_id
           AND unused.structure_id = vs.structure_id
       )
/*END*/
--ルート要素の名称を構成グループマスタから取得
UNION ALL
SELECT
     -1 AS structureId
    ,0 AS factoryId
    ,0 AS locationStructureId
    ,sg.structure_group_id AS structureGroupId
    ,-1 AS structureLayerNo
    ,-1 AS parentStructureId
    ,-1 AS structureItemId
    ,1 AS displayOrder
    ,mt.translation_text AS translationText
    ,- 1 AS factory_order
FROM
    ms_structure_group sg
LEFT JOIN   ms_translation mt
ON sg.structure_group_translation_id = mt.translation_id

WHERE
    sg.delete_flg != 1
AND 
    mt.delete_flg = 0
AND
    sg.structure_group_id IN /*StructureGroupIdList*/(1000, 1010)
AND
    mt.language_id = /*LanguageId*/'ja'
        
/*IF NarrowHistoryFactory==0 */
ORDER BY
/*IF !IsTransFactoryOrderTree*/
    vs.structure_group_id, factory_order, vs.structure_layer_no, displayOrder, structureId
/*END*/
/*IF IsTransFactoryOrderTree*/
    -- 翻訳を行う場合は工場の表示順は使えないので、工場IDごとに並べる
    vs.structure_group_id, factoryId, vs.structure_layer_no, displayOrder, structureId
/*END*/
/*END*/
/*IF NarrowHistoryFactory>0 */
--変更管理の場合、表示工場の絞り込みにより地区配下に表示する工場が無い場合はその地区を除外する
) 
, district_list AS ( 
    --地区のみ取得
    SELECT
        structureId 
    FROM
        structure_list 
    WHERE
        structureGroupId = 1000 
        AND structureLayerNo = 0
) 
, factory_list AS ( 
    --工場のみ取得
    SELECT
        parentStructureId 
    FROM
        structure_list 
    WHERE
        structureGroupId = 1000 
        AND structureLayerNo = 1
) 
SELECT
    * 
FROM
    structure_list sl 
WHERE
    NOT EXISTS ( 
        --配下にデータがない地区は除外する
        SELECT
            * 
        FROM
            ( 
                SELECT
                    dl.structureId
                    , COUNT(fl.parentStructureId) count 
                FROM
                    district_list dl 
                    LEFT JOIN factory_list fl 
                        ON dl.structureId = fl.parentStructureId 
                GROUP BY
                    dl.structureId 
                HAVING
                    COUNT(fl.parentStructureId) = 0
            ) district 
        WHERE
            sl.structureId = district.structureId
    ) 
ORDER BY
    structureGroupId
    , factory_order
    , structureLayerNo
    , displayOrder
    , structureId
/*END*/