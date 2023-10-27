UPDATE hm_mc_management_standards_content 
SET
    schedule_type_structure_id = @ScheduleTypeStructureId
    , update_serialid = update_serialid+1
    , update_datetime = @UpdateDatetime
    , update_user_id = @UpdateUserId
WHERE
    hm_management_standards_content_id = @HmManagementStandardsContentId
