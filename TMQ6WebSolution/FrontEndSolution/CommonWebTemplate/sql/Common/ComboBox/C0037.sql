/*
 * 名称マスタ コンボ用データリスト用SQL.(絞り込み条件付き）
 */
-- C0037(factoryIdList未使用)
-- 工場IDで絞込を行う場合、指定された工場IDの表示順を取得する
-- 工場ID一覧を生成するための構成マスタより取得
select '' as factoryId,
       '' as translationFactoryId,
       st.structure_id as 'values',
       st.translation_text as labels,
       st.delete_flg AS deleteFlg
  from v_structure_item_all AS st
       -- 全工場共通の表示順
       LEFT OUTER JOIN ms_structure_order AS order_common
       ON  st.structure_id = order_common.structure_id
       AND order_common.factory_id = 0
 where st.structure_group_id = /*param1*/0
   and st.language_id = /*languageId*/'ja'

/*IF param2 != null*/
   and st.structure_layer_no = /*param2*/0
/*END*/

/*IF param3 != null && param3 != ''*/
   and st.parent_structure_id = /*param3*/0
/*END*/


order by st.structure_group_id,st.structure_layer_no,
-- 表示順は全工場共通(0)より取得
row_number() over(ORDER BY coalesce(order_common.display_order,32768), st.structure_id)
