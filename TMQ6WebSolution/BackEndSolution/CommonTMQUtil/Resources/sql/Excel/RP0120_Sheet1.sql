WITH summary_list AS ( 
    SELECT
        su.activity_division                    --活動区分ID
        , pl.occurrence_date                    --発生日
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
        , re.issue_date                         -- 発行日
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
, inspection AS (-- 「点検情報」の翻訳を取得
   SELECT
       tra.translation_text
   FROM
       ms_translation tra
   WHERE
       tra.location_structure_id = 0
   AND tra.translation_id = 131190082
   AND tra.language_id = (SELECT DISTINCT languageId FROM #temp)
)
, failure AS ( 
    -- 「故障情報」の翻訳を取得
    SELECT
        tra.translation_text 
    FROM
        ms_translation tra 
    WHERE
        tra.location_structure_id = 0 
        AND tra.translation_id = 131100001 
        AND tra.language_id = (SELECT DISTINCT languageid FROM #temp)
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
    , summary.issue_date                        --発行日
    , CASE progress_no
          WHEN 1 THEN TraFinish.translation_text -- 完了済
          WHEN 2 THEN TraRece.translation_text   -- 保全受付
          ELSE TraCreated.translation_text       -- 作成済
      END AS progress_name                      --進捗状況
    , summary.summary_id                        --保全活動件名ID(非表示)
    , '1' AS output_report_location_name_got_flg       -- 機能場所名称情報取得済フラグ（帳票用）
    , '1' AS output_report_job_name_got_flg            -- 職種・機種名称情報取得済フラグ（帳票用）
    , CASE WHEN summary.activity_division = 2 THEN failure.translation_text ELSE inspection.translation_text END AS activity_division_name
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
    CROSS JOIN 
       failure --「故障情報」の翻訳
    CROSS JOIN 
       inspection --「点検情報」の翻訳
) tbl
ORDER BY
occurrence_date desc
,summary_id desc






--2024/06/25 準委任6月分の実装前に戻す為ここからコメント
--DECLARE @w_key1 bigint       -- 保全活動件名ID
--DECLARE @w_key2 nvarchar(2)  -- 言語ID
--DECLARE @w_key3 int          -- 本務工場

--CREATE TABLE #temp_rep(
---- ma_summary
--    summary_id int,                            -- 保全活動件名ID
--	completion_date date,                      -- 完了日
--	subject nvarchar(800),                     -- 件名
--	mq_class_name nvarchar(800),               -- MQ分類
--	district_name nvarchar(800),               -- 地区
--    factory_name nvarchar(800),                -- 工場
--    plant_name nvarchar(800),                  -- プラント
--    series_name nvarchar(800),                 -- 系列
--    stroke_name nvarchar(800),                 -- 工程
--    facility_name nvarchar(800),               -- 設備
--    job_name nvarchar(800),                    -- 職種
--    stop_system_name nvarchar(800),            -- 系停止
--    stop_time decimal(13,3),                   -- 系停止時間(Hr)
--    sudden_division_name nvarchar(800),        -- 突発区分
--    budget_management_name nvarchar(800),      -- 予算管理区分
--    budget_personality_name nvarchar(800),     -- 予算性格区分
--    maintenance_count int,                     -- カウント件数
--    plan_implementation_content nvarchar(800), -- 作業計画・実施内容
--    subject_note nvarchar(1200),               -- 件名メモ
--    activity_division_name nvarchar(800),      -- 故障・点検区分
--    --repair_cost_name nvarchar(800),            -- 修繕費分類 20240624出力対象外に変更
--    --change_management_name nvarchar(800),      -- 変更管理 20240624出力対象外に変更
--    --env_safety_management_name nvarchar(800),  -- 環境安全管理区分 20240624出力対象外に変更
--    --construction_date date,                    -- 着工日 20240624出力対象外に変更
--    --completion_time datetime,                  -- 完了時刻 20240624出力対象外に変更

---- ma_request
--    request_personnel_name nvarchar(800),            -- 依頼担当
--    discovery_methods_name nvarchar(800),            -- 発見方法
--    request_no nvarchar(40),                         -- 依頼No.
--    issue_date date,                                 -- 発行日
--    --urgency_name nvarchar(800),                      -- 緊急度 20240624出力対象外に変更
--    --desired_start_date date,                         -- 着工希望日 20240624出力対象外に変更
--    --desired_end_date date,                           -- 完了希望日 20240624出力対象外に変更
--    --request_content nvarchar(800),                   -- 依頼内容 20240624出力対象外に変更
--    --request_department_clerk_name nvarchar(800),     -- 依頼部課係 20240624出力対象外に変更
--    --request_personnel_tel nvarchar(200),             -- 依頼担当者tel 20240624出力対象外に変更
--    --request_department_chief_name nvarchar(800),     -- 依頼係長 20240624出力対象外に変更
--    --request_department_manager_name nvarchar(800),   -- 依頼課長 20240624出力対象外に変更
--    --request_department_foreman_name nvarchar(800),   -- 依頼職長 20240624出力対象外に変更
--    --maintenance_department_clerk_name nvarchar(800), -- 保全部課係 20240624出力対象外に変更
--    --request_reason nvarchar(800),                    -- 依頼事由 20240624出力対象外に変更
--    --examination_result nvarchar(800),                -- 件名検討結果 20240624出力対象外に変更
--    --construction_division_name nvarchar(800),        -- 工事区分 20240624出力対象外に変更
--    --construction_place nvarchar(800),                -- 場所 20240624出力対象外に変更

---- ma_plan
--	occurrence_date date,              -- 発生日
--    expected_construction_date date,   -- 着工予定日
--    total_budget_cost decimal(13, 3),  -- 予算金額(k円)
--    --subject_plan nvarchar(800),        -- 件名 20240624出力対象外に変更
--    --expected_completion_date date,     -- 終了予定日 20240624出力対象外に変更
--    --plan_man_hour decimal(13, 3),      -- 予定工数 20240624出力対象外に変更
--    --responsibility_name nvarchar(800), -- 自・他責 20240624出力対象外に変更
--    --failure_effect nvarchar(800),      -- 故障影響 20240624出力対象外に変更

---- ma_history
--    cost_note nvarchar(800),                    -- 費用メモ
--    maintenance_season_name nvarchar(800),      -- 保全時期
--    construction_personnel_name nvarchar(800),  -- 施工担当者
--    total_working_time decimal(13, 3),          -- 作業時間(Hr)
--    working_time_self decimal(13, 3),           -- 自係(Hr)
--    actual_result_name nvarchar(800),           -- 実績結果
--    construction_company nvarchar(800),         -- 施工会社
--    expenditure decimal(13, 3),                 -- 実績金額(k円)
--    --call_count nvarchar(800),                   -- 呼出 20240624出力対象外に変更
--    --loss_absence int,                           -- 休損量 20240624出力対象外に変更
--    --loss_absence_type_count decimal(13, 3),     -- 休損型数 20240624出力対象外に変更
--    -- occurrence_time datetime,                   -- 発生時刻 ※個別工場でしか管理していないため不要
--    -- discovery_personnel nvarchar(800),          -- 発見者 ※個別工場でしか管理していないため不要
--    -- working_time_research decimal(13, 3),       -- 調査時間 ※個別工場でしか管理していないため不要
--    -- working_time_procure decimal(13, 3),        -- 調達時間 ※個別工場でしか管理していないため不要
--    -- working_time_repair decimal(13, 3),         -- 修復時間 ※個別工場でしか管理していないため不要
--    -- working_time_test decimal(13, 3),           -- 試運転時間 ※個別工場でしか管理していないため不要
--    --working_time_company decimal(13, 3),        -- 作業時間(施工会社) 20240624出力対象外に変更
--    --maintenance_opinion nvarchar(800),          -- 保全見解 20240624出力対象外に変更
--    -- manufacturing_personnel_name nvarchar(800), -- 製造担当者 ※個別工場でしか管理していないため不要
--    -- work_failure_division_name nvarchar(800),   -- 作業・故障区分 ※個別工場でしか管理していないため不要
--    -- stop_count nvarchar(800),                   -- 系停止 ※個別工場でしか管理していないため不要
--    -- effect_production_name nvarchar(800),       -- 生産への影響 ※個別工場でしか管理していないため不要
--    -- effect_quality_name nvarchar(800),          -- 品質への影響 ※個別工場でしか管理していないため不要
--    -- failure_site nvarchar(800),                 -- 故障部位 ※個別工場でしか管理していないため不要
--    -- parts_existence_flg nvarchar(800),          -- 予備品(有無) ※個別工場でしか管理していないため不要
--    --follow_flg_history nvarchar(800),           -- フォロー有無 20240624出力対象外に変更
--    --rank_name nvarchar(800),                    -- ランク 20240624出力対象外に変更
--    -- failure_equipment_model_name nvarchar(800), -- 故障機器 ※個別工場でしか管理していないため不要
--    -- failure_time nvarchar(800),                 -- 故障時間 ※個別工場でしか管理していないため不要
--    -- history_importance_name nvarchar(800),      -- 履歴重要度 ※個別工場でしか管理していないため不要
--    -- history_conservation_name nvarchar(800),    -- 履歴保全方式 ※個別工場でしか管理していないため不要

---- ma_history_failure
--    phenomenon_name nvarchar(800),                 -- 現象
--    phenomenon_note nvarchar(800),                 -- 現象補足
--    failure_cause_name nvarchar(800),              -- 原因
--    failure_cause_note nvarchar(800),              -- 原因補足
--    failure_cause_personality_name1 nvarchar(800), -- 原因性格1
--    failure_cause_personality_name2 nvarchar(800), -- 原因性格2
--    failure_cause_personality_note nvarchar(800),  -- 性格補足
--    treatment_measure_name nvarchar(800),          -- 処置対策
--    failure_cause_addition_note nvarchar(800),     -- 故障原因
--    failure_status nvarchar(800),                  -- 故障状況
--    previous_situation nvarchar(800),              -- 故障前の保全実施状況
--    recovery_action nvarchar(800),                 -- 復旧措置
--    improvement_measure nvarchar(800),             -- 改善対策
--    system_feed_back nvarchar(800),                -- 保全システムのフィードバック
--    lesson nvarchar(800),                          -- 教訓
--    failure_note nvarchar(800),                    -- 特記(メモ)
--    --treatment_measure_note nvarchar(800),          -- 処置・対策メモ 20240624出力対象外に変更
--    -- failure_analysis_name nvarchar(800),           -- 故障分析 ※個別工場でしか管理していないため不要
--    -- failure_personality_factor_name nvarchar(800), -- 故障性格要因 ※個別工場でしか管理していないため不要
--    -- failure_personality_class_name nvarchar(800),  -- 故障性格分類 ※個別工場でしか管理していないため不要
--    -- treatment_status_name nvarchar(800),           -- 処置状況 ※個別工場でしか管理していないため不要
--    -- necessity_measure_name nvarchar(800),          -- 対策要否 ※個別工場でしか管理していないため不要
--    -- measure_plan_date date,                        -- 対策実施予定日 ※個別工場でしか管理していないため不要
--    -- measure_class1_name nvarchar(800),             -- 対策分類１ ※個別工場でしか管理していないため不要
--    -- measure_class2_name nvarchar(800),             -- 対策分類２ ※個別工場でしか管理していないため不要

---- ma_history_failureとma_history_machine・ma_history_inspection_site・ma_history_inspection_contentの共通部分
--    --used_days_machine int,                 -- 機器使用日数 20240624出力対象外に変更
--    inspection_site_name nvarchar(800),    -- 保全部位
--    inspection_content_name nvarchar(800), -- 保全内容
--    follow_flg nvarchar(800),              -- フォロー有無
--    follow_plan_date nvarchar(800),        -- フォロー予定日
--    follow_content nvarchar(800),          -- フォロー内容
--    --follow_completion_date date,           -- フォロー完了日 20240624出力対象外に変更
--    --work_record nvarchar(800),             -- 作業記録 20240624出力対象外に変更

---- mc_machine
--    machine_no nvarchar(800),                 -- 機器番号
--    machine_name nvarchar(800),               -- 機器名称
--    --factory_name2 nvarchar(800),              -- 工場(機器) 20240624出力対象外に変更
--    --plant_name2 nvarchar(800),                -- 工程(機器) 20240624出力対象外に変更
--    --series_name2 nvarchar(800),               -- 系列(機器) 20240624出力対象外に変更
--    --stroke_name2 nvarchar(800),               -- 設備(機器) 20240624出力対象外に変更
--    --facility_name2 nvarchar(800),             -- 場所５(機器) 20240624出力対象外に変更
--    --job_name2 nvarchar(800),                  -- 職種(機器) 20240624出力対象外に変更
--    --equipment_level nvarchar(800),            -- 機器レベル 20240624出力対象外に変更
--    --installation_location nvarchar(800),      -- 設置場所 20240624出力対象外に変更
--    --number_of_installation decimal(18, 0),    -- 設置台数 20240624出力対象外に変更
--    --date_of_installation date,                -- 設置年月 20240624出力対象外に変更
--    --importance_name nvarchar(800),            -- 重要度 20240624出力対象外に変更
--    --conservation_name nvarchar(800),          -- 保全方式 20240624出力対象外に変更
--    --machine_note nvarchar(800),               -- 機番メモ 20240624出力対象外に変更
--    --large_classfication_name2 nvarchar(800),  -- 機種大分類 20240624出力対象外に変更
--    --middle_classfication_name2 nvarchar(800), -- 機種中分類 20240624出力対象外に変更
--    --small_classfication_name2 nvarchar(800),  -- 機種小分類 20240624出力対象外に変更

---- mc_equipment
--    --use_segment_name nvarchar(800),    -- 使用区分 20240624出力対象外に変更
--    --circulation_target nvarchar(800),  -- 循環対象 20240624出力対象外に変更
--    --fixed_asset_no nvarchar(200),      -- 固定資産番号 20240624出力対象外に変更
--    --manufacturer_name nvarchar(800),   -- メーカー 20240624出力対象外に変更
--    --manufacturer_type nvarchar(200),   -- メーカー型式 20240624出力対象外に変更
--    --model_no nvarchar(200),            -- 型式コード 20240624出力対象外に変更
--    --serial_no nvarchar(200),           -- シリアル番号 20240624出力対象外に変更
--    --date_of_manufacture date,          -- 製造日 20240624出力対象外に変更
--    --equipment_note nvarchar(800),      -- 機器メモ 20240624出力対象外に変更

---- mc_applicable_laws
--    --applicable_laws_name1 nvarchar(800), -- 適用法規1 20240624出力対象外に変更
--    --applicable_laws_name2 nvarchar(800), -- 適用法規2 20240624出力対象外に変更
--    --applicable_laws_name3 nvarchar(800), -- 適用法規3 20240624出力対象外に変更
--    --applicable_laws_name4 nvarchar(800), -- 適用法規4 20240624出力対象外に変更
--    --applicable_laws_name5 nvarchar(800), -- 適用法規5 20240624出力対象外に変更

---- その他項目
--  progress_name nvarchar(800),  -- 進捗状況
--    seq int,                    -- 一時テーブル連番
--    occurrence_date_order date  -- 発生日(並び替え用)
--)

---- カーソル定義
--DECLARE cur_key CURSOR FOR
--	select Key1, languageId, factoryId from #temp

---- カーソルオープン
--OPEN cur_key;

---- １レコード取得
--FETCH NEXT FROM cur_key INTO @w_key1, @w_key2, @w_key3;

---- データの行数分ループ処理を実行する
--WHILE @@FETCH_STATUS = 0
--BEGIN
--	-- 保全活動一覧情報を取得
--    -- 複数行に対応させるため、一旦一時テーブルに保存
--	INSERT INTO #temp_rep EXEC [dbo].[pro_rep_rp0120] @w_key1, @w_key2, @w_key3
	
--	-- 次のレコードへ
--	FETCH NEXT FROM cur_key INTO @w_key1, @w_key2, @w_key3;
--END

---- カーソルクローズ
--CLOSE cur_key;
--DEALLOCATE cur_key;

---- 帳票データを返却
--SELECT * 
--    , '1' AS output_report_location_name_got_flg                -- 機能場所名称情報取得済フラグ（帳票用）
--    , '1' AS output_report_job_name_got_flg                     -- 職種・機種名称情報取得済フラグ（帳票用）
--FROM #temp_rep
--ORDER BY
--    occurrence_date_order DESC -- 発生日
--    , summary_id DESC          -- 保全活動件名ID
--    , seq ASC                  -- 機器の職種機種階層ID・機器番号・部位・内容で並び替えされた連番

---- 一時テーブル削除
--DROP TABLE #temp_rep