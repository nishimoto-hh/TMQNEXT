/*
 * ExcelPortダウンロード対象機能 コンボ用データリスト用SQL.(絞り込み条件付き、拡張データとの紐付け有）
 * exparam1:アイテム拡張マスタの拡張データ1～20
 */
-- EPC0001
SELECT DISTINCT
     item.structure_id AS id
    --,coalesce(item.factory_id, 0) AS factory_id
    ,CASE
        WHEN item.factory_id = 0 THEN item.location_structure_id    -- 構成マスタの工場IDが0の場合は翻訳マスタの場所階層ID(工場ID)を返す
        ELSE coalesce(item.factory_id, 0)                           -- 上記以外は構成マスタの工場ID
     END AS factory_id
    ,item.parent_structure_id AS parent_id
    ,item.translation_text AS name
    ,item_ex1.extension_data AS exparam1
    ,item_ex2.extension_data AS exparam2
    ,item_ex3.extension_data AS exparam3
    ,item_ex4.extension_data AS exparam4
    ,item_ex5.extension_data AS exparam5
    ,item_ex6.extension_data AS exparam6
    ,item_ex7.extension_data AS exparam7
    ,item_ex8.extension_data AS exparam8
    ,item_ex9.extension_data AS exparam9
    ,item_ex10.extension_data AS exparam10
    ,item_ex11.extension_data AS exparam11
    ,item_ex12.extension_data AS exparam12
    ,item_ex13.extension_data AS exparam13
    ,item_ex14.extension_data AS exparam14
    ,item_ex15.extension_data AS exparam15
    ,item_ex16.extension_data AS exparam16
    ,item_ex17.extension_data AS exparam17
    ,item_ex18.extension_data AS exparam18
    ,item_ex19.extension_data AS exparam19
    ,item_ex20.extension_data AS exparam20
    ,coalesce(order_common.display_order, order_factory.display_order, item.structure_id) AS display_order

FROM v_structure_item_all AS item

-- 場所階層と職種機種の場合、対象構成マスタ情報一時テーブルと結合する
/*IF param1 == 1000 || param1 == 1010*/
INNER JOIN #temp_structure_all st
ON item.structure_id = st.structure_id
/*END*/

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

LEFT OUTER JOIN ms_item_extension AS item_ex2
ON item.structure_item_id=item_ex2.item_id 
AND item_ex2.sequence_no=2

LEFT OUTER JOIN ms_item_extension AS item_ex3
ON item.structure_item_id=item_ex3.item_id 
AND item_ex3.sequence_no=3

LEFT OUTER JOIN ms_item_extension AS item_ex4
ON item.structure_item_id=item_ex4.item_id 
AND item_ex4.sequence_no=4

LEFT OUTER JOIN ms_item_extension AS item_ex5
ON item.structure_item_id=item_ex5.item_id 
AND item_ex5.sequence_no=5

LEFT OUTER JOIN ms_item_extension AS item_ex6
ON item.structure_item_id=item_ex6.item_id 
AND item_ex6.sequence_no=6

LEFT OUTER JOIN ms_item_extension AS item_ex7
ON item.structure_item_id=item_ex7.item_id 
AND item_ex7.sequence_no=7

LEFT OUTER JOIN ms_item_extension AS item_ex8
ON item.structure_item_id=item_ex8.item_id 
AND item_ex8.sequence_no=8

LEFT OUTER JOIN ms_item_extension AS item_ex9
ON item.structure_item_id=item_ex9.item_id 
AND item_ex9.sequence_no=9

LEFT OUTER JOIN ms_item_extension AS item_ex10
ON item.structure_item_id=item_ex10.item_id 
AND item_ex10.sequence_no=10

LEFT OUTER JOIN ms_item_extension AS item_ex11 
ON item.structure_item_id=item_ex11.item_id 
AND item_ex11.sequence_no=11

LEFT OUTER JOIN ms_item_extension AS item_ex12
ON item.structure_item_id=item_ex12.item_id 
AND item_ex12.sequence_no=12

LEFT OUTER JOIN ms_item_extension AS item_ex13
ON item.structure_item_id=item_ex13.item_id 
AND item_ex13.sequence_no=13

LEFT OUTER JOIN ms_item_extension AS item_ex14
ON item.structure_item_id=item_ex14.item_id 
AND item_ex14.sequence_no=14

LEFT OUTER JOIN ms_item_extension AS item_ex15
ON item.structure_item_id=item_ex15.item_id 
AND item_ex15.sequence_no=15

LEFT OUTER JOIN ms_item_extension AS item_ex16
ON item.structure_item_id=item_ex16.item_id 
AND item_ex16.sequence_no=16

LEFT OUTER JOIN ms_item_extension AS item_ex17
ON item.structure_item_id=item_ex17.item_id 
AND item_ex17.sequence_no=17

LEFT OUTER JOIN ms_item_extension AS item_ex18
ON item.structure_item_id=item_ex18.item_id 
AND item_ex18.sequence_no=18

LEFT OUTER JOIN ms_item_extension AS item_ex19
ON item.structure_item_id=item_ex19.item_id 
AND item_ex19.sequence_no=19

LEFT OUTER JOIN ms_item_extension AS item_ex20
ON item.structure_item_id=item_ex20.item_id 
AND item_ex20.sequence_no=20

WHERE 
    item.structure_group_id = /*param1*/1000
AND item.language_id = /*languageId*/'ja'

/*IF param1 != 1000 && param1 != 1010 && factoryIdList != null && factoryIdList.Count > 0*/
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

-- 表示順は親構成ID、工場共通表示順、工場別表示順、構成IDの順
ORDER BY 
 item.parent_structure_id
,coalesce(order_common.display_order, order_factory.display_order, item.structure_id)
