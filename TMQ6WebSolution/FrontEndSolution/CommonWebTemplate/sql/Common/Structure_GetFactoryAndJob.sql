WITH factory AS(
     SELECT
         factory.*
         -- 工場の表示順
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
    --AND factory.factory_id IN /*FactoryIdList*/(0, 5)
    /*END*/
)
,job AS(
    SELECT
        *
    FROM
        ms_structure AS job
    WHERE
        job.structure_group_id = 1010
    AND job.structure_layer_no = 0
    AND delete_flg != 1
)
SELECT
     vs.structure_id AS structureId
    ,vs.factory_id AS factoryId
    ,1005 AS structureGroupId
    ,0 AS structureLayerNo
    ,0 AS parentStructureId
    ,vs.structure_item_id AS structureItemId
    ,fc_order.factory_order AS displayOrder
    ,vs.translation_text AS translationText
    ,coalesce(fc_order.factory_order, 32768) AS factory_order
FROM
    factory AS fc
    INNER JOIN
        v_structure_item AS vs
    ON  (
            fc.structure_id = vs.structure_id
        AND vs.language_id = /*LanguageId*/'ja'
        )
    LEFT OUTER JOIN
        factory AS fc_order
    ON  (
            fc.structure_id = fc_order.structure_id
        )
    UNION ALL
    SELECT
         vs.structure_id AS structureId
        ,vs.factory_id AS factoryId
        ,1005 AS structureGroupId
        ,1 AS structureLayerNo
        ,vs.factory_id AS parentStructureId
        ,vs.structure_item_id AS structureItemId
        ,coalesce(order_common.display_order, 32768) AS displayOrder
        ,vs.translation_text AS translationText
        ,fc_order.factory_order
    FROM
        job
    INNER JOIN
        v_structure_item AS vs
    ON  (
            job.structure_id = vs.structure_id
        AND vs.language_id = /*LanguageId*/'ja'
        )
        -- 表示順(工場共通)
    LEFT OUTER JOIN
        ms_structure_order AS order_common
    ON  (
            vs.structure_id = order_common.structure_id
        AND order_common.factory_id = 0
        )
        -- 職種の工場の表示順を取得
    LEFT OUTER JOIN
        factory AS fc_order
    ON  (
            job.factory_id = fc_order.structure_id
        )
    WHERE
        EXISTS(
            SELECT
                *
            FROM
                factory
            WHERE
                job.factory_id = factory.factory_id
        )
    --ルート要素の名称を構成グループマスタから取得
UNION ALL
SELECT
     - 1 AS structureId
    ,st.factory_id AS factoryId
    ,st.structure_group_id AS structureGroupId
    ,- 1 AS structureLayerNo
    ,- 1 AS parentStructureId
    ,- 1 AS structureItemId
    ,1 AS displayOrder
    ,st.translation_text AS translationText
    ,- 1 AS factory_order
FROM
    v_structure_item st
WHERE
    st.structure_group_id = 1005
AND st.language_id = /*LanguageId*/'ja'
ORDER BY
     structureGroupId
    ,factory_order
    ,structureLayerNo
    ,displayOrder
    ,structureId