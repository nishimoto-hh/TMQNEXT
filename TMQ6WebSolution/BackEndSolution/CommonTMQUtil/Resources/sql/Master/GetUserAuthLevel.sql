SELECT
    ex.extension_data AS auth_level
FROM
    ms_user mu
    LEFT JOIN
        ms_structure ms
    ON  mu.authority_level_id = ms.structure_id
    AND ms.structure_group_id = 9040
    AND ms.delete_flg = 0
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
WHERE
    mu.user_id = @UserId