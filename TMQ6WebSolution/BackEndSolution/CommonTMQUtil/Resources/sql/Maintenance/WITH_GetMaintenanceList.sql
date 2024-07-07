WITH
-- 保全活動絞り込み、およびあらかじめ取得する値を設定
narrow_summary AS(
    SELECT
        *
        -- 故障情報フラグ
        ,CASE
            WHEN activity_division = 2 THEN 1
            ELSE 0
        END AS is_failure
    FROM
        ma_summary
    -- 詳細条件
    WHERE
    /*@GetList
        EXISTS(
            SELECT
                *
            FROM
                #temp_location temp
            WHERE
                location_structure_id = temp.structure_id
        )
    AND EXISTS(
            SELECT
                *
            FROM
                #temp_job temp
            WHERE
                job_structure_id = temp.structure_id
        )
    @GetList*/
    /*@GetDetail
    summary_id = @SummaryId
    @GetDetail*/
)
-- 保全活動に関連テーブルをJOIN
,join_summary AS(
    SELECT
        --発生日
        pl.occurrence_date
        --完了日
        ,su.completion_date
        --件名
        ,su.subject
        --MQ分類
        ,su.mq_class_structure_id
        --地区～設備
        ,su.location_structure_id
        --地区ID
        ,su.location_district_structure_id AS district_id
        --工場ID
        ,su.location_factory_structure_id AS factory_id
        --プラントID
        ,su.location_plant_structure_id AS plant_id
        --系列ID
        ,su.location_series_structure_id AS series_id
        --工程ID
        ,su.location_stroke_structure_id AS stroke_id
        --設備ID
        ,su.location_facility_structure_id AS facility_id
        --職種
        ,su.job_structure_id
        --系停止
        ,su.stop_system_structure_id
        --系停止時間(Hr)
        ,su.stop_time
        --系停止時間(Hr)(表示用)
        ,FORMAT(su.stop_time, '0.##') AS stop_time_disp
        --費用メモ
        ,hi.cost_note
        --突発区分
        ,su.sudden_division_structure_id
        --着工予定日
        ,pl.expected_construction_date
        /*@FileLinkFailure
        --故障原因分析書
        ,CASE
            WHEN get_factory.extension_data = '1' THEN
            --dbo.get_file_download_info(1690, hf.history_failure_id) 
            REPLACE((
                    SELECT
                        dbo.get_file_download_info_row(att_temp.file_name, att_temp.attachment_id, att_temp.function_type_id, att_temp.key_id, att_temp.extension_data)
                    FROM
                        #temp_attachment as att_temp
                    WHERE
                        hf.history_failure_id = att_temp.key_id
                    AND att_temp.function_type_id = 1690
                    ORDER BY
                        document_no FOR xml path('')
                ), ' ', '')
            ELSE
            --dbo.get_file_download_info(1670, hf.history_failure_id) 
            REPLACE((
                    SELECT
                        dbo.get_file_download_info_row(att_temp.file_name, att_temp.attachment_id, att_temp.function_type_id, att_temp.key_id, att_temp.extension_data)
                    FROM
                        #temp_attachment as att_temp
                    WHERE
                        hf.history_failure_id = att_temp.key_id
                    AND att_temp.function_type_id = 1670
                    ORDER BY
                        document_no FOR xml path('')
                ), ' ', '')
        END AS file_link_failure
        @FileLinkFailure*/
        --予算管理区分
        ,su.budget_management_structure_id
        --予算性格区分
        ,su.budget_personality_structure_id
        --予算金額(k円)
        ,pl.total_budget_cost
        --予算金額(k円)(表示用)
        ,FORMAT(pl.total_budget_cost, '#,###') AS total_budget_cost_disp
        --保全時期
        ,hi.maintenance_season_structure_id
        --依頼担当
        ,COALESCE(mu_request.display_name, re.request_personnel_name) AS request_personnel_name
        --施工担当者
        ,COALESCE(mu.display_name, hi.construction_personnel_name) AS construction_personnel_name
        --作業時間(Hr)
        ,hi.total_working_time
        --作業時間(Hr)(表示用)
        ,FORMAT(hi.total_working_time, '0.##') AS total_working_time_disp
        --自係(Hr)
        ,hi.working_time_self
        --自係(Hr)(表示用)
        ,FORMAT(hi.working_time_self, '0.##') AS working_time_self_disp
        --発見方法
        ,re.discovery_methods_structure_id
        --実績結果
        ,hi.actual_result_structure_id
        --施工会社
        ,hi.construction_company
        --カウント件数
        ,su.maintenance_count
        --作業計画・実施内容
        ,su.plan_implementation_content
        --件名メモ
        ,su.subject_note
        /*@FileLinkSubject
        --件名添付有無(dbo.get_file_download_info(1650, su.summary_id))
        ,REPLACE((
                SELECT
                    dbo.get_file_download_info_row(att_temp.file_name, att_temp.attachment_id, att_temp.function_type_id, att_temp.key_id, att_temp.extension_data)
                FROM
                    #temp_attachment as att_temp
                WHERE
                    su.summary_id = att_temp.key_id
                AND att_temp.function_type_id = 1650
                ORDER BY
                    document_no FOR xml path('')
            ), ' ', '') AS file_link_subject
        @FileLinkSubject*/
        -- 保全部位(翻訳) 故障のみ表示
        ,CASE
            WHEN su.is_failure = 1 THEN hf.maintenance_site
            ELSE NULL
        END AS maintenance_site_name
        -- 保全内容(翻訳) 故障のみ表示
        ,CASE
            WHEN su.is_failure = 1 THEN hf.maintenance_content
            ELSE NULL
        END AS maintenance_content_name
        --実績金額(k円)
        ,hi.expenditure
        --実績金額(k円)(表示用)
        ,FORMAT(hi.expenditure, '#,###') AS expenditure_disp
        --現象
        ,hf.phenomenon_structure_id
        --現象補足
        ,hf.phenomenon_note
        --原因
        ,hf.failure_cause_structure_id
        --原因補足
        ,hf.failure_cause_note
        --原因性格1、原因性格2
        ,hf.failure_cause_personality_structure_id
        --原因性格1
        ,dbo.get_target_layer_id(hf.failure_cause_personality_structure_id, 0) AS failure_cause_personality1_structure_id
        --原因性格2
        ,dbo.get_target_layer_id(hf.failure_cause_personality_structure_id, 1) AS failure_cause_personality2_structure_id
        --性格補足
        ,hf.failure_cause_personality_note
        --処置対策
        ,hf.treatment_measure_structure_id
        --故障原因
        ,hf.failure_cause_addition_note
        --故障状況
        ,hf.failure_status
        --故障前の保全実施状況
        ,hf.previous_situation
        --復旧措置
        ,hf.recovery_action
        --改善対策
        ,hf.improvement_measure
        --保全システムのフィードバック
        ,hf.system_feed_back
        --教訓
        ,hf.lesson
        --特記（メモ）
        ,hf.failure_note
        --フォロー有無
        ,hi.follow_flg
        --フォロー予定年月 故障のみ表示
        ,CASE
            WHEN su.is_failure = 1 THEN hf.follow_plan_date
            ELSE NULL
        END AS follow_plan_date
        --フォロー内容 故障のみ表示
        ,CASE
            WHEN su.is_failure = 1 THEN hf.follow_content
            ELSE NULL
        END AS follow_content
        --依頼No.
        ,re.request_no
        --進捗状況
        --,CASE
        --    WHEN su.completion_date IS NOT NULL THEN '1'
        --    WHEN hi.construction_personnel_id IS NOT NULL THEN '2'
        --    ELSE '3'
        --END AS progress_no
        ,ex.structure_id AS progress_id
        --保全活動件名ID(非表示)
        ,su.summary_id
        ,re.issue_date
        --呼出回数
        ,hi.call_count
    FROM
        narrow_summary su
        LEFT JOIN
            ma_request re
        ON  su.summary_id = re.summary_id
        LEFT JOIN
            ma_plan pl
        ON  su.summary_id = pl.summary_id
        LEFT JOIN
            ma_history hi
        ON  su.summary_id = hi.summary_id
        LEFT JOIN
            ma_history_failure hf
        ON  hi.history_id = hf.history_id
        LEFT JOIN
            ms_user mu
        ON  hi.construction_personnel_id = mu.user_id
        LEFT JOIN
            ms_user mu_request
        ON  re.request_personnel_id = mu_request.user_id
        LEFT JOIN
            #get_factory get_factory
        ON  get_factory.structure_id = su.location_factory_structure_id
        LEFT JOIN
            #temp_progress ex
        ON  ex.extension_data = (
            CASE
                WHEN su.completion_date IS NOT NULL THEN '1'
                WHEN hi.construction_personnel_id IS NOT NULL THEN '2'
                ELSE '3'
            END
        )
)