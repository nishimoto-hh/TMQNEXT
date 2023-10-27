INSERT INTO #temp_location
SELECT DISTINCT
    dbo.get_target_layer_id(ms.structure_id, 0)
FROM
    ms_structure ms
    INNER JOIN
        #temp_location temp
    ON  ms.structure_id = temp.structure_id
WHERE
    ms.structure_group_id = 1000
AND ms.structure_layer_no = 1