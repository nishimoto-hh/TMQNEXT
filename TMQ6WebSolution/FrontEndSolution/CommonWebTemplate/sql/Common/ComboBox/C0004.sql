/*
 * 文書種類 コンボ用データリスト用SQL.(絞り込み条件付き）
 */
-- C0004
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
    item.factory_id as factoryId,
    item.location_structure_id as translationFactoryId,
    item.structure_id AS 'values',
    item.translation_text + '（' + conduct_name.translation_text + '＿' + action_name.translation_text + '）' as labels,
    item.delete_flg AS deleteFlg
    /*IF factoryIdList != null && factoryIdList.Count > 0*/
    -- JavaScript側で画面の工場IDにより絞り込みを行うので、絞込用の工場IDと工場ID毎に表示順を取得する
    -- 表示順用工場ID
    ,coalesce(ft.factory_id, 0) AS orderFactoryId
   /*END*/
FROM
    v_structure_item_all item
    LEFT JOIN
        ms_item_extension conduct
    ON  item.structure_item_id = conduct.item_id
    AND conduct.sequence_no = 2
    LEFT JOIN
        ms_translation conduct_name
    ON  CAST(conduct.extension_data AS bigint) = conduct_name.translation_id
    AND conduct_name.language_id = /*languageId*/'ja'
    LEFT JOIN
        ms_item_extension action
    ON  item.structure_item_id = action.item_id
    AND action.sequence_no = 1
    LEFT JOIN
        ms_translation action_name
    ON  CAST(action.extension_data AS bigint) = action_name.translation_id
    AND action_name.language_id = /*languageId*/'ja'
    /*IF factoryIdList != null && factoryIdList.Count > 0*/
    -- 工場ごとに工場別表示順を取得する
    CROSS JOIN factory AS ft
    LEFT OUTER JOIN ms_structure_order AS order_factory
    ON  item.structure_id = order_factory.structure_id
    AND order_factory.factory_id = ft.factory_id
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
ORDER BY
    item.structure_group_id,
    item.structure_layer_no,
    /*IF factoryIdList != null && factoryIdList.Count > 0*/
    -- 工場ID毎の表示順
    row_number() over(partition BY coalesce(ft.factory_id, 0) ORDER BY coalesce(coalesce(order_factory.display_order, order_common.display_order), 32768), item.structure_id)
    /*END*/
    /*IF factoryIdList == null || factoryIdList.Count == 0*/
    -- 工場IDの指定が無い場合、表示順は全工場共通(0)より取得
    row_number() over(ORDER BY coalesce(order_common.display_order,32768), item.structure_id)
    /*END*/
