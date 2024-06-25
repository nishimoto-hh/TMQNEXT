WITH schedule_date_by_machine as ( 
    -- 機IDに対するスケジュール情報を取得
    select
	    sch.maintainance_schedule_id
        , cont.management_standards_content_id
        , sch.start_date
        , det.complition
        , det.schedule_date 
    from
        mc_management_standards_component comp 
        left join mc_management_standards_content cont 
            on comp.management_standards_component_id = cont.management_standards_component_id 
        left join mc_maintainance_schedule sch 
            on cont.management_standards_content_id = sch.management_standards_content_id 
        left join mc_maintainance_schedule_detail det 
            on sch.maintainance_schedule_id = det.maintainance_schedule_id 
) 
, max_schedule_date_by_content as ( 
    -- 保全スケジュールID・内容ごとの保全活動が完了したデータの最大のスケジュール日を取得
    select
	    maintainance_schedule_id
        , management_standards_content_id
        , max(schedule_date) schedule_date 
    from
        schedule_date_by_machine 
    where
        complition = 1 
    group by
        maintainance_schedule_id, management_standards_content_id
) 
, next_date_exists_comp as ( 
    -- 保全スケジュールID・内容ごとの保全活動が完了したデータの最大のスケジュール日の次のスケジュール日を取得
    select
	    schedule_date_by_machine.maintainance_schedule_id
        , schedule_date_by_machine.management_standards_content_id
        , min(schedule_date_by_machine.schedule_date) schedule_date
    from
        schedule_date_by_machine 
        inner join max_schedule_date_by_content 
            on schedule_date_by_machine.maintainance_schedule_id = max_schedule_date_by_content.maintainance_schedule_id
			and schedule_date_by_machine.management_standards_content_id = max_schedule_date_by_content.management_standards_content_id
    where
        schedule_date_by_machine.schedule_date > max_schedule_date_by_content.schedule_date
	group by
	    schedule_date_by_machine.maintainance_schedule_id, schedule_date_by_machine.management_standards_content_id
) 
, next_date_not_exists_comp as ( 
    -- 保全スケジュールID・内容ごとの開始日より後のスケジュール日を取得(保全活動が完了していない)
    select
	    schedule_date_by_machine.maintainance_schedule_id
        , schedule_date_by_machine.management_standards_content_id
        , min(schedule_date_by_machine.schedule_date) schedule_date 
    from
        schedule_date_by_machine 
        left join ( 
            select
			    maintainance_schedule_id
                , management_standards_content_id
                , max(start_date) start_date 
            from
                schedule_date_by_machine 
            group by
                maintainance_schedule_id, management_standards_content_id
        ) max_start_date 
            on schedule_date_by_machine.maintainance_schedule_id = max_start_date.maintainance_schedule_id
			and schedule_date_by_machine.management_standards_content_id = max_start_date.management_standards_content_id
    where
        schedule_date_by_machine.start_date >= max_start_date.start_date 
    group by
        schedule_date_by_machine.maintainance_schedule_id, schedule_date_by_machine.management_standards_content_id
),
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
, main as(
SELECT 
       ma.factoryId,
       ma.job_structure_id,
       ma.job_kind_structure_id AS job_id,
       ma.job_large_classfication_structure_id AS large_classfication_id,
       ma.job_middle_classfication_structure_id AS middle_classfication_id,
       ma.job_small_classfication_structure_id AS small_classfication_id,
	   ma.location_structure_id,
	   ma.location_district_structure_id AS district_id,
	   ma.location_factory_structure_id AS factory_id,
	   ma.location_plant_structure_id AS plant_id,
	   ma.location_series_structure_id AS series_id,
	   ma.location_stroke_structure_id AS stroke_id,
	   ma.location_facility_structure_id AS facility_id,
       mcp.management_standards_component_id,        -- 機器別管理基準部位ID
       mcp.machine_id,                               -- 機番ID
	   ma.equipment_level_structure_id,              -- 機器レベル
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
					   #temp_structure_factory AS st_f 
				   WHERE
					   st_f.structure_id = ma.equipment_level_structure_id
					   AND st_f.factory_id IN (0, factoryId)
			   ) 
			   AND tra.structure_id = ma.equipment_level_structure_id
		) AS equipment_level_name, -- 機器レベル
	   ma.machine_no,                                -- 機器番号
	   ma.machine_name,                              -- 機器名称
	   ma.machine_no + ' ' + ma.machine_name AS machine,
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
					   #temp_structure_factory AS st_f 
				   WHERE
					   st_f.structure_id = mcp.inspection_site_structure_id
					   AND st_f.factory_id IN (0, factoryId)
			   ) 
			   AND tra.structure_id = mcp.inspection_site_structure_id
		) AS inspection_site_name, -- 部位
	   msc.inspection_site_importance_structure_id,  -- 部位重要度ID
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
					   #temp_structure_factory AS st_f 
				   WHERE
					   st_f.structure_id = msc.inspection_site_importance_structure_id
					   AND st_f.factory_id IN (0, factoryId)
			   ) 
			   AND tra.structure_id = msc.inspection_site_importance_structure_id
		) AS inspection_site_importance_name, -- 部位重要度
	   msc.inspection_site_conservation_structure_id,-- 部位保全方式ID
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
					   #temp_structure_factory AS st_f 
				   WHERE
					   st_f.structure_id = msc.inspection_site_conservation_structure_id
					   AND st_f.factory_id IN (0, factoryId)
			   ) 
			   AND tra.structure_id = msc.inspection_site_conservation_structure_id
		) AS inspection_site_conservation_name, -- 部位保全方式
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
					   #temp_structure_factory AS st_f 
				   WHERE
					   st_f.structure_id = msc.inspection_content_structure_id
					   AND st_f.factory_id IN (0, factoryId)
			   ) 
			   AND tra.structure_id = msc.inspection_content_structure_id
		) AS inspection_content_name, -- 点検内容
	   msc.maintainance_division,                    -- 保全区分
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
					#temp_structure_factory AS st_f 
				   WHERE
					   st_f.structure_id = msc.maintainance_division
					   AND st_f.factory_id IN (0, factoryId)
			   ) 
			   AND tra.structure_id = msc.maintainance_division
		) AS maintainance_division_name, -- 保全区分
	   msc.maintainance_kind_structure_id,           -- 点検種別ID
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
					#temp_structure_factory AS st_f 
				   WHERE
					   st_f.structure_id = msc.maintainance_kind_structure_id
					   AND st_f.factory_id IN (0, factoryId)
			   ) 
			   AND tra.structure_id = msc.maintainance_kind_structure_id
		) AS maintainance_kind_name, -- 点検種別
	   msc.budget_amount,                            -- 予算金額
	   msc.preparation_period,                       -- 準備期間(日)
	   msc.order_no,                                 -- 並び順
	   msc.long_plan_id,                             -- 長期計画件名ID
	   msc.schedule_type_structure_id,               -- スケジュール管理区分
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
					#temp_structure_factory AS st_f 
				WHERE
					st_f.structure_id = msc.schedule_type_structure_id
					AND st_f.factory_id IN (0, factoryId)
			) 
			AND tra.structure_id = msc.schedule_type_structure_id
		) AS schedule_type_name, -- スケジュール管理区分
	   ms.maintainance_schedule_id,                  -- 保全スケジュールID
	   ms.is_cyclic,                                 -- 周期ありフラグ
	   ms.cycle_year,                                -- 周期(年)
	   ms.cycle_month,                               -- 周期(月)
	   ms.cycle_day,                                 -- 周期(日)
	   ms.disp_cycle,                                -- 表示周期
	   ms.start_date,                                -- 開始日
		-- 場所階層の翻訳
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
					   #temp_structure_factory AS st_f 
				   WHERE
					   st_f.structure_id = ma.location_district_structure_id
					   AND st_f.factory_id IN (0, factoryId)
			   ) 
			   AND tra.structure_id = ma.location_district_structure_id
		) AS district_name,
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
					   #temp_structure_factory AS st_f 
				   WHERE
					   st_f.structure_id = ma.location_factory_structure_id
					   AND st_f.factory_id IN (0, factoryId)
			   ) 
			   AND tra.structure_id = ma.location_factory_structure_id
		) AS factory_name,
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
					   #temp_structure_factory AS st_f 
				   WHERE
					   st_f.structure_id = ma.location_plant_structure_id
					   AND st_f.factory_id IN (0, factoryId)
			   ) 
			   AND tra.structure_id = ma.location_plant_structure_id
		) AS plant_name,
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
					   #temp_structure_factory AS st_f 
				   WHERE
					   st_f.structure_id = ma.location_series_structure_id
					   AND st_f.factory_id IN (0, factoryId)
			   ) 
			   AND tra.structure_id = ma.location_series_structure_id
		) AS series_name,
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
					   #temp_structure_factory AS st_f 
				   WHERE
					   st_f.structure_id = ma.location_stroke_structure_id
					   AND st_f.factory_id IN (0, factoryId)
			   ) 
			   AND tra.structure_id = ma.location_stroke_structure_id
		) AS stroke_name,
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
					   #temp_structure_factory AS st_f 
				   WHERE
					   st_f.structure_id = ma.location_facility_structure_id
					   AND st_f.factory_id IN (0, factoryId)
			   ) 
			   AND tra.structure_id = ma.location_facility_structure_id
		) AS facility_name,
		-- 職種機種階層の翻訳
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
					   #temp_structure_factory AS st_f 
				   WHERE
					   st_f.structure_id = ma.job_kind_structure_id
					   AND st_f.factory_id IN (0, factoryId)
			   ) 
			   AND tra.structure_id = ma.job_kind_structure_id
		) AS job_name,
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
					   #temp_structure_factory AS st_f 
				   WHERE
					   st_f.structure_id = ma.job_large_classfication_structure_id
					   AND st_f.factory_id IN (0, factoryId)
			   ) 
			   AND tra.structure_id = ma.job_large_classfication_structure_id
		) AS large_classfication_name,
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
					   #temp_structure_factory AS st_f 
				   WHERE
					   st_f.structure_id = ma.job_middle_classfication_structure_id
					   AND st_f.factory_id IN (0, factoryId)
			   ) 
			   AND tra.structure_id = ma.job_middle_classfication_structure_id
		) AS middle_classfication_name,
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
					   #temp_structure_factory AS st_f 
				   WHERE
					   st_f.structure_id = ma.job_small_classfication_structure_id
					   AND st_f.factory_id IN (0, factoryId)
			   ) 
			   AND tra.structure_id = ma.job_small_classfication_structure_id
		) AS small_classfication_name,
    ( 
          select
              top(1) v.structure_id 
          from
              ( 
                  select
                      * 
                  from
                      v_structure_item_all 
                  where
                      structure_group_id = 2130 
                      and language_id = @languageid 
                      and location_structure_id in (0, factoryid)
              ) v
              , ms_item_extension mi 
          where
              v.structure_item_id = mi.item_id 
              and mi.sequence_no = 1 
              and mi.extension_data = '1'
    ) as is_update_schedule,
    ( 
          select
              top(1) v.translation_text 
          from
              ( 
                  select
                      * 
                  from
                      v_structure_item_all 
                  where
                      structure_group_id = 2130 
                      and language_id = @languageid 
                      and location_structure_id in (0, factoryid)
              ) v
              , ms_item_extension mi 
          where
              v.structure_item_id = mi.item_id 
              and mi.sequence_no = 1 
              and mi.extension_data = '1'
    ) as is_update_schedule_name
	,mcp.remarks -- 備考
FROM mc_management_standards_component mcp -- 機器別管理基準部位
    ,mc_management_standards_content msc   -- 機器別管理基準内容
    ,schedule_content ms, -- 保全スケジュール
	 (SELECT mc.*,location_factory_structure_id as factoryId FROM mc_machine mc)  ma,
	 mc_equipment eq
WHERE mcp.management_standards_component_id = msc.management_standards_component_id
AND msc.management_standards_content_id = ms.management_standards_content_id
AND mcp.machine_id = ma.machine_id
AND ma.machine_id = eq.machine_id
AND mcp.is_management_standard_conponent = 1    -- 機器別管理基準フラグ
--AND mcp.machine_id = @MachineId
AND EXISTS(SELECT * FROM #temp_structure_selected temp WHERE temp.structure_group_id = 1000 AND ma.location_structure_id = temp.structure_id)
AND EXISTS(SELECT * FROM #temp_structure_selected temp WHERE temp.structure_group_id = 1010 AND  ma.job_structure_id = temp.structure_id)
)