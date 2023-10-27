/*
 * 機器別管理基準選択画面　登録　機器別管理基準内容の長期計画IDを更新するSQL
 */
UPDATE
    mc_management_standards_content
SET
    long_plan_id = @LongPlanId,
	update_serialid = update_serialid+1,
	update_datetime = @UpdateDatetime,
	update_user_id = @UpdateUserId
WHERE
    management_standards_content_id = @ManagementStandardsContentId
