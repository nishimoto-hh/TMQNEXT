--WITH_GetMaintenanceList.sqlの最後に開始の括弧有り
    ) target_list
) --WITH句target_dataの終了

-- 件名単位に集約する
SELECT DISTINCT
    occurrence_date                             --発生日
    , completion_date                           --完了日
    , subject                                   --件名
    , mq_class_structure_id                     --MQ分類
    , STUFF( 
        ( 
            SELECT DISTINCT
                ',' + machine_no 
            FROM
                summary_list ts 
            WHERE
                ts.summary_id = summary.summary_id FOR XML PATH ('')
        ) 
        , 1
        , 1
        , ''
    ) AS machine_no                             --機器番号
    , STUFF( 
        ( 
            SELECT DISTINCT
                ',' + machine_name 
            FROM
                summary_list ts 
            WHERE
                ts.summary_id = summary.summary_id FOR XML PATH ('')
        ) 
        , 1
        , 1
        , ''
    ) AS machine_name                           --機器名称
    , location_structure_id                     --地区～設備
    , STUFF( 
        ( 
            SELECT DISTINCT
                ',' + cast(job_structure_id AS nvarchar(MAX)) 
            FROM
                summary_list ts 
            WHERE
                ts.summary_id = summary.summary_id FOR XML PATH ('')
        ) 
        , 1
        , 1
        , ''
    ) AS job_structure_id                       --職種～機種小分類
    , stop_system_structure_id                  --系停止
    , stop_time                                 --系停止時間(Hr)
    , stop_time_disp                            --系停止時間(Hr)(表示用)
    , cost_note                                 --費用メモ
    , sudden_division_structure_id              --突発区分
    , expected_construction_date                --着工予定日
    , file_link_failure                         --故障原因分析書
    , budget_management_structure_id            --予算管理区分
    , budget_personality_structure_id           --予算性格区分
    , total_budget_cost                         --予算金額(k円)
    , total_budget_cost_disp                    --予算金額(k円)(表示用)
    , maintenance_season_structure_id           --保全時期
    , request_personnel_name                    --依頼担当
    , construction_personnel_name               --施工担当者
    , total_working_time                        --作業時間(Hr)
    , total_working_time_disp                   --作業時間(Hr)(表示用)
    , working_time_self                         --自係(Hr)
    , working_time_self_disp                    --自係(Hr)(表示用)
    , discovery_methods_structure_id            --発見方法
    , actual_result_structure_id                --実績結果
    , construction_company                      --施工会社
    , maintenance_count                         --カウント件数
    , plan_implementation_content               --作業計画・実施内容
    , subject_note                              --件名メモ
    , file_link_subject                         --件名添付有無
    , expenditure                               --実績金額(k円)
    , expenditure_disp                          --実績金額(k円)(表示用)
    , phenomenon_structure_id                   --現象
    , phenomenon_note                           --現象補足
    , failure_cause_structure_id                --原因
    , failure_cause_note                        --原因補足
    , failure_cause_personality_structure_id    --原因性格1、原因性格2
    , failure_cause_personality_note            --性格補足
    , treatment_measure_structure_id            --処置対策
    , failure_cause_addition_note               --故障原因
    , failure_status                            --故障状況
    , previous_situation                        --故障前の保全実施状況
    , recovery_action                           --復旧措置
    , improvement_measure                       --改善対策
    , system_feed_back                          --保全システムのフィードバック
    , lesson                                    --教訓
    , failure_note                              --特記（メモ）
    , ( 
        SELECT
            CASE 
                WHEN COUNT(follow_flg) > 0 
                    THEN 1 
                ELSE 0 
                END 
        FROM
            summary_list ts 
        WHERE
            ts.summary_id = summary.summary_id 
            AND follow_flg = 1
    ) AS follow_flg                             --フォロー有無
    , STUFF( 
        ( 
            SELECT DISTINCT
                ',' + FORMAT(follow_plan_date, 'yyyy/MM') 
            FROM
                summary_list ts 
            WHERE
                ts.summary_id = summary.summary_id FOR XML PATH ('')
        ) 
        , 1
        , 1
        , ''
    ) AS follow_plan_date_disp                  --フォロー予定年月
    , STUFF( 
        ( 
            SELECT DISTINCT
                ',' + follow_content 
            FROM
                summary_list ts 
            WHERE
                ts.summary_id = summary.summary_id FOR XML PATH ('')
        ) 
        , 1
        , 1
        , ''
    ) AS follow_content                         --フォロー内容
    , request_no                                --依頼No.
    , summary.summary_id                        --保全活動件名ID(非表示)
    , ex.structure_id AS progress_id            --進捗状況
    , factory_id                                 --このレコードの工場ID
    , STUFF( 
        ( 
            SELECT DISTINCT
                ',' + maintenance_site_name
            FROM
                summary_list ts 
            WHERE
                ts.summary_id = summary.summary_id FOR XML PATH ('')
        ) 
        , 1
        , 1
        , ''
    ) AS maintenance_site_name                  --保全部位(翻訳)
    , STUFF( 
        ( 
            SELECT DISTINCT
                ',' + maintenance_content_name  
            FROM
                summary_list ts 
            WHERE
                ts.summary_id = summary.summary_id FOR XML PATH ('')
        ) 
        , 1
        , 1
        , ''
    ) AS maintenance_content_name               --保全内容(翻訳)
    , ( 
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
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = summary.mq_class_structure_id 
                    AND st_f.factory_id IN (0, summary.factory_id)
            ) 
            AND tra.structure_id = summary.mq_class_structure_id
    ) AS mq_class_name                          --MQ分類(翻訳)
    , ( 
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
                    st_f.structure_id = summary.stop_system_structure_id 
                    AND st_f.factory_id IN (0, summary.factory_id)
            ) 
            AND tra.structure_id = summary.stop_system_structure_id
    ) AS stop_system_name                       --系停止(翻訳)
    , ( 
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
                    st_f.structure_id = summary.sudden_division_structure_id 
                    AND st_f.factory_id IN (0, summary.factory_id)
            ) 
            AND tra.structure_id = summary.sudden_division_structure_id
    ) AS sudden_division_name                   --突発区分(翻訳)
    , ( 
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
                    st_f.structure_id = summary.budget_management_structure_id 
                    AND st_f.factory_id IN (0, summary.factory_id)
            ) 
            AND tra.structure_id = summary.budget_management_structure_id
    ) AS budget_management_name                 --予算管理区分(翻訳)
    , ( 
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
                    st_f.structure_id = summary.budget_personality_structure_id 
                    AND st_f.factory_id IN (0, summary.factory_id)
            ) 
            AND tra.structure_id = summary.budget_personality_structure_id
    ) AS budget_personality_name                --予算性格区分(翻訳)
    , ( 
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
                    st_f.structure_id = summary.maintenance_season_structure_id 
                    AND st_f.factory_id IN (0, summary.factory_id)
            ) 
            AND tra.structure_id = summary.maintenance_season_structure_id
    ) AS maintenance_season_name                --保全時期(翻訳)
    , ( 
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
                    st_f.structure_id = summary.discovery_methods_structure_id 
                    AND st_f.factory_id IN (0, summary.factory_id)
            ) 
            AND tra.structure_id = summary.discovery_methods_structure_id
    ) AS discovery_methods_name                 --発見方法(翻訳)
    , ( 
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
                    st_f.structure_id = summary.actual_result_structure_id 
                    AND st_f.factory_id IN (0, summary.factory_id)
            ) 
            AND tra.structure_id = summary.actual_result_structure_id
    ) AS actual_result_name                     --実績結果(翻訳)
    , ( 
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
                    st_f.structure_id = summary.phenomenon_structure_id 
                    AND st_f.factory_id IN (0, summary.factory_id)
            ) 
            AND tra.structure_id = summary.phenomenon_structure_id
    ) AS phenomenon_name                        --現象(翻訳)
    , ( 
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
                    st_f.structure_id = summary.failure_cause_structure_id 
                    AND st_f.factory_id IN (0, summary.factory_id)
            ) 
            AND tra.structure_id = summary.failure_cause_structure_id
    ) AS failure_cause_name                     --原因(翻訳)
    , ( 
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
                    st_f.structure_id = summary.treatment_measure_structure_id 
                    AND st_f.factory_id IN (0, summary.factory_id)
            ) 
            AND tra.structure_id = summary.treatment_measure_structure_id
    ) AS treatment_measure_name                 --処置対策(翻訳)
    , ( 
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
                    st_f.structure_id = ex.structure_id 
                    AND st_f.factory_id IN (0, summary.factory_id)
            ) 
            AND tra.structure_id = ex.structure_id
    ) AS progress_name                          --進捗状況(翻訳)
FROM
    summary_list summary 
    INNER JOIN target_data target 
        ON summary.summary_id = target.summary_id
    LEFT JOIN item_ex ex 
        ON CAST(summary.progress_no AS varchar) = ex.extension_data