DECLARE @w_key1 bigint       -- 保全活動件名ID
DECLARE @w_key2 nvarchar(2)  -- 言語ID
DECLARE @w_key3 int          -- 本務工場

CREATE TABLE #temp_rep(
	job_structure_id            int,            -- 職種機種階層ID
	location_structure_id       int,            -- 機能場所階層ID
	job_structure_id2           int,            -- 職種機種階層ID(機器)
	location_structure_id2      int,            -- 機能場所階層ID(機器)
	occurrence_date             date,           -- 発生日
    subject                     nvarchar(800),  -- 件名
	plan_implementation_content nvarchar(800),  -- 作業計画内容
	phenomenon_name             nvarchar(800),  -- 現象
	call_count_name             nvarchar(800),  -- 呼出
    sudden_division_name        nvarchar(800),  -- 突発区分
    stop_count_name             nvarchar(800),  -- 系停止
	work_purpose_name           nvarchar(800),  -- 目的区分
	maintenance_season_name     nvarchar(800),  -- 時期
	expected_construction_date  date,           -- 着工予定日
	expected_completion_date    date,           -- 終了予定日
	total_budget_cost           decimal(13, 3), -- 予算金額
    construction_personnel_name nvarchar(800),  -- 担当者
	construction_company        nvarchar(800),  -- 施工会社
    discovery_methods_name      nvarchar(800),  -- 発見方法
    plan_man_hour               decimal(13, 3), -- 予定工数
    subject_note                nvarchar(800),  -- メモ
	budget_management_name      nvarchar(800),  -- 予算管理区分
	budget_personality_name     nvarchar(800),  -- 予算性格区分
    machine_no                  nvarchar(800),  -- 機器番号
    machine_name                nvarchar(800),  -- 機器名称
	equipment_level             nvarchar(800),  -- 機器レベル
	installation_location       nvarchar(800),  -- 設置場所
    number_of_installation      decimal,        -- 設置台数
    date_of_installation        nvarchar(200),  -- 設置年月
    importance_name             nvarchar(800),  -- 重要度
    conservation_name           nvarchar(800),  -- 保全方式
	applicable_laws_name1       nvarchar(800),  -- 適用法規１
    applicable_laws_name2       nvarchar(800),  -- 適用法規２
    applicable_laws_name3       nvarchar(800),  -- 適用法規３
    applicable_laws_name4       nvarchar(800),  -- 適用法規４
    applicable_laws_name5       nvarchar(800),  -- 適用法規５
    machine_note                nvarchar(800),  -- 機番メモ
    use_segment_name            nvarchar(800),  -- 使用区分
	circulation_target          nvarchar(800),  -- 循環対象
    fixed_asset_no              nvarchar(800),  -- 固定資産番号
	manufacturer_name           nvarchar(800),  -- メーカー
	manufacturer_type           nvarchar(200),  -- メーカー型式
    model_no                    nvarchar(200),  -- 型式コード
    serial_no                   nvarchar(200),  -- 製造番号
    date_of_manufacture         nvarchar(200),  -- 製造年月
    equipment_note              nvarchar(800),  -- 機器メモ
	district_name               nvarchar(800),  -- 地区名
	factory_name                nvarchar(800),  -- 工場名
    plant_name                  nvarchar(800),  -- プラント名
    series_name                 nvarchar(800),  -- 系列名
    stroke_name                 nvarchar(800),  -- 工程名
    facility_name               nvarchar(800),  -- 設備名
    job_name                    nvarchar(800),  -- 職種名
	district_name2              nvarchar(800),  -- 地区名(機器)
	factory_name2               nvarchar(800),  -- 工場名(機器)
    plant_name2                 nvarchar(800),  -- プラント名(機器)
    series_name2                nvarchar(800),  -- 系列名(機器)
    stroke_name2                nvarchar(800),  -- 工程名(機器)
    facility_name2              nvarchar(800),  -- 設備名(機器)
    job_name2                   nvarchar(800),  -- 職種名(機器)
    large_classfication_name2   nvarchar(800),  -- 大分類(機器)
    middle_classfication_name2  nvarchar(800),  -- 中分類(機器)
    small_classfication_name2   nvarchar(800),  -- 小分類(機器)
	seq                         int             -- 一時テーブル連番
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
	-- 構成一覧 - ループ構成情報取得
    -- 複数行に対応させるため、一旦一時テーブルに保存
	INSERT INTO #temp_rep EXEC [dbo].[pro_rep_rp0140] @w_key1, @w_key2, @w_key3
	
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

-- 一時テーブル削除
DROP TABLE #temp_rep

