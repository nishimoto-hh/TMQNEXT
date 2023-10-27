SELECT
     ms.structure_id AS StructureId
    ,ms.factory_id AS FactoryId
    ,ms.structure_group_id AS GroupId
    ,ms.structure_layer_no AS LayerNo
    ,0 AS DutyFlg
FROM
    ms_structure ms
LEFT JOIN ms_item mi
    ON ms.structure_item_id = mi.item_id
AND mi.delete_flg = 0
    -- 表示順(工場共通)
LEFT OUTER JOIN ms_structure_order AS order_common
    ON  (ms.structure_id = order_common.structure_id
        AND order_common.factory_id = 0)
WHERE
    ms.structure_group_id = /*GroupId*/1000    -- 構成グループID
    /*IF FactoryId != null */
    AND ms.factory_id = /*FactoryId*/0   -- 構成階層番号
	/*END*/
    /*IF LayerNo != null */
    AND ms.structure_layer_no = /*LayerNo*/1   -- 構成階層番号
    --ELSE AND structure_layer_no > 0
	/*END*/
	AND ms.delete_flg = 0
    
ORDER BY ms.structure_layer_no, ms.parent_structure_id, coalesce(order_common.display_order, 32768)
