SELECT
    ex.extension_data AS ex_data
FROM
    ms_user mu
    LEFT JOIN
        ms_structure ms
    ON  mu.authority_level_id = ms.structure_id
    AND ms.structure_group_id = 9040
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
    AND ex.sequence_no = 1
WHERE
    mu.user_id = @ApprovalUserId