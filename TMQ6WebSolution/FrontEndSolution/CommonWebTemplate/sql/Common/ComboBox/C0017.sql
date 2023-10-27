/*
 * 原因性格マスタメンテ コンボ用データリスト用SQL.(絞り込み条件付き）
 */
-- C0017
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
select item.factory_id as factoryId,
       item.location_structure_id as translationFactoryId,
       item.structure_id as 'values',
--       translation_text as labels
     dbo.get_translation_text_all( 
        item.structure_id
        , /*param3*/0
        , /*param1*/0
        , /*languageId*/'ja'
    ) AS labels,                      -- アイテム翻訳名称
       item.delete_flg AS deleteFlg
    /*IF factoryIdList != null && factoryIdList.Count > 0*/
    -- JavaScript側で画面の工場IDにより絞り込みを行うので、絞込用の工場IDと工場ID毎に表示順を取得する
    -- 表示順用工場ID
    ,coalesce(ft.factory_id, 0) AS orderFactoryId
    /*END*/
  from v_structure_item_all AS item
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
 where item.structure_group_id = /*param1*/0
   and item.language_id = /*languageId*/'ja'
--/*IF factoryIdList != null && factoryIdList.Count > 0*/
--   and factory_id in /*factoryIdList*/(0)
--/*END*/
/*IF param2 != null*/
   and item.structure_layer_no = /*param2*/0
/*END*/
  and (
/*IF param3 != null && param3 >= 1 */
     (item.factory_id = /*param3*/0
      and item.location_structure_id in (0, /*param3*/0)
     )
     or
/*END*/
     (item.factory_id = 0
      and item.location_structure_id = 0)
     )
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
order by item.structure_group_id,item.structure_layer_no,
/*IF factoryIdList != null && factoryIdList.Count > 0*/
-- 工場ID毎の表示順
row_number() over(partition BY coalesce(ft.factory_id, 0) ORDER BY coalesce(coalesce(order_factory.display_order, order_common.display_order), 32768), item.structure_id)
/*END*/
/*IF factoryIdList == null || factoryIdList.Count == 0*/
-- 工場IDの指定が無い場合、表示順は全工場共通(0)より取得
row_number() over(ORDER BY coalesce(order_common.display_order,32768), item.structure_id)
/*END*/

