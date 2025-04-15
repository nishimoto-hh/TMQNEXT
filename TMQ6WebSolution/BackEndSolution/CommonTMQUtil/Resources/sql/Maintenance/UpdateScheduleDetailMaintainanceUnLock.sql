UPDATE mc_maintainance_schedule_detail 
SET
    complition = 0
    , summary_id = NULL 
    , update_serialid = update_serialid + 1
    , update_datetime = @UpdateDatetime
    , update_user_id = @UpdateUserId
WHERE
    summary_id = @SummaryId
