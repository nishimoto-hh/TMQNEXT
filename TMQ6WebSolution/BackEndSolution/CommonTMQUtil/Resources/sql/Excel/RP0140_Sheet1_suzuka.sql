DECLARE @w_key1 bigint       -- 保全活動件名ID
DECLARE @w_key2 nvarchar(2)  -- 言語ID
DECLARE @w_key3 int          -- 本務工場

CREATE TABLE #temp_rep(
		    job_structure_id            int,            -- 職種機種階層ID
		    location_structure_id       int,            -- 機能場所階層ID
			job_structure_id2           int,            -- 職種機種階層ID(機器)
			location_structure_id2      int,            -- 機能場所階層ID(機器)
			construction_personnel_name	nvarchar(800)	,--	担当者
			subject	nvarchar(800)	,--	件名
			occurrence_date	date	,--	作業/故障　　　発生日
			buid	nvarchar(800)	,--	部位大分類
			completion_date	date	,--	完了年月日
			buic	nvarchar(800)	,--	部位中分類
			plan_implementation_content	nvarchar(800)	,--	概要/故障状況
			work_failure_division_name	nvarchar(800)	,--	作業/故障区分
			buis	nvarchar(800)	,--	部位小分類
			zairyou	nvarchar(800)	,--	材料費(機器)
			total_budget_cost	decimal(13, 3)	,--	予算費用(任意)
			expenditure	decimal(13, 3)	,--	実績費用(任意)
			series_name	nvarchar(800)	,--	系列
			stroke_name	nvarchar(800)	,--	工程
			machine_name	nvarchar(800)	,--	機器名称
			equipment_level_structure_id	nvarchar(800)	,--	機器レベル
			failure_equipment_model_structure_id	nvarchar(800)	,--	故障機種（機器）
			small_classfication_name2	nvarchar(800)	,--	機種小分類
			working_time_self	nvarchar(800)	,--	自社
			hozenjiki	nvarchar(800)	,--	保全時期
			history_conservation_structure_id	nvarchar(800)	,--	保全方式
			roumuhi	nvarchar(800)	,--	労務費(機器)
			discovery_personnel	nvarchar(800)	,--	発見者
			discovery_methods_structure_id	nvarchar(800)	,--	発見方法
			recovery_action	nvarchar(800)	,--	復旧処置
			failure_personality_factor_structure_id	nvarchar(800)	,--	故障原因性格要因　(故障区分に合わせる)
			failure_personality_class_structure_id	nvarchar(800)	,--	故障原因　　　性格分類　　　(○%で検索)
			effect_production_structure_id	nvarchar(800)	,--	生産への影響
			effect_quality_structure_id	nvarchar(800)	,--	品質への影響
			cost_note	decimal(13, 3)	,--	費用メモ
			stop_time	decimal(13, 3)	,--	系停止時間
			zappi	nvarchar(800)	,--	雑費(機器)
			kyoutuuhi	nvarchar(800)	,--	共通費(機器)
			sagyoujikan	nvarchar(800)	,--	作業時間(機器)
			jisya	nvarchar(800)	,--	自社時間(機器)
			sekou	nvarchar(800)	,--	施工会社（機器）
			failure_cause_name	nvarchar(800)	,--	故障原因
			working_time_repair	nvarchar(800)	,--	修理時間(総計)
			treatment_measure_structure_id	nvarchar(800)	,--	処置・対策
			expenditure2	nvarchar(800)	,--	実績金額
			job_name	nvarchar(800)	,--	職種
			machine_factory_name	nvarchar(800)	,--	工場(機器)
			machine_stroke_name	nvarchar(800)	,--	工程(機器)
			bunsekisyo	nvarchar(800)	,--	故障原因分析書
			machine_series_name	nvarchar(800)	,--	系列(機器)
			saihatsu1	nvarchar(800)	,--	再発防止対策
			applicable_laws_structure_name1	nvarchar(800)	,--	適用法規１
			applicable_laws_structure_name2	nvarchar(800)	,--	適用法規２
			applicable_laws_structure_name3	nvarchar(800)	,--	適用法規３
			saihatsuhiduke	date	,--	再発防止対策予定日
			applicable_laws_structure_name4	nvarchar(800)	,--	適用法規５
			saihatsu2	nvarchar(800)	,--	再発防止対策実施
			factory_name	nvarchar(800)	,--	工場
			machine_no	nvarchar(800)	,--	機器番号
			call_count	int	,--	呼出
		    seq                         int             ,-- 一時テーブル連番
			order_id                    int IDENTITY(1,1) -- 登録順（ソート用）
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
	INSERT INTO #temp_rep EXEC [dbo].[pro_rep_rp0140_suzuka] @w_key1, @w_key2, @w_key3
	
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
ORDER BY order_id

-- 一時テーブル削除
DROP TABLE #temp_rep

