SELECT mcp.management_standards_component_id,        -- 機器別管理基準部位ID
       mcp.machine_id,                               -- 機番ID
	   ma.equipment_level_structure_id,              -- 機器レベル
	   ma.machine_no,                                -- 機器番号
	   ma.machine_name,                              -- 機器名称
	   ma.importance_structure_id,                   -- 機器重要度
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
	   ms.start_date,                                -- 開始日
	   -- 3テーブルのうち最大更新日付を取得
	   CASE WHEN mcp.update_datetime > msc.update_datetime AND mcp.update_datetime > ms.update_datetime THEN mcp.update_datetime
	        WHEN msc.update_datetime > mcp.update_datetime AND msc.update_datetime > ms.update_datetime THEN msc.update_datetime
			ELSE ms.update_datetime
	   END update_datetime,                          -- 機番ID
       dbo.get_file_link(1620,msc.management_standards_content_id) AS file_link_machine, -- 添付ファイル	 
	   REPLACE(
			(SELECT STR(md.maintainance_schedule_detail_id) + '|'
		     FROM mc_maintainance_schedule m,
			      mc_maintainance_schedule_detail md			      
			 WHERE m.maintainance_schedule_id = md.maintainance_schedule_id
			 AND m.management_standards_content_id = msc.management_standards_content_id
			 ORDER BY md.schedule_date,md.sequence_count
			 FOR XML PATH(''))
		,' ','') AS maintainance_schedule_detail_id,  -- 保全スケジュール詳細ID
	   REPLACE(
			(SELECT FORMAT(md.schedule_date,'yyyy/MM/dd') + '|'
		     FROM mc_maintainance_schedule m,
			      mc_maintainance_schedule_detail md			      
			 WHERE m.maintainance_schedule_id = md.maintainance_schedule_id
			 AND m.management_standards_content_id = msc.management_standards_content_id
			 ORDER BY md.schedule_date,md.sequence_count
			 FOR XML PATH(''))
		,' ','') AS schedule_date,                    -- スケジュール日
	   REPLACE(
			(SELECT STR(md.complition) + '|'
		     FROM mc_maintainance_schedule m,
			      mc_maintainance_schedule_detail md			      
			 WHERE m.maintainance_schedule_id = md.maintainance_schedule_id
			 AND m.management_standards_content_id = msc.management_standards_content_id
			 ORDER BY md.schedule_date,md.sequence_count
			 FOR XML PATH(''))
		,' ','') AS complition,  -- 完了フラグ
	   REPLACE(
			(SELECT STR(md.summary_id) + '|'
		     FROM mc_maintainance_schedule m,
			      mc_maintainance_schedule_detail md			      
			 WHERE m.maintainance_schedule_id = md.maintainance_schedule_id
			 AND m.management_standards_content_id = msc.management_standards_content_id
			 ORDER BY md.schedule_date,md.sequence_count
			 FOR XML PATH(''))
		,' ','') AS summary_id,　  -- 保全活動ID	   
		(SELECT MAX(md.update_datetime) 
		    FROM mc_maintainance_schedule m,
			    mc_maintainance_schedule_detail md			      
			WHERE m.maintainance_schedule_id = md.maintainance_schedule_id
			AND m.management_standards_content_id = msc.management_standards_content_id			
		) AS update_datetime_sch,　  -- 更新日付(スケジュール部分)
	   msc.management_standards_content_id AS key_id  -- 機器別管理基準内容ID(スケジュール紐づけキー)
	,DENSE_RANK() OVER(ORDER BY mcp.machine_id, msc.maintainance_kind_structure_id) AS group_key
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
AND (msc.maintainance_division <> (SELECT it.structure_id
	  FROM ms_structure it,
	       ms_item_extension ex
	  WHERE it.structure_item_id = ex.item_id
	  AND it.structure_group_id = 1230
	  AND ex.extension_data IN (3)-- 保全区分が日常点検は非表示
	 ) -- 構成マスタ
	 OR msc.maintainance_division IS NULL)
AND mcp.is_management_standard_conponent = 1    -- 機器別管理基準フラグ
AND mcp.management_standards_component_id = @ManagementStandardsComponentId
AND msc.management_standards_content_id = @ManagementStandardsContentId
ORDER BY msc.maintainance_kind_structure_id,mcp.inspection_site_structure_id,msc.inspection_content_structure_id
 
