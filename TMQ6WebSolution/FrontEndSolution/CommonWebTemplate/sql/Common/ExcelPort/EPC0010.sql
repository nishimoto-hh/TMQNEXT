/*
 * ExcelPortダウンロード対象機能 コンボ用データリスト用SQL.(絞り込み条件付き、拡張データとの紐付け有）
 * id:アイテム拡張マスタの拡張データ1
 * name:アイテム拡張マスタの拡張データ2の翻訳IDと紐づく翻訳
 */
-- EPC0010

-- スケジュール文字列
SELECT
     CONVERT(int, item_ex1.extension_data) AS id
    ,tr.translation_text AS name
    ,CASE
        WHEN ms.factory_id = 0 THEN tr.location_structure_id    -- 構成マスタの工場IDが0の場合は翻訳マスタの場所階層ID(工場ID)を返す
        ELSE coalesce(ms.factory_id, 0)                         -- 上記以外は構成マスタの工場ID
     END AS factory_id
    ,coalesce(order_common.display_order, order_factory.display_order, item_ex1.extension_data) AS display_order
    
FROM
    ms_structure ms

-- 拡張項目1(ID)
LEFT JOIN ms_item_extension item_ex1
ON  ms.structure_item_id = item_ex1.item_id
AND item_ex1.sequence_no = 1

-- 拡張項目2(翻訳ID)
INNER JOIN ms_item_extension item_ex2
ON  ms.structure_item_id = item_ex2.item_id
AND item_ex2.sequence_no = 2

-- 翻訳
LEFT JOIN ms_translation tr
ON  CONVERT(int, item_ex2.extension_data) = tr.translation_id
AND tr.language_id = /*languageId*/'ja'

-- 工場共通表示順
LEFT OUTER JOIN ms_structure_order AS order_common
ON  ms.structure_id = order_common.structure_id
AND order_common.factory_id = 0

-- 工場別表示順
LEFT OUTER JOIN ms_structure_order AS order_factory
ON  ms.structure_id = order_factory.structure_id
AND ms.factory_id = order_factory.factory_id

WHERE
    ms.structure_group_id = /*param1*/1860

/*IF factoryIdList != null && factoryIdList.Count > 0*/
    AND ms.factory_id in /*factoryIdList*/(0,5,6)
    AND tr.location_structure_id in /*factoryIdList*/(0,5,6)
/*END*/

-- 拡張データ1のID指定の場合
/*IF param2 != null && param2 != '' */
AND 
    item_ex1.extension_data IN /*param2*/(1,5)
/*END*/

 -- 工場別未使用標準アイテムに工場が含まれていないものを表示
AND
    NOT EXISTS(
        SELECT
           *
        FROM
            ms_structure_unused AS unused
        WHERE
            unused.factory_id = ms.factory_id
        AND unused.structure_id = ms.structure_id
    )

-- 表示順は工場共通表示順、工場別表示順、拡張項目1の順
ORDER BY 
 coalesce(order_common.display_order, order_factory.display_order, item_ex1.extension_data)
