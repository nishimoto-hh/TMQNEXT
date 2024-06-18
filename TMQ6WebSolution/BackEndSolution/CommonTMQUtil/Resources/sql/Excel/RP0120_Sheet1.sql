DECLARE @w_key1 bigint       -- 保全活動件名ID
DECLARE @w_key2 nvarchar(2)  -- 言語ID
DECLARE @w_key3 int          -- 本務工場

CREATE TABLE #temp_rep(
-- ma_summary
    summary_id int,                            -- 保全活動件名ID
	completion_date date,                      -- 完了日
	subject nvarchar(800),                     -- 件名
	mq_class_name nvarchar(800),               -- MQ分類
	district_name nvarchar(800),               -- 地区
    factory_name nvarchar(800),                -- 工場
    plant_name nvarchar(800),                  -- プラント
    series_name nvarchar(800),                 -- 系列
    stroke_name nvarchar(800),                 -- 工程
    facility_name nvarchar(800),               -- 設備
    job_name nvarchar(800),                    -- 職種
    stop_system_name nvarchar(800),            -- 系停止
    stop_time decimal(13,3),                   -- 系停止時間(Hr)
    sudden_division_name nvarchar(800),        -- 突発区分
    budget_management_name nvarchar(800),      -- 予算管理区分
    budget_personality_name nvarchar(800),     -- 予算性格区分
    maintenance_count int,                     -- カウント件数
    plan_implementation_content nvarchar(800), -- 作業計画・実施内容
    subject_note nvarchar(1200),               -- 件名メモ
    activity_division_name nvarchar(800),      -- 故障・点検区分
    repair_cost_name nvarchar(800),            -- 修繕費分類
    change_management_name nvarchar(800),      -- 変更管理
    env_safety_management_name nvarchar(800),  -- 環境安全管理区分
    construction_date date,                    -- 着工日
    completion_time datetime,                  -- 完了時刻

-- ma_request
    request_personnel_name nvarchar(800),            -- 依頼担当
    discovery_methods_name nvarchar(800),            -- 発見方法
    request_no nvarchar(40),                         -- 依頼No.
    issue_date date,                                 -- 発行日
    urgency_name nvarchar(800),                      -- 緊急度
    desired_start_date date,                         -- 着工希望日
    desired_end_date date,                           -- 完了希望日
    request_content nvarchar(800),                   -- 依頼内容
    request_department_clerk_name nvarchar(800),     -- 依頼部課係
    request_personnel_tel nvarchar(200),             -- 依頼担当者tel
    request_department_chief_name nvarchar(800),     -- 依頼係長
    request_department_manager_name nvarchar(800),   -- 依頼課長
    request_department_foreman_name nvarchar(800),   -- 依頼職長
    maintenance_department_clerk_name nvarchar(800), -- 保全部課係
    request_reason nvarchar(800),                    -- 依頼事由
    examination_result nvarchar(800),                -- 件名検討結果
    construction_division_name nvarchar(800),        -- 工事区分
    construction_place nvarchar(800),                -- 場所

-- ma_plan
	occurrence_date date,              -- 発生日
    expected_construction_date date,   -- 着工予定日
    total_budget_cost decimal(13, 3),  -- 予算金額(k円)
    subject_plan nvarchar(800),        -- 件名
    expected_completion_date date,     -- 終了予定日
    plan_man_hour decimal(13, 3),      -- 予定工数
    responsibility_name nvarchar(800), -- 自・他責
    failure_effect nvarchar(800),      -- 故障影響

