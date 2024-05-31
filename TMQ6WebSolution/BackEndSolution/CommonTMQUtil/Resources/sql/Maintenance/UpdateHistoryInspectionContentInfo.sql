UPDATE ma_history_inspection_content 
SET
/*@ExcelPort
    [history_inspection_site_id] = @HistoryInspectionSiteId
    , [inspection_content_structure_id] = @InspectionContentStructureId
    , 
@ExcelPort*/
    [follow_flg] = @FollowFlg
    , [follow_plan_date] = @FollowPlanDate
    , [follow_content] = @FollowContent
    , [follow_completion_date] = @FollowCompletionDate
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
    , [work_record] = @WorkRecord
WHERE
    [history_inspection_content_id] = @HistoryInspectionContentId
