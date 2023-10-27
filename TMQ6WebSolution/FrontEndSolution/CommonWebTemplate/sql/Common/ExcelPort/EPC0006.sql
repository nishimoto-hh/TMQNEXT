/*
 * ExcelPort MQ分類 コンボ用データリスト用SQL
 * parent_id:アイテム拡張マスタの拡張データ5(点検・故障分類)
 */
-- EPC0006
SELECT DISTINCT
     item.structure_id AS id
    ,coalesce(item.factory_id, 0) AS factory_id
    ,item_ex5.extension_data AS parent_id
    ,item.translation_text AS name
    ,item_ex1.extension_data AS exparam1
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
LEFT OUTER JOIN ms_item_extension AS item_ex1 
ON item.structure_item_id=item_ex1.item_id 
AND item_ex1.sequence_no=1

LEFT OUTER JOIN ms_item_extension AS item_ex5
ON item.structure_item_id=item_ex5.item_id 
AND item_ex5.sequence_no=5

WHERE 
    item.structure_group_id = /*param1*/1000
AND item.language_id = /*languageId*/'ja'

/*IF param1 != 1000 && param1 != 1010 && factoryIdList != null && factoryIdList.Count > 0*/
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

-- 表示順は親構成ID、工場共通表示順、工場別表示順、構成IDの順
ORDER BY 
 item_ex5.extension_data
,coalesce(order_common.display_order, order_factory.display_order, item.structure_id)
