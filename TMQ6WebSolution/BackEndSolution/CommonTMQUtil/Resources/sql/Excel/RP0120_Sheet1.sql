WITH summary_list AS ( 
    SELECT
        pl.occurrence_date                      --発生日
        , su.completion_date                    --完了日
        , su.subject                            --件名
        , su.mq_class_structure_id              --MQ分類
        , CASE WHEN su.activity_division = 2 THEN mc.machine_no ELSE NULL END AS machine_no -- 機器番号 故障のみ表示
        , CASE WHEN su.activity_division = 2 THEN mc.machine_name ELSE NULL END AS machine_name -- 機器名称 故障のみ表示
        , su.location_structure_id              --地区～設備
        , su.job_structure_id                   --職種
        , su.location_district_structure_id AS district_id      -- 機能場所階層id（地区）
        , su.location_factory_structure_id AS factory_id        -- 機能場所階層id（工場）
        , su.location_plant_structure_id AS plant_id            -- 機能場所階層id（プラント）
        , su.location_series_structure_id AS series_id          -- 機能場所階層id（系列）
        , su.location_stroke_structure_id AS stroke_id          -- 機能場所階層id（工程）
        , su.location_facility_structure_id AS facility_id      -- 機能場所階層id（設備）
        , su.stop_system_structure_id           --系停止
        , su.stop_time                          --系停止時間(Hr)
        , hi.cost_note                          --費用メモ
        , su.sudden_division_structure_id       --突発区分
        , pl.expected_construction_date         --着工予定日
        , su.budget_management_structure_id     --予算管理区分
        , su.budget_personality_structure_id    --予算性格区分
        , pl.total_budget_cost                  --予算金額(k円)
        , hi.maintenance_season_structure_id    --保全時期
        , coalesce( 
            mu_request.display_name
            , re.request_personnel_name
        ) AS request_personnel_name             --依頼担当
        , coalesce(mu.display_name, hi.construction_personnel_name) AS construction_personnel_name --施工担当者
        , hi.total_working_time                 --作業時間(Hr)
        , hi.working_time_self                  --自係(Hr)
        , re.discovery_methods_structure_id     --発見方法
        , hi.actual_result_structure_id         --実績結果
        , hi.construction_company               --施工会社
        , su.maintenance_count                  --カウント件数
        , su.plan_implementation_content        --作業計画・実施内容
        , su.subject_note                       --件名メモ
        , CASE WHEN su.activity_division = 2 THEN hf.maintenance_site ELSE NULL END AS inspection_site_name -- 保全部位(翻訳) 故障のみ表示
        , CASE WHEN su.activity_division = 2 THEN hf.maintenance_content ELSE NULL END AS inspection_content_name -- 保全内容(翻訳) 故障のみ表示
        , hi.expenditure                        --実績金額(k円)
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
        , hi.follow_flg                         --フォロー有無
        , CASE WHEN su.activity_division = 2 THEN hf.follow_plan_date ELSE NULL END AS follow_plan_date --フォロー予定年月 故障のみ表示
        , CASE WHEN su.activity_division = 2 THEN hf.follow_content ELSE NULL END AS follow_content --フォロー内容 故障のみ表示
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
        LEFT JOIN ma_history_failure hf 
            ON hi.history_id = hf.history_id 
        LEFT JOIN ms_user mu 
            ON hi.construction_personnel_id = mu.user_id 
        LEFT JOIN ms_user mu_request 
            ON re.request_personnel_id = mu_request.user_id 
        LEFT JOIN mc_machine mc
            ON hf.machine_id = mc.machine_id

) 
, TraFinish AS (-- 「完了済」の翻訳を取得
    SELECT
        tra.translation_text
    FROM
        ms_translation tra
    WHERE
        tra.location_structure_id = 0
    AND tra.translation_id = 131060003
    AND tra.language_id = (SELECT DISTINCT languageId FROM #temp)
)
, TraRece AS (-- 「保全受付」の翻訳を取得
    SELECT
        tra.translation_text
    FROM
        ms_translation tra
    WHERE
        tra.location_structure_id = 0
    AND tra.translation_id = 131300002
    AND tra.language_id = (SELECT DISTINCT languageId FROM #temp)
)
, TraCreated AS (-- 「作成済」の翻訳を取得
    SELECT
        tra.translation_text
    FROM
        ms_translation tra
    WHERE
        tra.location_structure_id = 0
    AND tra.translation_id = 131110001
    AND tra.language_id = (SELECT DISTINCT languageId FROM #temp)
)
, DateFormat AS (--「yyyy/MM」の翻訳を取得
    SELECT
        tra.translation_text
    FROM
        ms_translation tra
    WHERE
        tra.location_structure_id = 0
    AND tra.translation_id = 150000002
    AND tra.language_id = (SELECT DISTINCT languageId FROM #temp)
)


SELECT * FROM (
SELECT 
    summary.occurrence_date                     --発生日
    , summary.completion_date                   --完了日
    , summary.subject                           --件名
    , (
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = summary.mq_class_structure_id
                    AND st_f.factory_id IN (0, summary.factory_id)
            )
            AND tra.structure_id = summary.mq_class_structure_id
      ) AS mq_class_name                        --MQ分類
    , summary.machine_no -- 機器番号
    , summary.machine_name -- 機器名称
    , summary.location_structure_id             --地区～設備
    , summary.job_structure_id                  --職種～機種小分類
    --地区(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
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
            tra.language_id = temp.languageId
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
            tra.language_id = temp.languageId
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
            tra.language_id = temp.languageId
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
            tra.language_id = temp.languageId
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
            tra.language_id = temp.languageId
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
            tra.language_id = temp.languageId
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
    , (
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = summary.stop_system_structure_id
                    AND st_f.factory_id IN (0, summary.factory_id)
            )
            AND tra.structure_id = summary.stop_system_structure_id
      ) AS stop_system_name                     --系停止
    , summary.stop_time                         --系停止時間(Hr)
    , summary.cost_note                         --費用メモ
    , (
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = summary.sudden_division_structure_id
                    AND st_f.factory_id IN (0, summary.factory_id)
            )
            AND tra.structure_id = summary.sudden_division_structure_id
      ) AS sudden_division_name                 --突発区分
    , summary.expected_construction_date        --着工予定日
    , (
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = summary.budget_management_structure_id
                    AND st_f.factory_id IN (0, summary.factory_id)
            )
            AND tra.structure_id = summary.budget_management_structure_id
      ) AS budget_management_name               --予算管理区分
    , (
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = summary.budget_personality_structure_id
                    AND st_f.factory_id IN (0, summary.factory_id)
            )
            AND tra.structure_id = summary.budget_personality_structure_id
      ) AS budget_personality_name              --予算性格区分
    , summary.total_budget_cost                 --予算金額(k円)
    , (
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = summary.maintenance_season_structure_id
                    AND st_f.factory_id IN (0, summary.factory_id)
            )
            AND tra.structure_id = summary.maintenance_season_structure_id
      ) AS maintenance_season_name              --保全時期
    , summary.request_personnel_name            --依頼担当
    , summary.construction_personnel_name       --施工担当者
    , summary.total_working_time                --作業時間(Hr)
    , summary.working_time_self                 --自係(Hr)
    , (
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = summary.discovery_methods_structure_id
                    AND st_f.factory_id IN (0, summary.factory_id)
            )
            AND tra.structure_id = summary.discovery_methods_structure_id
      ) AS discovery_methods_name               --発見方法
    , (
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = summary.actual_result_structure_id
                    AND st_f.factory_id IN (0, summary.factory_id)
            )
            AND tra.structure_id = summary.actual_result_structure_id
      ) AS actual_result_name                        ---実績結果
    , summary.construction_company              --施工会社
    , summary.maintenance_count                 --カウント件数
    , summary.plan_implementation_content       --作業計画・実施内容
    , summary.subject_note                      --件名メモ
    , summary.inspection_site_name              --保全部位
    , summary.inspection_content_name           --保全内容
    , summary.expenditure                       --実績金額(k円)
    , (
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = summary.phenomenon_structure_id
                    AND st_f.factory_id IN (0, summary.factory_id)
            )
            AND tra.structure_id = summary.phenomenon_structure_id
      ) AS phenomenon_name                      --現象
    , summary.phenomenon_note                   --現象補足
    , (
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = summary.failure_cause_structure_id
                    AND st_f.factory_id IN (0, summary.factory_id)
            )
            AND tra.structure_id = summary.failure_cause_structure_id
      ) AS failure_cause_name                   --原因
    , summary.failure_cause_note                --原因補足
    , [dbo].[get_failure_cause_personality](summary.failure_cause_personality_structure_id, 1, summary.factory_id, temp.languageId) AS failure_cause_personality_name1    --原因性格1
    , [dbo].[get_failure_cause_personality](summary.failure_cause_personality_structure_id, 2, summary.factory_id, temp.languageId) AS failure_cause_personality_name2    --原因性格2
    , summary.failure_cause_personality_note    --性格補足
    , (
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = summary.treatment_measure_structure_id
                    AND st_f.factory_id IN (0, summary.factory_id)
            )
            AND tra.structure_id = summary.treatment_measure_structure_id
      ) AS treatment_measure_name               --処置対策
    , summary.failure_cause_addition_note       --故障原因
    , summary.failure_status                    --故障状況
    , summary.previous_situation                --故障前の保全実施状況
    , summary.recovery_action                   --復旧措置
    , summary.improvement_measure               --改善対策
    , summary.system_feed_back                  --保全システムのフィードバック
    , summary.lesson                            --教訓
    , summary.failure_note                      --特記（メモ）
    , CASE ISNULL(summary.follow_flg, 0)
          WHEN 1 THEN '○'
          ELSE ''
      END AS follow_flg                         --フォロー有無
    , FORMAT(summary.follow_plan_date, DateFormat.translation_text) AS follow_plan_date  --フォロー予定年月
    , summary.follow_content                    --フォロー内容
    , summary.request_no                        --依頼No.
    , CASE progress_no
          WHEN 1 THEN TraFinish.translation_text -- 完了済
          WHEN 2 THEN TraRece.translation_text   -- 保全受付
          ELSE TraCreated.translation_text       -- 作成済
      END AS progress_name                      --進捗状況
    , summary.summary_id                        --保全活動件名ID(非表示)
    , '1' AS output_report_location_name_got_flg       -- 機能場所名称情報取得済フラグ（帳票用）
    , '1' AS output_report_job_name_got_flg            -- 職種・機種名称情報取得済フラグ（帳票用）
FROM
    summary_list summary 
    INNER JOIN #temp temp
    ON summary.summary_id = temp.Key1
    CROSS JOIN
       TraFinish --「完了済」の翻訳
    CROSS JOIN
       TraRece -- 「保全受付」の翻訳
    CROSS JOIN
       TraCreated --「作成済」の翻訳
    CROSS JOIN
       DateFormat -- 「yyyy/MM」の翻訳
) tbl
ORDER BY
occurrence_date desc
,summary_id desc
