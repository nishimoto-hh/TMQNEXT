/*
 * 長期計画に紐づいた機器別管理基準に保全活動が作成されているかを確認するSQL
*/
SELECT
    COUNT(ms_con.management_standards_content_id)
FROM
    mc_management_standards_content AS ms_con
    INNER JOIN
        mc_maintainance_schedule AS schedule
    ON  (
            schedule.management_standards_content_id = ms_con.management_standards_content_id
        )
WHERE
    ms_con.long_plan_id = @LongPlanId
AND EXISTS(
        SELECT
            *
        FROM
            mc_maintainance_schedule_detail AS sc_d
        WHERE
            sc_d.maintainance_schedule_id = schedule.maintainance_schedule_id
        AND sc_d.summary_id IS NOT NULL
    )
