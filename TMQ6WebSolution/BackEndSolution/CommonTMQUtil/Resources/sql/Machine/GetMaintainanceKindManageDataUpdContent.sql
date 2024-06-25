WITH -- 保全スケジュールを機器別管理基準内容IDごとに取得(同じ値なら最大の開始日のレコード)
schedule_start_date AS(
    SELECT
	    sc.maintainance_schedule_id,
        sc.management_standards_content_id,
        sc.cycle_year,
        sc.cycle_month,
        sc.cycle_day,
        sc.start_date,
		sc.is_cyclic,
		sc.disp_cycle,
		sc.update_datetime
    FROM
        mc_maintainance_schedule AS sc
    WHERE
        NOT EXISTS(
            SELECT
                *
            FROM
                mc_maintainance_schedule AS sub
            WHERE
                sc.management_standards_content_id = sub.management_standards_content_id
            AND sc.start_date < sub.start_date
        )
),
-- 上で取得した保全スケジュールを機器別管理基準内容ID、開始日ごとに取得(同じ値なら最大の更新日時のレコード)
schedule_content AS(
    SELECT
	    main.maintainance_schedule_id,
        main.management_standards_content_id,
        main.cycle_year,
        main.cycle_month,
        main.cycle_day,
        main.start_date,
		main.is_cyclic,
		main.disp_cycle,
		main.update_datetime
    FROM
        schedule_start_date AS main
    WHERE
        NOT EXISTS(
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
       mcp.management_standards_component_id,        -- 機器別管理基準部位ID
       mcp.machine_id,                               -- 機番ID
	   mcp.inspection_site_structure_id,             -- 部位ID
	   msc.inspection_site_importance_structure_id,  -- 部位重要度ID
	   msc.inspection_site_conservation_structure_id,-- 部位保全方式ID
	   mcp.is_management_standard_conponent,         -- 機器別管理基準フラグ
	   msc.management_standards_content_id,          -- 機器別管理基準内容ID
	   msc.inspection_content_structure_id,          -- 点検内容ID
	   msc.maintainance_division,                    -- 保全区分
	   msc.maintainance_kind_structure_id,           -- 点検種別ID
	   msc.budget_amount,                            -- 予算金額
	   msc.preparation_period,                       -- 準備期間(日)
	   msc.order_no,                                 -- 並び順
	   msc.long_plan_id,                             -- 長期計画件名ID
	   msc.schedule_type_structure_id,               -- スケジュール管理区分
	   ms.maintainance_schedule_id,                  -- 保全スケジュールID
	   ms.is_cyclic,                                 -- 周期ありフラグ
	   ms.cycle_year,                                -- 周期(年)
	   ms.cycle_month,                               -- 周期(月)
	   ms.cycle_day,                                 -- 周期(日)
	   ms.disp_cycle,                                -- 表示周期
	   ms.start_date                                 -- 開始日
FROM mc_management_standards_component mcp -- 機器別管理基準部位
    ,mc_management_standards_content msc   -- 機器別管理基準内容
    ,schedule_content ms, -- 保全スケジュール
	 mc_machine ma
WHERE mcp.management_standards_component_id = msc.management_standards_component_id
AND msc.management_standards_content_id = ms.management_standards_content_id
AND mcp.machine_id = ma.machine_id
AND mcp.is_management_standard_conponent = 1    -- 機器別管理基準フラグ
AND mcp.machine_id = @MachineId
AND msc.maintainance_kind_structure_id = @MaintainanceKindStructureId    -- 同一点検種別
AND msc.management_standards_content_id <> @ManagementStandardsContentId
--AND (ISNULL(ms.cycle_year,0) <> ISNULL(@CycleYear,0) OR ISNULL(ms.cycle_month,0) <> ISNULL(@CycleMonth,0) OR ISNULL(ms.cycle_day,0) <> ISNULL(@CycleDay,0) OR ms.start_date <> @StartDate)
--AND (ms.cycle_year <> @CycleYear OR ms.cycle_month <> @CycleMonth OR ms.cycle_day <> @CycleDay OR ms.start_date <> @StartDate)
