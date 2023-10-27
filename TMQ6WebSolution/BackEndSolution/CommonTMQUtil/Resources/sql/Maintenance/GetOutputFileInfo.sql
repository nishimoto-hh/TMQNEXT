WITH FILE_LIST AS ( 
    SELECT
        dbo.get_file(1620, content.management_standards_content_id) AS file_info 
    FROM
        mc_management_standards_content AS content 
        LEFT JOIN mc_maintainance_schedule AS schedule 
            ON content.management_standards_content_id = schedule.management_standards_content_id 
        LEFT JOIN mc_maintainance_schedule_detail AS detail 
            ON schedule.maintainance_schedule_id = detail.maintainance_schedule_id 
    WHERE
        detail.summary_id = @SummaryId
) 
SELECT
    file_info 
FROM
    FILE_LIST 
WHERE
    file_info IS NOT NULL
