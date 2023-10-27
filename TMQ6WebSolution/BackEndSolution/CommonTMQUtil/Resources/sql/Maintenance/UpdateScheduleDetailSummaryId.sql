UPDATE
    mc_maintainance_schedule_detail
SET
    [summary_id] = @SummaryId
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId
WHERE
    [maintainance_schedule_detail_id] = @MaintainanceScheduleDetailId