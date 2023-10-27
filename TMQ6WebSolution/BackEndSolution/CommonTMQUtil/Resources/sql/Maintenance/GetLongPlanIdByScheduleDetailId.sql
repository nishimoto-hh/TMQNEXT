SELECT
    mmsc.long_plan_id
FROM
    mc_maintainance_schedule_detail mmsd
    INNER JOIN
        mc_maintainance_schedule mms
    ON  mmsd.maintainance_schedule_id = mms.maintainance_schedule_id
    INNER JOIN
        mc_management_standards_content mmsc
    ON  mms.management_standards_content_id = mmsc.management_standards_content_id
WHERE
    mmsd.maintainance_schedule_detail_id = @MaintainanceScheduleDetailId