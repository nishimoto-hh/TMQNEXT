SELECT
    cont.long_plan_id                                         -- 長計件名ID
    , machine.machine_id                                      -- 機番ID
    , comp.management_standards_component_id                  -- 機器別管理基準部位ID
    , cont.management_standards_content_id                    -- 機器別管理基準内容ID
    , schedule.maintainance_schedule_id                       -- 保全スケジュールID
    , machine.machine_no                                      -- 機器番号
    , machine.machine_name                                    -- 機器名称
    , schedule.cycle_year                                     -- 周期(年)
    , schedule.cycle_month                                    -- 周期(月)
    , schedule.cycle_day                   　                 -- 周期(日)
    , format(schedule.start_date, 'yyyy/MM/dd') AS start_date -- 開始日
    , dense_rank() OVER ( 
        ORDER BY
            machine.machine_no
            , machine.machine_name
    ) AS list_group_id                                        -- グループ(折り畳み単位)毎の連番、同一グループに同じ値が入る
    , concat_ws( 
        '|'
        , ln.long_plan_id
        , machine.machine_id
        , comp.management_standards_component_id
        , cont.management_standards_content_id
        , schedule.maintainance_schedule_id
    ) AS key_id
    , '1' AS output_report_location_name_got_flg              -- 機能場所名称情報取得済フラグ（帳票用）
    , '1' AS output_report_job_name_got_flg                   -- 職種・機種名称情報取得済フラグ（帳票用）
    ---------- 以下は翻訳を取得 ----------
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageid 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = comp.inspection_site_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = comp.inspection_site_structure_id
    ) AS site -- 部位
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageid 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = cont.inspection_site_importance_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = cont.inspection_site_importance_structure_id
    ) AS site_importance -- 部位重要度
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageid 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = cont.inspection_site_conservation_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = cont.inspection_site_conservation_structure_id
    ) AS conservation -- 保全方式
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageid 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = cont.inspection_content_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = cont.inspection_content_structure_id
    ) AS inspection_content_name -- 内容
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageid 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = ln.budget_personality_structure_id 
                    AND st_f.factory_id IN (0, ln.location_factory_structure_id)
            ) 
            AND tra.structure_id = ln.budget_personality_structure_id
    ) AS budget_personality -- 予算性格区分
FROM
    mc_management_standards_component comp -- 機器別管理基準部位
    LEFT JOIN mc_management_standards_content cont -- 機器別管理基準内容
        ON comp.management_standards_component_id = cont.management_standards_component_id 
    LEFT JOIN (SELECT a.*
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
	 ) schedule -- 保全スケジュール
        ON cont.management_standards_content_id = schedule.management_standards_content_id
    INNER JOIN mc_machine machine -- 機番情報
        ON comp.machine_id = machine.machine_id 
    INNER JOIN #temp temp -- 出力対象の機番IDが格納されている一時テーブル
        ON machine.machine_id = temp.key1 
    LEFT JOIN ln_long_plan ln -- 長計件名
        ON cont.long_plan_id = ln.long_plan_id 
ORDER BY
    machine.machine_no
    , machine.machine_name
    , comp.inspection_site_structure_id
    , cont.inspection_site_importance_structure_id
    , cont.inspection_site_conservation_structure_id
    , cont.inspection_content_structure_id