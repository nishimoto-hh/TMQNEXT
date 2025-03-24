--/*
--* 一覧の項目の内容を取得するSQL
--*/
WITH schedule_start_date AS ( -- 保全スケジュールを機器別管理基準内容IDごとに取得(同じ値なら最大の開始日のレコード)
    SELECT
        sc.maintainance_schedule_id
        , sc.management_standards_content_id
        , sc.cycle_year
        , sc.cycle_month
        , sc.cycle_day
        , sc.start_date
        , sc.disp_cycle
        , sc.update_datetime 
    FROM
        mc_maintainance_schedule AS sc 
    WHERE
        NOT EXISTS ( 
            SELECT
                * 
            FROM
                mc_maintainance_schedule AS sub 
            WHERE
                sc.management_standards_content_id = sub.management_standards_content_id 
                AND sc.start_date < sub.start_date
        )
) 
, -- 上で取得した保全スケジュールを機器別管理基準内容ID、開始日ごとに取得(同じ値なら最大の更新日時のレコード)
schedule AS ( 
    SELECT
        main.maintainance_schedule_id
        , main.management_standards_content_id
        , main.cycle_year
        , main.cycle_month
        , main.cycle_day
        , main.start_date
        , main.disp_cycle 
    FROM
        schedule_start_date AS main 
    WHERE
        NOT EXISTS ( 
            SELECT
                * 
            FROM
                schedule_start_date AS sub 
            WHERE
                main.management_standards_content_id = sub.management_standards_content_id 
                AND main.start_date = sub.start_date 
                AND main.update_datetime < sub.update_datetime
        )
) ,target AS (
SELECT
    lplan.long_plan_id
    , lplan.location_factory_structure_id AS factory_id
    , machine.location_factory_structure_id AS machine_factory_id
    , machine.machine_id
    , com.management_standards_component_id
    , con.management_standards_content_id
    , schedule.maintainance_schedule_id
    , machine.machine_no
    , machine.machine_name
    , com.inspection_site_structure_id
    , con.inspection_site_importance_structure_id
    , con.inspection_site_conservation_structure_id
    , con.inspection_content_structure_id
    , schedule.cycle_year
    , schedule.cycle_month
    , schedule.cycle_day
    , schedule.start_date
    , lplan.budget_personality_structure_id
    ,                                           -- グループ(折り畳み単位)毎の連番、同一グループに同じ値が入る
    DENSE_RANK() OVER ( 
        ORDER BY
            machine.machine_no
            , machine.machine_name
    ) AS list_group_id
    , CONCAT_WS( 
        '|'
        , lplan.long_plan_id
        , machine.machine_id
        , com.management_standards_component_id
        , con.management_standards_content_id
    ) AS key_id
    , lplan.location_structure_id
    , lplan.job_structure_id 
FROM
    ln_long_plan lplan 
    INNER JOIN mc_management_standards_content con 
        ON con.long_plan_id = lplan.long_plan_id 
    INNER JOIN mc_management_standards_component com 
        ON com.management_standards_component_id = con.management_standards_component_id 
    INNER JOIN mc_machine machine 
        ON machine.machine_id = com.machine_id 
    LEFT JOIN schedule 
        ON schedule.management_standards_content_id = con.management_standards_content_id
)