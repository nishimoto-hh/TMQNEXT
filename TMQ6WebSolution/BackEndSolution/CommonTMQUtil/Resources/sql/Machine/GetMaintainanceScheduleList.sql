WITH structure_factory AS (
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1170,1200,1180,1030,1230,1220,1240, 1890) 
        AND language_id = @LanguageId
) 
, schedule_date_by_machine as ( 
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
    where
        comp.machine_id = @MachineId
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
)
, main as(
SELECT mcp.management_standards_component_id,        -- 機器別管理基準部位ID
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
					structure_factory AS st_f 
				WHERE
					st_f.structure_id = ma.equipment_level_structure_id
					AND st_f.factory_id IN (0, ma.location_factory_structure_id)
			) 
			AND tra.structure_id = ma.equipment_level_structure_id 
		) AS equipment_level_name, -- 機器レベル
	   ma.machine_no,                                -- 機器番号
	   ma.machine_name,                              -- 機器名称
	   ma.importance_structure_id,                   -- 機器重要度
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
					st_f.structure_id = ma.importance_structure_id
					AND st_f.factory_id IN (0, ma.location_factory_structure_id)
			) 
			AND tra.structure_id = ma.importance_structure_id
		) AS importance_name, -- 機器重要度
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
					st_f.structure_id = msc.inspection_site_importance_structure_id
					AND st_f.factory_id IN (0, ma.location_factory_structure_id)
			) 
			AND tra.structure_id = msc.inspection_site_importance_structure_id
		) AS inspection_site_importance_nam, -- 部位重要度
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
					structure_factory AS st_f 
				WHERE
					st_f.structure_id = msc.inspection_site_conservation_structure_id
					AND st_f.factory_id IN (0, ma.location_factory_structure_id)
			) 
			AND tra.structure_id = msc.inspection_site_conservation_structure_id
		) AS inspection_site_conservation_n, -- 部位保全方式
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
					st_f.structure_id = msc.maintainance_division
					AND st_f.factory_id IN (0, ma.location_factory_structure_id)
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
					structure_factory AS st_f 
				WHERE
					st_f.structure_id = msc.maintainance_kind_structure_id
					AND st_f.factory_id IN (0, ma.location_factory_structure_id)
			) 
			AND tra.structure_id = msc.maintainance_kind_structure_id
		) AS maintainance_kind_name, -- 点検種別
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
					st_f.structure_id = msc.schedule_type_structure_id
					AND st_f.factory_id IN (0, ma.location_factory_structure_id)
			) 
			AND tra.structure_id = msc.schedule_type_structure_id
		) AS schedule_type_name, -- スケジュール管理
	   msc.budget_amount,                            -- 予算金額
	   msc.preparation_period,                       -- 準備期間(日)
	   msc.order_no,                                 -- 並び順
	   msc.long_plan_id,                             -- 長期計画件名ID
	   msc.schedule_type_structure_id,               -- スケジュール管理区分
	   ms.maintainance_schedule_id,                  -- 保全スケジュールID
	   ms.is_cyclic,                                 -- 周期ありフラグ
	   ms.cycle_year,                                -- 周期(年)
	   ms.cycle_year as cycle_year_disp,             -- 周期(年)ラベル用(変更前)
	   ms.cycle_month,                               -- 周期(月)
	   ms.cycle_month as cycle_month_disp,           -- 周期(月)ラベル用(変更前)
	   ms.cycle_day,                                 -- 周期(日)
	   ms.cycle_day as cycle_day_disp,               -- 周期(日)ラベル用(変更前)
	   ms.disp_cycle,                                -- 表示周期
	   ms.disp_cycle as disp_cycle_disp,             -- 表示周期ラベル用(変更前)
	   ms.start_date,                                -- 開始日
	   ms.start_date as start_date_disp,             -- 開始日ラベル用(変更前)
	   -- 3テーブルのうち最大更新日付を取得
	   CASE WHEN mcp.update_datetime > msc.update_datetime AND mcp.update_datetime > ms.update_datetime THEN mcp.update_datetime
	        WHEN msc.update_datetime > mcp.update_datetime AND msc.update_datetime > ms.update_datetime THEN msc.update_datetime
			ELSE ms.update_datetime
	   END update_datetime,                          -- 機番ID
       -- dbo.get_file_link(1620,msc.management_standards_content_id) AS file_link_machine, -- 添付ファイル	 
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
	   msc.management_standards_content_id AS key_id,  -- 機器別管理基準内容ID(スケジュール紐づけキー)
	   '1' AS is_update_schedule, -- スケジュールを更新する
	   '1' AS is_update_schedule_disp -- スケジュールを更新する ラベル用(変更前)
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
AND (msc.maintainance_division <> (SELECT  TOP(1) it.structure_id
	  FROM ms_structure it,
	       ms_item_extension ex
	  WHERE it.structure_item_id = ex.item_id
	  AND it.structure_group_id = 1230
	  AND ex.extension_data IN ('3')-- 保全区分が日常点検は非表示
	 ) -- 構成マスタ
	 OR msc.maintainance_division IS NULL)
AND mcp.is_management_standard_conponent = 1    -- 機器別管理基準フラグ
AND mcp.machine_id = @MachineId
)

select
    main.*
	-- 以下は次回実施予定日
	-- ●(保全活動が完了したデータ)が存在する場合は、最新の●の次の○のデータの日付
	-- ●が存在しない場合は開始日より後の○の日付
    , coalesce( 
        next_date_exists_comp.schedule_date
        , next_date_not_exists_comp.schedule_date
    ) next_schedule_date 
    , coalesce( 
        next_date_exists_comp.schedule_date
        , next_date_not_exists_comp.schedule_date
    ) next_schedule_date_disp 
from
    main 
    left join next_date_exists_comp 
        on main.maintainance_schedule_id = next_date_exists_comp.maintainance_schedule_id
		and main.management_standards_content_id = next_date_exists_comp.management_standards_content_id
    left join next_date_not_exists_comp 
        on main.maintainance_schedule_id = next_date_not_exists_comp.maintainance_schedule_id
		and main.management_standards_content_id = next_date_not_exists_comp.management_standards_content_id 

ORDER BY inspection_site_structure_id,inspection_content_structure_id,maintainance_kind_structure_id
 
