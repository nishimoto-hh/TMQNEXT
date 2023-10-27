/*
 * 長期計画　ExcelPortアップロード 機器別管理基準情報の削除
 */
DELETE FROM
    mc_management_standards_content
WHERE
    management_standards_content_id = @ManagementStandardsContentId
