/*
 * 名称マスタ コンボ用データリスト用SQL.(絞り込み条件付き）
 */
-- C0036
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
select st.factory_id as factoryId,
       st.location_structure_id as translationFactoryId,
       st.structure_id as 'values',
       st.translation_text as labels,
       st.delete_flg AS deleteFlg
  from v_structure_item_all AS st
       /*IF factoryIdList != null && factoryIdList.Count > 0*/
       -- 工場ごとに工場別表示順を取得する
       CROSS JOIN factory AS ft
       LEFT OUTER JOIN ms_structure_order AS order_factory
       ON  st.structure_id = order_factory.structure_id
       AND order_factory.factory_id = ft.factory_id
       /*END*/
       -- 全工場共通の表示順
       LEFT OUTER JOIN ms_structure_order AS order_common
       ON  st.structure_id = order_common.structure_id
       AND order_common.factory_id = 0
 where st.structure_group_id = /*param1*/0
   and st.language_id = /*languageId*/'ja'
   
/*IF factoryIdList != null && factoryIdList.Count > 0*/
   and st.factory_id in /*factoryIdList*/(0)
   -- 共通工場のレコードまたは絞込用工場IDと表示順用工場IDが一致するもののみ抽出
   and (st.factory_id = 0 and st.location_structure_id in (coalesce(ft.factory_id, 0), 0) OR coalesce(ft.factory_id, 0) in (st.factory_id, 0))
/*END*/

/*IF param2 != null*/
   and st.structure_layer_no = /*param2*/0
/*END*/

/*IF param3 != null && param3 != ''*/
   and st.parent_structure_id = /*param3*/0
/*END*/


order by st.structure_group_id,st.structure_layer_no,
/*IF factoryIdList != null && factoryIdList.Count > 0*/
-- 工場ID毎の表示順
row_number() over(partition BY coalesce(ft.factory_id, 0) ORDER BY coalesce(coalesce(order_factory.display_order, order_common.display_order), 32768), st.structure_id)
/*END*/
/*IF factoryIdList == null || factoryIdList.Count == 0*/
-- 工場IDの指定が無い場合、表示順は全工場共通(0)より取得
row_number() over(ORDER BY coalesce(order_common.display_order,32768), st.structure_id)
/*END*/
