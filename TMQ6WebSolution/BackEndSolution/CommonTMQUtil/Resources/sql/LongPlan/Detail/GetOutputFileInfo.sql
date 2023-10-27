WITH FILE_LIST AS(
    SELECT
        dbo.get_file(1620, content.management_standards_content_id) AS file_info
    FROM
        mc_management_standards_content AS content
    WHERE
        content.long_plan_id = @LongPlanId
)
SELECT
    file_info
FROM
    FILE_LIST
WHERE
    file_info IS NOT NULL