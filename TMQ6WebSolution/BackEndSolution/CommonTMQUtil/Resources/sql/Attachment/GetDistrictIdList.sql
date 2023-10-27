SELECT DISTINCT
    dbo.get_target_layer_id(ms.structure_id, 0) AS districtId
FROM
    ms_structure ms
WHERE
    ms.structure_group_id = 1000
AND ms.structure_layer_no = 1
AND ms.structure_id in @LocationIdList