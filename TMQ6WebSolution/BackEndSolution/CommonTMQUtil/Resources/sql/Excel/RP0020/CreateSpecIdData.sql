-- 検索対象の仕様項目IDを格納するための一時テーブルを作成
DROP TABLE IF EXISTS #temp_spec_id; 

CREATE TABLE #temp_spec_id(spec_id bigint); 