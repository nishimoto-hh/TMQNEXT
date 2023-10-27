SELECT
	 mu.location_structure_id AS structureId
	,mu.duty_flg AS dutyFlg
    ,ms.factory_id AS factoryId
    ,ms.structure_group_id AS groupId
    ,ms.structure_layer_no AS layerNo
FROM ms_user_belong mu
LEFT JOIN ms_structure ms
    ON  mu.location_structure_id = ms.structure_id
    AND ms.delete_flg = 0
LEFT JOIN ms_item mi
    ON ms.structure_item_id = mi.item_id
AND mi.delete_flg = 0    
    -- 表示順(工場共通)
LEFT OUTER JOIN ms_structure_order AS order_common
    ON  (mu.location_structure_id = order_common.structure_id
        AND order_common.factory_id = 0)
WHERE mu.delete_flg != 1
AND mu.user_id = /*UserId*/1001
ORDER BY ms.structure_group_id, ms.structure_layer_no, coalesce(order_common.display_order, 32768),mu.location_structure_id
