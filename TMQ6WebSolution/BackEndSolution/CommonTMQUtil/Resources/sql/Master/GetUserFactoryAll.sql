SELECT
    ms.structure_id AS factory_id
FROM
    ms_structure ms
WHERE
    ms.structure_group_id = 1000
AND ms.structure_layer_no = 1