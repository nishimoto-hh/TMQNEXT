/*
* 一覧の項目の内容を取得するSQL
*/
WITH target AS (
SELECT
    lplan.long_plan_id,
    lplan.location_factory_structure_id AS factory_id,
    machine.location_factory_structure_id AS machine_factory_id,
    machine.machine_id,
    com.management_standards_component_id,
    con.management_standards_content_id,
    schedule.maintainance_schedule_id,
    machine.machine_no,
    machine.machine_name,
    com.inspection_site_structure_id,
    con.inspection_site_importance_structure_id,
    con.inspection_site_conservation_structure_id,
    con.inspection_content_structure_id,
    schedule.cycle_year,
    schedule.cycle_month,
    schedule.cycle_day,
    schedule.start_date,
    lplan.budget_personality_structure_id,
    -- グループ(折り畳み単位)毎の連番、同一グループに同じ値が入る
    DENSE_RANK() OVER(ORDER BY machine.machine_no, machine.machine_name) AS list_group_id,
    CONCAT_WS('|',lplan.long_plan_id,machine.machine_id,com.management_standards_component_id,con.management_standards_content_id,schedule.maintainance_schedule_id) AS key_id,
    lplan.location_structure_id,
    lplan.job_structure_id
FROM
    ln_long_plan AS lplan
    INNER JOIN
        mc_management_standards_content AS con
    ON  (
            con.long_plan_id = lplan.long_plan_id
        )
    INNER JOIN
        mc_management_standards_component AS com
    ON  (
            com.management_standards_component_id = con.management_standards_component_id
        )
    INNER JOIN
        mc_machine AS machine
    ON  (
            machine.machine_id = com.machine_id
        )
    LEFT OUTER JOIN
        mc_maintainance_schedule AS schedule
    ON  (
            schedule.management_standards_content_id = con.management_standards_content_id
        )
)