SELECT COUNT(*)
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
AND msc.management_standards_content_id <> @ManagementStandardsContentId -- 自分自身以外
AND msc.maintainance_kind_structure_id = @MaintainanceKindStructureId    -- 同一点検種別
AND (ms.cycle_year <> @CycleYear OR ms.cycle_month <> @CycleMonth OR ms.cycle_day <> @CycleDay OR ms.start_date <> @StartDate)
