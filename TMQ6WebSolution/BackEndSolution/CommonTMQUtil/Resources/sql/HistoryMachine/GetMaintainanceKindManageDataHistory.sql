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
    ,(SELECT a.*
	 		FROM mc_maintainance_schedule AS a -- 保全スケジュール
	 		INNER JOIN
	 		-- 機器別管理基準内容IDごとの開始日最新データを取得
	 		(SELECT management_standards_content_id,
	 				MAX(start_date) AS start_date,
					MAX(update_datetime) AS update_datetime
	 		 FROM mc_maintainance_schedule
	 		 GROUP BY management_standards_content_id
	 		) b
	 		ON a.management_standards_content_id = b.management_standards_content_id
	 		AND (a.start_date = b.start_date 
	 			 OR a.start_date IS NULL AND b.start_date IS NULL --null結合を考慮
	 			)
	 		AND (a.update_datetime = b.update_datetime 
	 			 OR a.update_datetime IS NULL AND b.update_datetime IS NULL --null結合を考慮
	 			)
	 ) ms, -- 保全スケジュール
	 mc_machine ma
WHERE mcp.management_standards_component_id = msc.management_standards_component_id
AND msc.management_standards_content_id = ms.management_standards_content_id
AND mcp.machine_id = ma.machine_id
AND mcp.is_management_standard_conponent = 1    -- 機器別管理基準フラグ
AND mcp.machine_id = @MachineId
AND msc.maintainance_kind_structure_id = @MaintainanceKindStructureId    -- 同一点検種別
AND (ISNULL(ms.cycle_year,0) <> ISNULL(@CycleYear,0) OR ISNULL(ms.cycle_month,0) <> ISNULL(@CycleMonth,0) OR ISNULL(ms.cycle_day,0) <> ISNULL(@CycleDay,0) OR ms.start_date <> @StartDate)