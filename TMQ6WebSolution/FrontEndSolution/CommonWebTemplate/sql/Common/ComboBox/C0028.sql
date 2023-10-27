/*
 * ExcelPortダウンロード対象機能 コンボ用データリスト用SQL.(絞り込み条件付き、拡張データとの紐付け有）
 * exparam1:アイテム拡張マスタの拡張データ1(対象機能ID)
 * exparam2:アイテム拡張マスタの拡張データ2(対象シート番号※子階層のみ)
 */
-- C00028
SELECT 
     item.factory_id AS factoryId
    ,item.location_structure_id AS translationFactoryId
    ,item.structure_id AS 'values'
    ,item.translation_text AS labels
    ,item_ex1.extension_data AS exparam1
    ,item_ex2.extension_data AS exparam2
 
FROM v_structure_item_all AS item

LEFT OUTER JOIN ms_item_extension AS item_ex1 
ON item.structure_item_id=item_ex1.item_id 
AND item_ex1.sequence_no=1

LEFT OUTER JOIN ms_item_extension AS item_ex2
ON item.structure_item_id=item_ex2.item_id 
AND item_ex2.sequence_no=2

-- 全工場共通の表示順
LEFT OUTER JOIN ms_structure_order AS order_common
ON  item.structure_id = order_common.structure_id
AND order_common.factory_id = 0

WHERE 
    item.structure_group_id = /*param1*/1000
AND item.language_id = /*languageId*/'ja'
   
/*IF param2 != null && param2 != '' */
    AND item.structure_layer_no = /*param2*/0
/*END*/

/*IF param3 == null || param3 == '' */
    -- 親階層指定なしの場合、拡張項目1の機能IDがユーザ機能権限に含まれるもののみ抽出
     AND item_ex1.extension_data IN (
         SELECT conduct_id FROM ms_user_conduct_authority WHERE user_id = /*userId*/1001)
/*END*/

/*IF param3 != null && param3 != '' */
    -- 親階層指定有りの場合
    AND item.parent_structure_id = /*param3*/1
/*END*/

ORDER BY item.structure_group_id,item.structure_layer_no,

-- 表示順は全工場共通(0)より取得
row_number() OVER(ORDER BY coalesce(order_common.display_order,32768), item.structure_id)
