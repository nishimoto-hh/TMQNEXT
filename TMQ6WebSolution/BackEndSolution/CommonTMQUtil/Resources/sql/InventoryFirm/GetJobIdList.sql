SELECT
    ms.structure_id
FROM
    ms_structure ms
WHERE
    ms.structure_group_id = 1010
AND ms.structure_layer_no = 0
AND (
        ms.factory_id IN(
            SELECT
                belong.location_structure_id
            FROM
                ms_user_belong belong
            WHERE
                belong.user_id = @UserId
            AND belong.delete_flg = 0
        )
    OR  ms.factory_id = 0
    )
