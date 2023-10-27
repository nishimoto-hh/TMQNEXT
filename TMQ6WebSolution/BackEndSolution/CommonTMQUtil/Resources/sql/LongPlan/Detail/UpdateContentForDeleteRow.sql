/*
 * 参照画面　行削除　機器別管理基準の長計件名IDをNullに更新
 */
UPDATE
    mc_management_standards_content
SET
    long_plan_id = null,
	update_serialid = update_serialid+1,
	update_datetime = @UpdateDatetime,
	update_user_id = @UpdateUserId
WHERE
    management_standards_content_id = @ManagementStandardsContentId
