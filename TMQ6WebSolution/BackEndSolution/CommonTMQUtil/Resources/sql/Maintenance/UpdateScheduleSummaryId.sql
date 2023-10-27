UPDATE mc_maintainance_schedule_detail 
SET
    summary_id = null
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
WHERE
    summary_id = @SummaryId
