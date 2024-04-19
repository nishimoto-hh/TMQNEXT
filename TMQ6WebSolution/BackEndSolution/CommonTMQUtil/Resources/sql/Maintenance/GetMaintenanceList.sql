SELECT
    --発生日
    summary.occurrence_date
    --完了日
    ,summary.completion_date
    --件名
    ,summary.subject
    --MQ分類
    ,summary.mq_class_structure_id
    --地区～設備
    ,summary.location_structure_id
    --地区ID
    ,summary.district_id
    --工場ID
    ,summary.factory_id
    --プラントID
    ,summary.plant_id
    --系列ID
    ,summary.series_id
    --工程ID
    ,summary.stroke_id
    --設備ID
    ,summary.facility_id
    --職種
    ,summary.job_structure_id
    --系停止
    ,summary.stop_system_structure_id
    --系停止時間(Hr)
    ,summary.stop_time
    --系停止時間(Hr)(表示用)
    ,summary.stop_time_disp
    --費用メモ
    ,summary.cost_note
    --突発区分
    ,summary.sudden_division_structure_id
    --着工予定日
    ,summary.expected_construction_date
    --故障原因分析書
    ,summary.file_link_failure
    --予算管理区分
    ,summary.budget_management_structure_id
    --予算性格区分
    ,summary.budget_personality_structure_id
    --予算金額(k円)
    ,summary.total_budget_cost
    --予算金額(k円)(表示用)
    ,summary.total_budget_cost_disp
    --保全時期
    ,summary.maintenance_season_structure_id
    --依頼担当
    ,summary.request_personnel_name
    --施工担当者
    ,summary.construction_personnel_name
    --作業時間(Hr)
    ,summary.total_working_time
    --作業時間(Hr)(表示用)
    ,summary.total_working_time_disp
    --自係(Hr)
    ,summary.working_time_self
    --自係(Hr)(表示用)
    ,summary.working_time_self_disp
    --発見方法
    ,summary.discovery_methods_structure_id
    --実績結果
    ,summary.actual_result_structure_id
    --施工会社
    ,summary.construction_company
    --カウント件数
    ,summary.maintenance_count
    --作業計画・実施内容
    ,summary.plan_implementation_content
    --件名メモ
    ,summary.subject_note
    --件名添付有無
    ,summary.file_link_subject
    --実績金額(k円)
    ,summary.expenditure
    --実績金額(k円)(表示用)
    ,summary.expenditure_disp
    --現象
    ,summary.phenomenon_structure_id
    --現象補足
    ,summary.phenomenon_note
    --原因
    ,summary.failure_cause_structure_id
    --原因補足
    ,summary.failure_cause_note
    --原因性格1、原因性格2
    ,summary.failure_cause_personality_structure_id
    --原因性格1
    ,summary.failure_cause_personality1_structure_id
    --原因性格2
    ,summary.failure_cause_personality2_structure_id
    --性格補足
    ,summary.failure_cause_personality_note
    --処置対策
    ,summary.treatment_measure_structure_id
    --故障原因
    ,summary.failure_cause_addition_note
    --故障状況
    ,summary.failure_status
    --故障前の保全実施状況
    ,summary.previous_situation
    --復旧措置
    ,summary.recovery_action
    --改善対策
    ,summary.improvement_measure
    --保全システムのフィードバック
    ,summary.system_feed_back
    --教訓
    ,summary.lesson
    --特記（メモ）
    ,summary.failure_note
    --フォロー有無
    ,summary.follow_flg
    --フォロー内容
    ,summary.follow_content
    --依頼No.
    ,summary.request_no
    --保全活動件名ID(非表示)
    ,summary.summary_id
    --保全部位(翻訳)
    ,summary.maintenance_site_name
    --保全内容(翻訳)
    ,summary.maintenance_content_name
    --フォロー予定年月
    ,summary.follow_plan_date
    --フォロー予定年月
    ,FORMAT(summary.follow_plan_date, 'yyyy/MM') AS follow_plan_date_disp
    --進捗状況
    ,ex.structure_id AS progress_id
    --MQ分類(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                -- 工場IDまたは0で合致する最大の工場IDを取得→工場IDのレコードがなければ0となる
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = summary.mq_class_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.mq_class_structure_id
    ) AS mq_class_name
    --系停止(翻訳)
    ,(
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
                    st_f.structure_id = summary.stop_system_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.stop_system_structure_id
    ) AS stop_system_name
    --突発区分(翻訳)
    ,(
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
                    st_f.structure_id = summary.sudden_division_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.sudden_division_structure_id
    ) AS sudden_division_name
    --予算管理区分(翻訳)
    ,(
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
                    st_f.structure_id = summary.budget_management_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.budget_management_structure_id
    ) AS budget_management_name
    --予算性格区分(翻訳)
    ,(
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
                    st_f.structure_id = summary.budget_personality_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.budget_personality_structure_id
    ) AS budget_personality_name
    --保全時期(翻訳)
    ,(
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
                    st_f.structure_id = summary.maintenance_season_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.maintenance_season_structure_id
    ) AS maintenance_season_name
    --発見方法(翻訳)
    ,(
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
                    st_f.structure_id = summary.discovery_methods_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.discovery_methods_structure_id
    ) AS discovery_methods_name
    --実績結果(翻訳)
    ,(
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
                    st_f.structure_id = summary.actual_result_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.actual_result_structure_id
    ) AS actual_result_name
    --現象(翻訳)
    ,(
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
                    st_f.structure_id = summary.phenomenon_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.phenomenon_structure_id
    ) AS phenomenon_name
    --原因(翻訳)
    ,(
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
                    st_f.structure_id = summary.failure_cause_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.failure_cause_structure_id
    ) AS failure_cause_name
    --処置対策(翻訳)
    ,(
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
                    st_f.structure_id = summary.treatment_measure_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.treatment_measure_structure_id
    ) AS treatment_measure_name
    --進捗状況(翻訳)
    ,(
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
                    st_f.structure_id = ex.structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = ex.structure_id
    ) AS progress_name
    --地区(翻訳)
    ,(
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
                    st_f.structure_id = summary.district_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.district_id
    ) AS district_name
    --工場(翻訳)
    ,(
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
                    st_f.structure_id = summary.factory_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.factory_id
    ) AS factory_name
    --プラント(翻訳)
    ,(
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
                    st_f.structure_id = summary.plant_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.plant_id
    ) AS plant_name
    --系列(翻訳)
    ,(
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
                    st_f.structure_id = summary.series_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.series_id
    ) AS series_name
    --工程(翻訳)
    ,(
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
                    st_f.structure_id = summary.stroke_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.stroke_id
    ) AS stroke_name
    --設備(翻訳)
    ,(
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
                    st_f.structure_id = summary.facility_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.facility_id
    ) AS facility_name
    --職種(翻訳)
    ,(
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
                    st_f.structure_id = summary.job_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.job_structure_id
    ) AS job_name
    --原因性格1(翻訳)
    ,(
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
                    st_f.structure_id = summary.failure_cause_personality1_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.failure_cause_personality1_structure_id
    ) AS failure_cause_personality1_structure_name
    --原因性格2(翻訳)
    ,(
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
                    st_f.structure_id = summary.failure_cause_personality2_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.failure_cause_personality2_structure_id
    ) AS failure_cause_personality2_structure_name
    ,summary.issue_date--20230911 shiraishi add
    ,summary.call_count
FROM
    join_summary AS summary
    LEFT JOIN
        #temp_progress AS ex
    ON  summary.progress_no = ex.extension_data