-- ma_history
    cost_note nvarchar(800),                    -- 費用メモ
    maintenance_season_name nvarchar(800),      -- 保全時期
    construction_personnel_name nvarchar(800),  -- 施工担当者
    total_working_time decimal(13, 3),          -- 作業時間(Hr)
    working_time_self decimal(13, 3),           -- 自係(Hr)
    actual_result_name nvarchar(800),           -- 実績結果
    construction_company nvarchar(800),         -- 施工会社
    expenditure decimal(13, 3),                 -- 実績金額(k円)
    call_count nvarchar(800),                   -- 呼出
    loss_absence int,                           -- 休損量
    loss_absence_type_count decimal(13, 3),     -- 休損型数
    -- occurrence_time datetime,                   -- 発生時刻 ※個別工場でしか管理していないため不要
    -- discovery_personnel nvarchar(800),          -- 発見者 ※個別工場でしか管理していないため不要
    -- working_time_research decimal(13, 3),       -- 調査時間 ※個別工場でしか管理していないため不要
    -- working_time_procure decimal(13, 3),        -- 調達時間 ※個別工場でしか管理していないため不要
    -- working_time_repair decimal(13, 3),         -- 修復時間 ※個別工場でしか管理していないため不要
    -- working_time_test decimal(13, 3),           -- 試運転時間 ※個別工場でしか管理していないため不要
    working_time_company decimal(13, 3),        -- 作業時間(施工会社)
    maintenance_opinion nvarchar(800),          -- 保全見解
    -- manufacturing_personnel_name nvarchar(800), -- 製造担当者 ※個別工場でしか管理していないため不要
    -- work_failure_division_name nvarchar(800),   -- 作業・故障区分 ※個別工場でしか管理していないため不要
    -- stop_count nvarchar(800),                   -- 系停止 ※個別工場でしか管理していないため不要
    -- effect_production_name nvarchar(800),       -- 生産への影響 ※個別工場でしか管理していないため不要
    -- effect_quality_name nvarchar(800),          -- 品質への影響 ※個別工場でしか管理していないため不要
    -- failure_site nvarchar(800),                 -- 故障部位 ※個別工場でしか管理していないため不要
    -- parts_existence_flg nvarchar(800),          -- 予備品(有無) ※個別工場でしか管理していないため不要
    follow_flg_history nvarchar(800),           -- フォロー有無
    rank_name nvarchar(800),                    -- ランク
    -- failure_equipment_model_name nvarchar(800), -- 故障機器 ※個別工場でしか管理していないため不要
    -- failure_time nvarchar(800),                 -- 故障時間 ※個別工場でしか管理していないため不要
    -- history_importance_name nvarchar(800),      -- 履歴重要度 ※個別工場でしか管理していないため不要
    -- history_conservation_name nvarchar(800),    -- 履歴保全方式 ※個別工場でしか管理していないため不要

-- ma_history_failure
    phenomenon_name nvarchar(800),                 -- 現象
    phenomenon_note nvarchar(800),                 -- 現象補足
    failure_cause_name nvarchar(800),              -- 原因
    failure_cause_note nvarchar(800),              -- 原因補足
    failure_cause_personality_name1 nvarchar(800), -- 原因性格1
    failure_cause_personality_name2 nvarchar(800), -- 原因性格2
    failure_cause_personality_note nvarchar(800),  -- 性格補足
    treatment_measure_name nvarchar(800),          -- 処置対策
    failure_cause_addition_note nvarchar(800),     -- 故障原因
    failure_status nvarchar(800),                  -- 故障状況
    previous_situation nvarchar(800),              -- 故障前の保全実施状況
    recovery_action nvarchar(800),                 -- 復旧措置
    improvement_measure nvarchar(800),             -- 改善対策
    system_feed_back nvarchar(800),                -- 保全システムのフィードバック
    lesson nvarchar(800),                          -- 教訓
    failure_note nvarchar(800),                    -- 特記(メモ)
    treatment_measure_note nvarchar(800),          -- 処置・対策メモ
    -- failure_analysis_name nvarchar(800),           -- 故障分析 ※個別工場でしか管理していないため不要
    -- failure_personality_factor_name nvarchar(800), -- 故障性格要因 ※個別工場でしか管理していないため不要
    -- failure_personality_class_name nvarchar(800),  -- 故障性格分類 ※個別工場でしか管理していないため不要
    -- treatment_status_name nvarchar(800),           -- 処置状況 ※個別工場でしか管理していないため不要
    -- necessity_measure_name nvarchar(800),          -- 対策要否 ※個別工場でしか管理していないため不要
    -- measure_plan_date date,                        -- 対策実施予定日 ※個別工場でしか管理していないため不要
    -- measure_class1_name nvarchar(800),             -- 対策分類１ ※個別工場でしか管理していないため不要
    -- measure_class2_name nvarchar(800),             -- 対策分類２ ※個別工場でしか管理していないため不要

