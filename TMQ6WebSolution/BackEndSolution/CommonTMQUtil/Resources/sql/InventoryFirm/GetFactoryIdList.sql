SELECT
    ms.structure_id
FROM
    ms_structure ms
WHERE
    ms.structure_group_id = 1000
AND ms.structure_layer_no = 1
AND ms.structure_id IN(
        SELECT
            belong.location_structure_id
        FROM
            ms_user_belong belong
        WHERE
            belong.user_id = @UserId
        AND belong.delete_flg = 0
    )
