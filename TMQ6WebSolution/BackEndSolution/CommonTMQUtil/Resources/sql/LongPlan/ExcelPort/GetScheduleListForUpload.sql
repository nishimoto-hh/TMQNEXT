/*@LongPlan
-- 長計件名ID
WITH long_plan_ids AS (
SELECT
    *
FROM
    STRING_SPLIT(@LongPlanIdList, ',')
)
@LongPlan*/

/*@Content
-- 保全項目ID
WITH management_standards_content_ids AS (
SELECT
    *
FROM
    STRING_SPLIT(@ManagementStandardsContentIdList, ',')
)
@Content*/

SELECT
/*@LongPlan
    lp.long_plan_id,
@LongPlan*/
    con.management_standards_content_id,
    ms.maintainance_schedule_id,
    md.maintainance_schedule_detail_id,
    md.schedule_date,
    md.summary_id,
    md.complition
FROM
/*@LongPlan
    ln_long_plan AS lp
    INNER JOIN
        mc_management_standards_content AS con
    ON  (
            lp.long_plan_id = con.long_plan_id
        )
@LongPlan*/
/*@Content
    mc_management_standards_content AS con
@Content*/

    INNER JOIN
        mc_maintainance_schedule AS ms
    ON  (
            con.management_standards_content_id = ms.management_standards_content_id
        )
    INNER JOIN
        mc_maintainance_schedule_detail AS md
    ON  (
            ms.maintainance_schedule_id = md.maintainance_schedule_id
        )
WHERE
/*@LongPlan
    EXISTS(
        SELECT * FROM long_plan_ids temp
        WHERE lp.long_plan_id = temp.value)
@LongPlan*/

/*@Content
    EXISTS(
        SELECT * FROM management_standards_content_ids temp
        WHERE con.management_standards_content_id = temp.value)
@Content*/

AND FORMAT(md.schedule_date, 'yyyyMM') BETWEEN @ScheduleDateFrom AND @ScheduleDateTo

ORDER BY
/*@LongPlan
    lp.long_plan_id,
@LongPlan*/
    con.management_standards_content_id,
    ms.maintainance_schedule_id,
    md.schedule_date
