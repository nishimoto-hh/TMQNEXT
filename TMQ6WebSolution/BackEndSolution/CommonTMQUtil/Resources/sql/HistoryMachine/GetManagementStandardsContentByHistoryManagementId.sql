SELECT
    content.hm_management_standards_content_id
FROM
    hm_mc_management_standards_content content
WHERE
    content.history_management_id = @HistoryManagementId