WITH CONDITION AS(
    -- åüçıèåè
    SELECT
        content.long_plan_id,
        detail.schedule_date
    FROM
        mc_maintainance_schedule_detail detail
        INNER JOIN
            mc_maintainance_schedule schedule
        ON  detail.maintainance_schedule_id = schedule.maintainance_schedule_id
        INNER JOIN
            mc_management_standards_content content
        ON  schedule.management_standards_content_id = content.management_standards_content_id
    WHERE
        detail.maintainance_schedule_detail_id = @MaintainanceScheduleDetailId
)
SELECT
    MAX(mmsd.update_datetime) AS max_update_datetime_schedule
FROM
    mc_maintainance_schedule_detail mmsd
    INNER JOIN
        mc_maintainance_schedule mms
    ON  mmsd.maintainance_schedule_id = mms.maintainance_schedule_id
    INNER JOIN
        mc_management_standards_content mmsc
    ON  mms.management_standards_content_id = mmsc.management_standards_content_id
    INNER JOIN
        CONDITION
    ON  mmsc.long_plan_id = CONDITION.long_plan_id
    AND FORMAT(mmsd.schedule_date, 'yyyyMM') = FORMAT(CONDITION.schedule_date, 'yyyyMM')
WHERE
    mmsd.summary_id IS NULL