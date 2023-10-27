UPDATE
    mc_machine_parent_info
SET
    parent_moto_id = NULL,
    update_serialid = update_serialid+1,
    update_datetime = @UpdateDatetime,
    update_user_id = @UpdateUserId
WHERE
    parent_id = @ParentId
