SELECT DISTINCT
    summary.occurrence_date                 --発生日
    , summary.completion_date               --完了日
    , summary.subject                       --件名
    , summary.mq_class_structure_id         --MQ分類
    , summary.machine_no                    --機器番号
    , summary.machine_name                  --機器名称
    , summary.location_structure_id         --地区～設備
    , summary.job_structure_id              --職種～機種小分類
    , summary.stop_system_structure_id      --系停止
    , summary.stop_time                     --系停止時間(Hr)
    , summary.stop_time_disp                --系停止時間(Hr)(表示用)
    , summary.cost_note                     --費用メモ
    , summary.sudden_division_structure_id  --突発区分
    , summary.expected_construction_date    --着工予定日
    , summary.file_link_failure             --故障原因分析書
    , summary.budget_management_structure_id --予算管理区分
    , summary.budget_personality_structure_id --予算性格区分
    , summary.total_budget_cost             --予算金額(k円)
    , summary.total_budget_cost_disp        --予算金額(k円)(表示用)
    , summary.maintenance_season_structure_id --保全時期
    , summary.request_personnel_name        --依頼担当
    , summary.construction_personnel_name   --施工担当者
    , summary.total_working_time            --作業時間(Hr)
    , summary.total_working_time_disp       --作業時間(Hr)(表示用)
    , summary.working_time_self             --自係(Hr)
    , summary.working_time_self_disp        --自係(Hr)(表示用)
    , summary.discovery_methods_structure_id --発見方法
    , summary.actual_result_structure_id    --実績結果
    , summary.construction_company          --施工会社
    , summary.maintenance_count             --カウント件数
    , summary.plan_implementation_content   --作業計画・実施内容
    , summary.subject_note                  --件名メモ
    , summary.file_link_subject             --件名添付有無
    , summary.maintenance_site_name         --保全部位(翻訳)
    , summary.maintenance_content_name      --保全内容(翻訳)
    , summary.expenditure                   --実績金額(k円)
    , summary.expenditure_disp              --実績金額(k円)(表示用)
    , summary.phenomenon_structure_id       --現象
    , summary.phenomenon_note               --現象補足
    , summary.failure_cause_structure_id    --原因
    , summary.failure_cause_note            --原因補足
    , summary.failure_cause_personality_structure_id --原因性格1、原因性格2
    , summary.failure_cause_personality_note --性格補足
    , summary.treatment_measure_structure_id --処置対策
    , summary.failure_cause_addition_note   --故障原因
    , summary.failure_status                --故障状況
    , summary.previous_situation            --故障前の保全実施状況
    , summary.recovery_action               --復旧措置
    , summary.improvement_measure           --改善対策
    , summary.system_feed_back              --保全システムのフィードバック
    , summary.lesson                        --教訓
    , summary.failure_note                  --特記（メモ）
    , summary.follow_flg                    --フォロー有無
    , summary.follow_plan_date              --フォロー予定年月
    , summary.follow_content                --フォロー内容
    , summary.request_no                    --依頼No.
    , summary.summary_id                    --保全活動件名ID(非表示)
    , ex.structure_id AS progress_id        --進捗状況
FROM
    summary_list summary 
    LEFT JOIN item_ex ex 
        ON CAST(summary.progress_no AS varchar) = ex.extension_data