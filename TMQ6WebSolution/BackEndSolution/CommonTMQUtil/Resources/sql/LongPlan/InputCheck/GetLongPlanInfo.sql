SELECT
    long_plan.long_plan_id,
    content.management_standards_content_id,
    schedule_detail.maintainance_schedule_detail_id,
    schedule_detail.schedule_date,
    format(schedule_detail.schedule_date, 'yyyyMM') AS schedule_ym,
    schedule_detail.summary_id
FROM
    ln_long_plan AS long_plan
    INNER JOIN
        mc_management_standards_content AS content
    ON  (
            long_plan.long_plan_id = content.long_plan_id
        )
    INNER JOIN
        mc_maintainance_schedule AS schedule
    ON  (
            content.management_standards_content_id = schedule.management_standards_content_id
        )
    INNER JOIN
        mc_maintainance_schedule_detail AS schedule_detail
    ON  (
            schedule.maintainance_schedule_id = schedule_detail.maintainance_schedule_id
        )
WHERE
    long_plan.long_plan_id = @LongPlanId
AND schedule_detail.schedule_date BETWEEN @ScheduleDateFrom AND @ScheduleDateTo
/*@Content
AND content.management_standards_content_id IN @ManagementStandardsContentIdList
@Content*/
ORDER BY
    schedule_detail.schedule_date
