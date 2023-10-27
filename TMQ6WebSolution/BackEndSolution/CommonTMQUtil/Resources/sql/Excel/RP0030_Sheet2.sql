DECLARE @w_key1_2 bigint       -- 機番ID
DECLARE @w_key2_2 nvarchar(2)  -- 言語ID
DECLARE @w_key3_2 int          -- 本務工場

CREATE TABLE #temp_rep2(
    equipment_level_name  nvarchar(800),
	parent_no             nvarchar(800),
	parent_name           nvarchar(800),
	child_no              nvarchar(800),
	child_name            nvarchar(800),
	job_structure_id      int,
	importance_name       nvarchar(200),
	conservation_name     nvarchar(200),
	equipment_level       nvarchar(200),
	use_segment_name      nvarchar(200),
	circulation_target    nvarchar(200),
    fixed_asset_no        nvarchar(200),
	location_structure_id int,
	manufacturer_name     nvarchar(200),
	manufacturer_type     nvarchar(200),
	model_no              nvarchar(200),
	serial_no             nvarchar(200),
	date_of_manufacture   nvarchar(10),
	loop_id               bigint,
	update_serialid       int,
	flg_child             int
)

-- カーソル定義
DECLARE cur_key2 CURSOR FOR
	select Key1, languageId, factoryId from #temp

-- カーソルオープン
OPEN cur_key2;

-- １レコード取得
FETCH NEXT FROM cur_key2 INTO @w_key1_2, @w_key2_2, @w_key3_2;

-- データの行数分ループ処理を実行する
WHILE @@FETCH_STATUS = 0
BEGIN
	-- 構成一覧 - ループ構成情報取得
    -- 複数行に対応させるため、一旦一時テーブルに保存
	INSERT INTO #temp_rep2 EXEC [dbo].[pro_rep_rp0030_2] @w_key1_2, @w_key2_2, @w_key3_2
	
	-- 次のレコードへ
	FETCH NEXT FROM cur_key2 INTO @w_key1_2, @w_key2_2, @w_key3_2;
END

-- カーソルクローズ
CLOSE cur_key2;
DEALLOCATE cur_key2;

-- 帳票データを返却
SELECT * FROM #temp_rep2

-- 一時テーブル削除
DROP TABLE #temp_rep2

