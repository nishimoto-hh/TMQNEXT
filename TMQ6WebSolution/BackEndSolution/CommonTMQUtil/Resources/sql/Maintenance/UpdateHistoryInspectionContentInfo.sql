UPDATE ma_history_inspection_content 
SET
    [follow_flg] = @FollowFlg
    , [follow_plan_date] = @FollowPlanDate
    , [follow_content] = @FollowContent
    , [follow_completion_date] = @FollowCompletionDate
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
WHERE
    [history_inspection_content_id] = @HistoryInspectionContentId
