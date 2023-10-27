WITH schedule AS(
    SELECT
        sc.maintainance_schedule_id
    FROM
        mc_maintainance_schedule AS sc
    WHERE
        EXISTS(
            SELECT
                *
            FROM
                mc_management_standards_content AS con
            WHERE
                sc.management_standards_content_id = con.management_standards_content_id
            AND con.long_plan_id = @LongPlanId
            AND con.management_standards_content_id IN @ManagementStandardsContentIdList
        )
)
UPDATE
    sc_d
SET
    sc_d.schedule_date = DATEADD(MONTH, @PostponeMonths, sc_d.schedule_date),
    sc_d.update_serialid = sc_d.update_serialid + 1,
    sc_d.update_datetime = @UpdateDatetime,
    sc_d.update_user_id = @UpdateUserId
FROM
    mc_maintainance_schedule_detail AS sc_d
WHERE
    sc_d.schedule_date >= @PostponeDate
AND EXISTS(
        SELECT
            *
        FROM
            schedule AS sc
        WHERE
            sc_d.maintainance_schedule_id = sc.maintainance_schedule_id
    )
