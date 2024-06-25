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
FROM mc_management_standards_component mcp -- 機器別管理基準部位
    ,mc_management_standards_content msc   -- 機器別管理基準内容
    ,schedule_content ms, -- 保全スケジュール
	 mc_machine ma
WHERE mcp.management_standards_component_id = msc.management_standards_component_id
AND msc.management_standards_content_id = ms.management_standards_content_id
AND mcp.machine_id = ma.machine_id
AND (msc.maintainance_division <> (SELECT TOP(1) it.structure_id
	  FROM ms_structure it,
	       ms_item_extension ex
	  WHERE it.structure_item_id = ex.item_id
	  AND it.structure_group_id = 1230
	  AND ex.extension_data IN ('3')-- 保全区分が日常点検は非表示
	 ) -- 構成マスタ
	 OR msc.maintainance_division IS NULL)
AND mcp.is_management_standard_conponent = 1    -- 機器別管理基準フラグ
AND mcp.management_standards_component_id = @ManagementStandardsComponentId
AND msc.management_standards_content_id = @ManagementStandardsContentId
ORDER BY mcp.inspection_site_structure_id,msc.inspection_content_structure_id,msc.maintainance_kind_structure_id
 
