SELECT
    dbo.get_file(1620, content.management_standards_content_id) AS file_info
FROM
    mc_management_standards_content as content
WHERE
    content.long_plan_id = @LongPlanId
