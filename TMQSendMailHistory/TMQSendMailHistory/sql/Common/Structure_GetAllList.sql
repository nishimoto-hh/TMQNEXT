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
SELECT
     vs.structure_id AS structureId
    ,vs.factory_id AS factoryId
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
    -- 構成アイテムの表示順
    LEFT OUTER JOIN
        ms_structure_order AS order_layer
    ON  (
            vs.structure_id = order_layer.structure_id
            -- 場所階層の地区、工場のみ0、他は自身の工場
        AND order_layer.factory_id = IIF( vs.structure_layer_no < 2 AND vs.structure_group_id = 1000 ,0 ,vs.factory_id)
        )
/*IF ExceptCommonFactory*/
-- 予備品の共通倉庫用の共通工場を除くアイテムマスタ拡張の連番=3の拡張データが'1'
-- 予備品のツリーを取得するときは、共通工場も表示
    LEFT JOIN
        ms_item_extension ie
    ON vs.structure_item_id = ie.item_id
    AND ie.sequence_no = 3
WHERE ie.extension_data IS NULL OR ie.extension_data != '1'
/*END*/
--ルート要素の名称を構成グループマスタから取得
UNION ALL
SELECT
     -1 AS structureId
    ,0 AS factoryId
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
        
ORDER BY
    vs.structure_group_id, factory_order, vs.structure_layer_no, displayOrder, structureId
