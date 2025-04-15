UPDATE ma_summary 
SET
    long_plan_id = NULL 

    -- 以下は保全活動件名のupdate文で更新されるためここでは更新しない
    --, update_serialid = update_serialid + 1
    --, update_datetime = @UpdateDatetime
    --, update_user_id = @UpdateUserId
WHERE
    summary_id = @SummaryId
