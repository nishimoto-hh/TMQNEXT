/*
 * ExcelPortダウンロード対象機能 コンボ用データリスト用SQL.(絞り込み条件付き、拡張データとの紐付け有）
 * id:アイテム拡張マスタの拡張データ1
 */
-- EPC0005
SELECT 
     CONVERT(int, item_ex.extension_data) AS id
    --,coalesce(item.factory_id, 0) AS factory_id
    ,CASE
        WHEN item.factory_id = 0 THEN item.location_structure_id    -- 構成マスタの工場IDが0の場合は翻訳マスタの場所階層ID(工場ID)を返す
        ELSE coalesce(item.factory_id, 0)                           -- 上記以外は構成マスタの工場ID
     END AS factory_id
    ,item.parent_structure_id AS parent_id
    ,item.translation_text AS name
    ,coalesce(order_common.display_order, order_factory.display_order, item.structure_id) AS display_order

FROM v_structure_item_all AS item

-- 工場共通表示順
LEFT OUTER JOIN ms_structure_order AS order_common
ON  item.structure_id = order_common.structure_id
AND order_common.factory_id = 0

-- 工場別表示順
LEFT OUTER JOIN ms_structure_order AS order_factory
ON  item.structure_id = order_factory.structure_id
AND item.factory_id = order_factory.factory_id

-- 拡張項目
LEFT OUTER JOIN ms_item_extension AS item_ex 
ON item.structure_item_id=item_ex.item_id 
AND item_ex.sequence_no=1

WHERE 
    item.structure_group_id = /*param1*/2120
AND item.language_id = /*languageId*/'ja'

/*IF factoryIdList != null && factoryIdList.Count > 0*/
    AND item.factory_id in /*factoryIdList*/(0,5,6)
    AND item.location_structure_id in /*factoryIdList*/(0,5,6)
/*END*/

/*IF param2 != null && param2 != '' */
    AND item.structure_layer_no = /*param2*/0
/*END*/

 -- 工場別未使用標準アイテムに工場が含まれていないものを表示
AND
    NOT EXISTS(
        SELECT
           *
        FROM
            ms_structure_unused AS unused
        WHERE
            unused.factory_id = item.factory_id
        AND unused.structure_id = item.structure_id
    )

-- 表示順は親構成ID、工場共通表示順、工場別表示順、拡張項目1の順
ORDER BY 
 item.parent_structure_id
,coalesce(order_common.display_order, order_factory.display_order, item_ex.extension_data)
