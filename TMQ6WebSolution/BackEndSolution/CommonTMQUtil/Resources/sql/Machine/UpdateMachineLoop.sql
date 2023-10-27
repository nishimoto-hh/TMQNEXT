UPDATE
    mc_loop_info
SET
    loop_moto_id = NULL,
    update_serialid = update_serialid+1,
    update_datetime = @UpdateDatetime,
    update_user_id = @UpdateUserId
WHERE
    loop_id = @LoopId
