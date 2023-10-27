DECLARE @w_key1 bigint       -- 長期計画件名ID
DECLARE @w_key2 nvarchar(2)  -- 言語ID
DECLARE @w_key3 int          -- 本務工場

CREATE TABLE #temp_rep(
	job_structure_id         int,           -- 職種機種階層ID
	location_structure_id    int,           -- 機能場所階層ID
	job_structure_id2        int,           -- 職種機種階層ID(機器)
	location_structure_id2   int,           -- 機能場所階層ID(機器)
	person_name              nvarchar(800), -- 長計担当者
	long_plan_id             bigint,        -- 件名NO
	subject                  nvarchar(800), -- 長計件名
	work_item_name           nvarchar(800), -- 作業項目
	budget_management_name   nvarchar(800), -- 予算管理区分
	budget_personality_name  nvarchar(800), -- 予算性格区分
	purpose_name             nvarchar(800), -- 目的区分
	work_class_name          nvarchar(800), -- 作業区分
	treatment_name           nvarchar(800), -- 処置区分
	facility_division_name   nvarchar(800), -- 設備区分
	subject_note             nvarchar(800), -- 件名メモ欄
	machine_no               nvarchar(800), -- 機器番号
	machine_name             nvarchar(800), -- 機器名称
	budget_amount            decimal(13,3), -- 予算金額
	equipment_level          nvarchar(800), -- 機器レベル
	installation_location    nvarchar(800), -- 設置場所
    number_of_installation   int,           -- 設置台数
	date_of_installation	 nvarchar(800), -- 設置年月
	importance_name          nvarchar(800), -- 重要度
    conservation_name        nvarchar(800), -- 保全方式
	equipment_note           nvarchar(800), -- 機器メモ
	use_segment_name         nvarchar(800), -- 使用区分
	fixed_asset_no           nvarchar(200), -- 固定資産番号
    manufacturer_name        nvarchar(800), -- メーカー
    manufacturer_type        nvarchar(200), -- メーカー型式
    model_no                 nvarchar(200), -- 型式コード
    serial_no                nvarchar(200), -- 製造番号
    date_of_manufacture      nvarchar(800), -- 製造年月
	inspection_site_name     nvarchar(800), -- 保全部位(共通詳細)
	inspection_content_name  nvarchar(800), -- 保全項目(共通詳細)
	inspection_site_conservation_name  nvarchar(800), -- 部位保全方式(共通詳細)
	disp_cycle               nvarchar(20),            -- 部位保全周期(共通詳細)
	factory_name2            nvarchar(800), -- 工場名(機器)
    plant_name2              nvarchar(800), -- プラント名(機器)
    series_name2             nvarchar(800), -- 系列名(機器)
    stroke_name2             nvarchar(800), -- 工程名(機器)
    facility_name2           nvarchar(800), -- 設備名(機器)
    job_name2                nvarchar(800), -- 職種名(機器)
    large_classfication_name2    nvarchar(800),  -- 大分類(機器)
    middle_classfication_name2   nvarchar(800),  -- 中分類(機器)
    small_classfication_name2    nvarchar(800),  -- 小分類(機器)
	seq                      int,           -- 一時テーブル連番
    spec_text01              nvarchar(800), -- テキスト仕様01
    spec_text02              nvarchar(800), -- テキスト仕様02
	spec_text03              nvarchar(800), -- テキスト仕様03
	spec_text04              nvarchar(800), -- テキスト仕様04
	spec_text05              nvarchar(800), -- テキスト仕様05
	spec_text06              nvarchar(800), -- テキスト仕様06
	spec_text07              nvarchar(800), -- テキスト仕様07
	spec_text08              nvarchar(800), -- テキスト仕様08
	spec_text09              nvarchar(800), -- テキスト仕様09
	spec_text10              nvarchar(800), -- テキスト仕様10
	spec_text11              nvarchar(800), -- テキスト仕様11
	spec_text12              nvarchar(800), -- テキスト仕様12
	spec_text13              nvarchar(800), -- テキスト仕様13
	spec_text14              nvarchar(800), -- テキスト仕様14
	spec_text15              nvarchar(800), -- テキスト仕様15
	spec_text16              nvarchar(800), -- テキスト仕様16
	spec_text17              nvarchar(800), -- テキスト仕様17
	spec_text18              nvarchar(800), -- テキスト仕様18
	spec_text19              nvarchar(800), -- テキスト仕様19
	spec_text20              nvarchar(800), -- テキスト仕様20
	spec_num01               nvarchar(800), -- 数値仕様01
	spec_num02               nvarchar(800), -- 数値仕様02
	spec_num03               nvarchar(800), -- 数値仕様03
	spec_num04               nvarchar(800), -- 数値仕様04
	spec_num05               nvarchar(800), -- 数値仕様05
	spec_num06               nvarchar(800), -- 数値仕様06
	spec_num07               nvarchar(800), -- 数値仕様07
	spec_num08               nvarchar(800), -- 数値仕様08
	spec_num09               nvarchar(800), -- 数値仕様09
	spec_num10               nvarchar(800), -- 数値仕様10
	spec_num11               nvarchar(800), -- 数値仕様11
	spec_num12               nvarchar(800), -- 数値仕様12
	spec_num13               nvarchar(800), -- 数値仕様13
	spec_num14               nvarchar(800), -- 数値仕様14
	spec_num15               nvarchar(800), -- 数値仕様15
	spec_num16               nvarchar(800), -- 数値仕様16
	spec_num17               nvarchar(800), -- 数値仕様17
	spec_num18               nvarchar(800), -- 数値仕様18
	spec_num19               nvarchar(800), -- 数値仕様19
	spec_num20               nvarchar(800), -- 数値仕様20
	spec_range01             nvarchar(800), -- 範囲仕様01
	spec_range02             nvarchar(800), -- 範囲仕様02
	spec_range03             nvarchar(800), -- 範囲仕様03
	spec_range04             nvarchar(800), -- 範囲仕様04
	spec_range05             nvarchar(800), -- 範囲仕様05
	spec_range06             nvarchar(800), -- 範囲仕様06
	spec_range07             nvarchar(800), -- 範囲仕様07
	spec_range08             nvarchar(800), -- 範囲仕様08
	spec_range09             nvarchar(800), -- 範囲仕様09
	spec_range10             nvarchar(800), -- 範囲仕様10
	spec_range11             nvarchar(800), -- 範囲仕様11
	spec_range12             nvarchar(800), -- 範囲仕様12
	spec_range13             nvarchar(800), -- 範囲仕様13
	spec_range14             nvarchar(800), -- 範囲仕様14
	spec_range15             nvarchar(800), -- 範囲仕様15
	spec_range16             nvarchar(800), -- 範囲仕様16
	spec_range17             nvarchar(800), -- 範囲仕様17
	spec_range18             nvarchar(800), -- 範囲仕様18
	spec_range19             nvarchar(800), -- 範囲仕様19
	spec_range20             nvarchar(800), -- 範囲仕様20
	spec_select01            nvarchar(800), -- 選択仕様01
	spec_select02            nvarchar(800), -- 選択仕様02
	spec_select03            nvarchar(800), -- 選択仕様03
	spec_select04            nvarchar(800), -- 選択仕様04
	spec_select05            nvarchar(800)  -- 選択仕様05
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
	INSERT INTO #temp_rep EXEC [dbo].[pro_rep_rp0080] @w_key1, @w_key2, @w_key3
	
	-- 次のレコードへ
	FETCH NEXT FROM cur_key INTO @w_key1, @w_key2, @w_key3;
END

-- カーソルクローズ
CLOSE cur_key;
DEALLOCATE cur_key;

-- 帳票データを返却
SELECT * FROM #temp_rep

-- 一時テーブル削除
DROP TABLE #temp_rep

