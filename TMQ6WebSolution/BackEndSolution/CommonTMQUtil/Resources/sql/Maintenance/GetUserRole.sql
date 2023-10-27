SELECT
    mie.extension_data
FROM
    ms_user_role mur
    LEFT JOIN ms_structure ms
    ON  mur.role_id = ms.structure_id
    AND ms.structure_group_id = 9010
    INNER JOIN ms_item_extension mie
    ON  ms.structure_item_id = mie.item_id
WHERE
    mur.user_id = @UserId
AND mur.delete_flg = 0
