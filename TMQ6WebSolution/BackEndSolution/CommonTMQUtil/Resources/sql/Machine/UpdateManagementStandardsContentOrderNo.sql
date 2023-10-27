UPDATE mc_management_standards_content 
SET
	  order_no = @OrderNo
    , update_serialid = update_serialid+1
    , update_datetime = @UpdateDatetime
    , update_user_id = @UpdateUserId
WHERE
    management_standards_content_id = @ManagementStandardsContentId