-- ma_history_failureとma_history_machine・ma_history_inspection_site・ma_history_inspection_contentの共通部分
    used_days_machine int,                 -- 機器使用日数
    inspection_site_name nvarchar(800),    -- 保全部位
    inspection_content_name nvarchar(800), -- 保全内容
    follow_flg nvarchar(800),              -- フォロー有無
    follow_plan_date nvarchar(800),        -- フォロー予定日
    follow_content nvarchar(800),          -- フォロー内容
    follow_completion_date date,           -- フォロー完了日
    work_record nvarchar(800),             -- 作業記録

-- mc_machine
    machine_no nvarchar(800),                 -- 機器番号
    machine_name nvarchar(800),               -- 機器名称
    factory_name2 nvarchar(800),              -- 工場(機器)
    plant_name2 nvarchar(800),                -- 工程(機器)
    series_name2 nvarchar(800),               -- 系列(機器)
    stroke_name2 nvarchar(800),               -- 設備(機器)
    facility_name2 nvarchar(800),             -- 場所５(機器)
    job_name2 nvarchar(800),                  -- 職種(機器)
    equipment_level nvarchar(800),            -- 機器レベル
    installation_location nvarchar(800),      -- 設置場所
    number_of_installation decimal(18, 0),    -- 設置台数
    date_of_installation date,                -- 設置年月
    importance_name nvarchar(800),            -- 重要度
    conservation_name nvarchar(800),          -- 保全方式
    machine_note nvarchar(800),               -- 機番メモ
    large_classfication_name2 nvarchar(800),  -- 機種大分類
    middle_classfication_name2 nvarchar(800), -- 機種中分類
    small_classfication_name2 nvarchar(800),  -- 機種小分類

-- mc_equipment
    use_segment_name nvarchar(800),    -- 使用区分
    circulation_target nvarchar(800),  -- 循環対象
    fixed_asset_no nvarchar(200),      -- 固定資産番号
    manufacturer_name nvarchar(800),   -- メーカー
    manufacturer_type nvarchar(200),   -- メーカー型式
    model_no nvarchar(200),            -- 型式コード
    serial_no nvarchar(200),           -- シリアル番号
    date_of_manufacture date,          -- 製造日
    equipment_note nvarchar(800),      -- 機器メモ

-- mc_applicable_laws
    applicable_laws_name1 nvarchar(800), -- 適用法規1
    applicable_laws_name2 nvarchar(800), -- 適用法規2
    applicable_laws_name3 nvarchar(800), -- 適用法規3
    applicable_laws_name4 nvarchar(800), -- 適用法規4
    applicable_laws_name5 nvarchar(800), -- 適用法規5

-- その他項目
  progress_name nvarchar(800),  -- 進捗状況
    seq int,                    -- 一時テーブル連番
    occurrence_date_order date  -- 発生日(並び替え用)
)

-- カーソル定義
DECLARE cur_key CURSOR FOR
	select Key1, languageId, factoryId from #temp

-- カーソルオープン
OPEN cur_key;

-- １レコード取得
FETCH NEXT FROM cur_key INTO @w_key1, @w_key2, @w_key3;

-- データの行数分ループ処理を実行する
WHILE @@FETCH_STATUS = 0
BEGIN
	-- 保全活動一覧情報を取得
    -- 複数行に対応させるため、一旦一時テーブルに保存
	INSERT INTO #temp_rep EXEC [dbo].[pro_rep_rp0120] @w_key1, @w_key2, @w_key3
	
	-- 次のレコードへ
	FETCH NEXT FROM cur_key INTO @w_key1, @w_key2, @w_key3;
END

-- カーソルクローズ
CLOSE cur_key;
DEALLOCATE cur_key;

-- 帳票データを返却
SELECT * 
    , '1' AS output_report_location_name_got_flg                -- 機能場所名称情報取得済フラグ（帳票用）
    , '1' AS output_report_job_name_got_flg                     -- 職種・機種名称情報取得済フラグ（帳票用）
FROM #temp_rep
ORDER BY
    occurrence_date_order DESC -- 発生日
    , summary_id DESC          -- 保全活動件名ID
    , seq ASC                  -- 機器の職種機種階層ID・機器番号・部位・内容で並び替えされた連番

-- 一時テーブル削除
DROP TABLE #temp_rep