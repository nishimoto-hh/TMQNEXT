WITH structure_factory AS (
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1180,1220) 
        AND language_id = @LanguageId
) ,
-- 保全スケジュールを機器別管理基準内容IDごとに取得(同じ値なら最大の開始日のレコード)
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
SELECT row_number() over(order by mcp.inspection_site_structure_id,msc.inspection_content_structure_id ) as row_no,
	   msc.management_standards_content_id AS key_id,  -- 機器別管理基準部位ID(スケジュール紐づけキー)
       lp.subject,                                   -- 件名
       mcp.management_standards_component_id,        -- 機器別管理基準部位ID
       mcp.machine_id,                               -- 機番ID
	   ma.equipment_level_structure_id,              -- 機器レベル
	   ma.machine_no,                                -- 機器番号
	   ma.machine_name,                              -- 機器名称
	   eq.maintainance_kind_manage,                  -- 点検種別毎管理
	   mcp.inspection_site_structure_id,             -- 部位ID
		( 
		SELECT
			tra.translation_text 
		FROM
			v_structure_item_all AS tra 
		WHERE
			tra.language_id = @LanguageId
			AND tra.location_structure_id = ( 
				SELECT
					MAX(st_f.factory_id) 
				FROM
					structure_factory AS st_f 
				WHERE
					st_f.structure_id = mcp.inspection_site_structure_id
					AND st_f.factory_id IN (0, ma.location_factory_structure_id)
			) 
			AND tra.structure_id = mcp.inspection_site_structure_id
		) AS inspection_site_name, -- 部位
	   msc.inspection_site_importance_structure_id,  -- 部位重要度ID
	   msc.inspection_site_conservation_structure_id,-- 部位保全方式ID
	   mcp.is_management_standard_conponent,         -- 機器別管理基準フラグ
	   msc.management_standards_content_id,          -- 機器別管理基準内容ID
	   msc.inspection_content_structure_id,          -- 点検内容ID
		( 
		SELECT
			tra.translation_text 
		FROM
			v_structure_item_all AS tra 
		WHERE
			tra.language_id = @LanguageId
			AND tra.location_structure_id = ( 
				SELECT
					MAX(st_f.factory_id) 
				FROM
					structure_factory AS st_f 
				WHERE
					st_f.structure_id = msc.inspection_content_structure_id
					AND st_f.factory_id IN (0, ma.location_factory_structure_id)
			) 
			AND tra.structure_id = msc.inspection_content_structure_id
		) AS inspection_content_name, -- 点検内容
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
	   END update_datetime                          -- 更新日付
       --dbo.get_file_link(1620,msc.management_standards_content_id) AS file_link_machine -- 添付ファイル
FROM mc_management_standards_component mcp -- 機器別管理基準部位
    ,mc_management_standards_content msc   -- 機器別管理基準内容
    ,schedule_content ms, -- 保全スケジュール
	 mc_machine ma,
	 mc_equipment eq,
	 ln_long_plan lp
WHERE mcp.management_standards_component_id = msc.management_standards_component_id
AND msc.management_standards_content_id = ms.management_standards_content_id
AND mcp.machine_id = ma.machine_id
AND ma.machine_id = eq.machine_id
AND mcp.is_management_standard_conponent = 1    -- 機器別管理基準フラグ
AND mcp.machine_id = @MachineId
AND msc.long_plan_id = lp.long_plan_id
AND msc.long_plan_id IS NOT NULL
ORDER BY mcp.inspection_site_structure_id,msc.inspection_content_structure_id -- 並び順
 
