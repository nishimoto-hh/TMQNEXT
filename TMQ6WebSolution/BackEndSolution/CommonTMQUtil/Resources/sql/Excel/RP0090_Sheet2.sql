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
) 
SELECT
    lplan.long_plan_id,											-- 長計件名ID
    machine.machine_id,											-- 機番ID
    com.management_standards_component_id,
    con.management_standards_content_id,
    schedule.maintainance_schedule_id,
    machine.machine_no,                                         -- 機器番号
    machine.machine_name,                                       -- 機器名称
--    com.inspection_site_structure_id,
--    viss.translation_text AS site,                              -- 部位
--    com.inspection_site_importance_structure_id,
--    vimp.translation_text AS site_importance,                   -- 部位重要度
--    com.inspection_site_conservation_structure_id,
--    vcns.translation_text AS conservation,                      -- 保全方式
--    con.inspection_content_structure_id,
--    vcon.translation_text AS inspection_content_name,           -- 保全項目

    -- [dbo].[get_v_structure_item](com.inspection_site_structure_id, temp.factoryId, temp.languageId) AS site,                        -- 部位
    --部位(翻訳)
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = com.inspection_site_structure_id
                AND st_f.factory_id IN(0, machine.location_factory_structure_id)
            )
        AND tra.structure_id = com.inspection_site_structure_id
    ) AS site,
    -- [dbo].[get_v_structure_item](con.inspection_site_importance_structure_id, temp.factoryId, temp.languageId) AS site_importance,  -- 部位重要度
    --部位重要度(翻訳)
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = con.inspection_site_importance_structure_id
                AND st_f.factory_id IN(0, machine.location_factory_structure_id)
            )
        AND tra.structure_id = con.inspection_site_importance_structure_id
    ) AS site_importance,
    -- [dbo].[get_v_structure_item](con.inspection_site_conservation_structure_id, temp.factoryId, temp.languageId) AS conservation,   -- 保全方式
    --保全方式(翻訳)
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = con.inspection_site_conservation_structure_id
                AND st_f.factory_id IN(0, machine.location_factory_structure_id)
            )
        AND tra.structure_id = con.inspection_site_conservation_structure_id
    ) AS conservation,
    -- [dbo].[get_v_structure_item](con.inspection_content_structure_id, temp.factoryId, temp.languageId) AS inspection_content_name,                  -- 保全項目
    --保全項目(翻訳)
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = con.inspection_content_structure_id
                AND st_f.factory_id IN(0, machine.location_factory_structure_id)
            )
        AND tra.structure_id = con.inspection_content_structure_id
    ) AS inspection_content_name,

    schedule.cycle_year,                                        -- 周期(年)
    schedule.cycle_month,                                       -- 周期(月)
    schedule.cycle_day,                                         -- 周期(日)
    FORMAT(schedule.start_date, 'yyyy/MM/dd') AS start_date,    -- 基準日（開始日）
--    lplan.budget_personality_structure_id,

--    vbdp.translation_text AS budget_personality,                -- 予算性格区分
    -- [dbo].[get_v_structure_item](lplan.budget_personality_structure_id, temp.factoryId, temp.languageId) AS budget_personality,                     -- 予算性格区分
    --予算性格区分(翻訳)
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lplan.budget_personality_structure_id
                AND st_f.factory_id IN(0, lplan.location_factory_structure_id)
            )
        AND tra.structure_id = lplan.budget_personality_structure_id
    ) AS budget_personality,

    -- グループ(折り畳み単位)毎の連番、同一グループに同じ値が入る
    DENSE_RANK() OVER(ORDER BY machine.machine_no, machine.machine_name) AS list_group_id,
    CONCAT_WS('|',lplan.long_plan_id,machine.machine_id,com.management_standards_component_id,con.management_standards_content_id,schedule.maintainance_schedule_id) AS key_id

    , '1' AS output_report_location_name_got_flg                -- 機能場所名称情報取得済フラグ（帳票用）
    , '1' AS output_report_job_name_got_flg                     -- 職種・機種名称情報取得済フラグ（帳票用）

FROM
    ln_long_plan lplan 
    INNER JOIN mc_management_standards_content con 
        ON con.long_plan_id = lplan.long_plan_id 
    INNER JOIN mc_management_standards_component com 
        ON com.management_standards_component_id = con.management_standards_component_id 
    INNER JOIN mc_machine machine 
        ON machine.machine_id = com.machine_id
    INNER JOIN #temp temp
        ON machine.machine_id = temp.Key1
    LEFT JOIN schedule 
        ON schedule.management_standards_content_id = con.management_standards_content_id
ORDER BY
    machine.machine_no,                                         -- 機器番号
    machine.machine_name,                                       -- 機器名称
--    lplan.long_plan_id,											-- 長計件名ID
--    machine.machine_id,											-- 機番ID
--    com.management_standards_component_id,
--    con.management_standards_content_id,
--    schedule.maintainance_schedule_id
    
     inspection_site_structure_id ,inspection_site_importance_structure_id
    ,inspection_site_conservation_structure_id ,inspection_content_structure_id
