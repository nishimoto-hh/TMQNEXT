/*
 * 名称マスタ コンボ用データリスト用SQL.(絞り込み条件付き、拡張データとの紐付け有）
 * 工場による絞り込み無し(工場個別アイテムが存在しない構成グループについて使用可).C0002の工場指定なしバージョン
 * exparam1:アイテム拡張マスタの拡張データ
 * exparam2:アイテム拡張マスタの拡張データの値を構成アイテムIDに持つ、構成マスタの構成ID
 * exparam3:アイテム拡張マスタの連番
 */
-- C0039
select item.factory_id as factoryId,
       item.location_structure_id as translationFactoryId,
       item.structure_id as 'values',
       item.translation_text as labels,
       item_ex.extension_data as exparam1,
       ex_item_1.structure_id as exparam2,
       ex_item_1.translation_text as exparam3,
       item.delete_flg AS deleteFlg
  from v_structure_item_all as item
       left outer join ms_item_extension as item_ex on (item.structure_item_id=item_ex.item_id and item_ex.sequence_no=/*param3*/0) 
       left outer join v_structure_item_all as  ex_item_1 on (item_ex.extension_data = CAST(ex_item_1.structure_item_id AS varchar) and item.language_id = ex_item_1.language_id)
       -- 全工場共通の表示順
       LEFT OUTER JOIN ms_structure_order AS order_common
       ON  item.structure_id = order_common.structure_id
       AND order_common.factory_id = 0
 where item.structure_group_id = /*param1*/0
   and item.language_id = /*languageId*/'ja'
   
/*IF param2 != null && param2 != '' */
   and item.structure_layer_no = /*param2*/0
/*END*/

order by item.structure_group_id,item.structure_layer_no,
-- 表示順は全工場共通(0)より取得
row_number() over(ORDER BY coalesce(order_common.display_order,32768), item.structure_id)


