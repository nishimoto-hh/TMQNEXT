UPDATE
    pt_stock_confirm
SET
    update_serialid = update_serialid + 1,
    delete_flg = 1,
    update_datetime = @UpdateDatetime,
    update_user_id = @UpdateUserId
WHERE
    FORMAT(target_month, 'yyyy/MM') = @TargetMonth
AND factory_id = @FactoryId
AND COALESCE(parts_job_id, 0) = COALESCE(@PartsJobId, 0)
AND delete_flg = 0
