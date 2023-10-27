SELECT
    mub.location_structure_id AS factory_id
FROM
    ms_user_belong mub
    INNER JOIN
        ms_structure ms
    ON  mub.location_structure_id = ms.structure_id
WHERE
    mub.user_id = @UserId
AND ms.structure_group_id = 1000
AND ms.structure_layer_no = 1