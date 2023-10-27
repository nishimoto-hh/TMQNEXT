WITH get_factory AS ( 
    --故障分析個別工場フラグ
    SELECT
        ms.structure_id
        , mie.extension_data 
    FROM
        ms_structure ms 
        LEFT JOIN ms_item mi 
            ON ms.structure_item_id = mi.item_id 
        LEFT JOIN ms_item_extension mie 
            ON mi.item_id = mie.item_id 
    WHERE
        ms.structure_group_id = 1000            --場所階層
        AND ms.structure_layer_no = 1           --工場
        AND mie.sequence_no = 2                 --故障分析個別工場フラグ
) 
, summary_list AS ( 
    SELECT
        pl.occurrence_date                      --発生日
        , su.completion_date                    --完了日
        , su.subject                            --件名
        , su.mq_class_structure_id              --MQ分類
        , CASE 
            WHEN su.activity_division = 1 
                THEN ma.machine_no 
            ELSE maf.machine_no 
            END AS machine_no                   --機器番号
        , CASE 
            WHEN su.activity_division = 1 
                THEN ma.machine_name 
            ELSE maf.machine_name 
            END AS machine_name                 --機器名称
        , su.location_structure_id              --地区～設備
        , CASE 
            WHEN su.activity_division = 1 
                THEN COALESCE(ma.job_structure_id, su.job_structure_id) 
            ELSE COALESCE(maf.job_structure_id, su.job_structure_id) 
            END AS job_structure_id             --職種～機種小分類
        , su.stop_system_structure_id           --系停止
        , su.stop_time                          --系停止時間(Hr)
        , FORMAT(su.stop_time, '0.##') AS stop_time_disp --系停止時間(Hr)(表示用)
        , hi.cost_note                          --費用メモ
        , su.sudden_division_structure_id       --突発区分
        , pl.expected_construction_date         --着工予定日
        , CASE 
            WHEN get_factory.extension_data = '1' 
                THEN dbo.get_file_download_info(1690, hf.history_failure_id) 
            ELSE dbo.get_file_download_info(1670, hf.history_failure_id) 
            END AS file_link_failure            --故障原因分析書
        , su.budget_management_structure_id     --予算管理区分
        , su.budget_personality_structure_id    --予算性格区分
        , pl.total_budget_cost                  --予算金額(k円)
        , FORMAT(pl.total_budget_cost, '#,###') AS total_budget_cost_disp --予算金額(k円)(表示用)
        , hi.maintenance_season_structure_id    --保全時期
        , COALESCE( 
            mu_request.display_name
            , re.request_personnel_name
        ) AS request_personnel_name             --依頼担当
        , COALESCE(mu.display_name, hi.construction_personnel_name) AS construction_personnel_name --施工担当者
        , hi.total_working_time                 --作業時間(Hr)
        , FORMAT(hi.total_working_time, '0.##') AS total_working_time_disp --作業時間(Hr)(表示用)
        , hi.working_time_self                  --自係(Hr)
        , FORMAT(hi.working_time_self, '0.##') AS working_time_self_disp --自係(Hr)(表示用)
        , re.discovery_methods_structure_id     --発見方法
        , hi.actual_result_structure_id         --実績結果
        , hi.construction_company               --施工会社
        , su.maintenance_count                  --カウント件数
        , su.plan_implementation_content        --作業計画・実施内容
        , su.subject_note                       --件名メモ
        , dbo.get_file_download_info(1650, su.summary_id) AS file_link_subject --件名添付有無
        , CASE 
            WHEN su.activity_division = 1 
                THEN his.inspection_site_structure_id 
            ELSE NULL 
            END AS inspection_site_structure_id -- 保全部位ID（点検情報）
        , CASE 
            WHEN su.activity_division = 1 
                THEN NULL
            ELSE hf.maintenance_site 
            END AS maintenance_site             --保全部位（故障情報）
        , CASE 
            WHEN su.activity_division = 1 
                THEN hic.inspection_content_structure_id 
            ELSE NULL 
            END AS inspection_content_structure_id -- 保全内容ID（点検情報）
        , CASE 
            WHEN su.activity_division = 1 
                THEN NULL
            ELSE hf.maintenance_content 
            END AS maintenance_content          --保全内容（故障情報）
        , hi.expenditure                        --実績金額(k円)
        , FORMAT(hi.expenditure, '#,###') AS expenditure_disp --実績金額(k円)(表示用)
        , hf.phenomenon_structure_id            --現象
        , hf.phenomenon_note                    --現象補足
        , hf.failure_cause_structure_id         --原因
        , hf.failure_cause_note                 --原因補足
        , hf.failure_cause_personality_structure_id --原因性格1、原因性格2
        , hf.failure_cause_personality_note     --性格補足
        , hf.treatment_measure_structure_id     --処置対策
        , hf.failure_cause_addition_note        --故障原因
        , hf.failure_status                     --故障状況
        , hf.previous_situation                 --故障前の保全実施状況
        , hf.recovery_action                    --復旧措置
        , hf.improvement_measure                --改善対策
        , hf.system_feed_back                   --保全システムのフィードバック
        , hf.lesson                             --教訓
        , hf.failure_note                       --特記（メモ）
        , CASE 
            WHEN su.activity_division = 1 
                THEN COALESCE(hic.follow_flg, 0)
            ELSE COALESCE(hf.follow_flg, 0)
            END AS follow_flg                   --フォロー有無
        , CASE 
            WHEN su.activity_division = 1 
                THEN hic.follow_plan_date 
            ELSE hf.follow_plan_date 
            END AS follow_plan_date             --フォロー予定年月
        , CASE 
            WHEN su.activity_division = 1 
                THEN hic.follow_content 
            ELSE hf.follow_content 
            END AS follow_content               --フォロー内容
        , re.request_no                         --依頼No.
        , CASE 
            WHEN su.completion_date IS NOT NULL 
                THEN 1 
            WHEN hi.construction_personnel_id IS NOT NULL 
                THEN 2 
            ELSE 3 
            END AS progress_no                  --進捗状況
        , su.summary_id                         --保全活動件名ID(非表示)
    FROM
        ma_summary su 
        LEFT JOIN ma_request re 
            ON su.summary_id = re.summary_id 
        LEFT JOIN ma_plan pl 
            ON su.summary_id = pl.summary_id 
        LEFT JOIN ma_history hi 
            ON su.summary_id = hi.summary_id 
        LEFT JOIN ma_history_machine hm 
            ON hi.history_id = hm.history_id 
        LEFT JOIN ma_history_inspection_site his 
            ON hm.history_machine_id = his.history_machine_id 
        LEFT JOIN ma_history_inspection_content hic 
            ON his.history_inspection_site_id = hic.history_inspection_site_id 
        LEFT JOIN mc_machine ma 
            ON hm.machine_id = ma.machine_id 
        LEFT JOIN ma_history_failure hf 
            ON hi.history_id = hf.history_id 
        LEFT JOIN mc_machine maf 
            ON hf.machine_id = maf.machine_id 
        LEFT JOIN ms_user mu 
            ON hi.construction_personnel_id = mu.user_id 
        LEFT JOIN ms_user mu_request 
            ON re.request_personnel_id = mu_request.user_id 
        LEFT JOIN get_factory 
            ON get_factory.structure_id = dbo.get_target_layer_id(su.location_structure_id, 1)
) 
, item_ex AS ( 
    --進捗状況
    SELECT
        ms.structure_id
        , mie.extension_data 
    FROM
        ms_structure ms 
        LEFT JOIN ms_item mi 
            ON ms.structure_item_id = mi.item_id 
        LEFT JOIN ms_item_extension mie 
            ON mi.item_id = mie.item_id 
    WHERE
        ms.structure_group_id = 1900 
        AND mie.sequence_no = 1 
) 
, translate_target AS ( 
    -- 翻訳対象
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
        , summary.inspection_site_structure_id  --保全部位ID（点検情報）
        , summary.maintenance_site              --保全部位（故障情報）
        , summary.inspection_content_structure_id --保全内容ID（点検情報）
        , summary.maintenance_content           --保全内容（故障情報）
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
        , dbo.get_target_layer_id(summary.location_structure_id, 1) AS factoryId --このレコードの工場ID
    FROM
        summary_list summary 
        LEFT JOIN item_ex ex 
            ON CAST(summary.progress_no AS varchar) = ex.extension_data
) 
, structure_factory AS ( 
    -- 使用する構成グループの構成IDを絞込、工場の指定に用いる
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN ( 
            1850
            , 1130
            , 1400
            , 1300
            , 1060
            , 1330
            , 1080
            , 1140
            , 1180
            , 1220
            , 1510
            , 1810
            , 1050
            , 1900
        ) 
        AND language_id = @LanguageId
) 