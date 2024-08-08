-- 機器一覧にて選択されたレコードの機番IDと入力された開始日を格納するための一時テーブルを作成
DROP TABLE IF EXISTS #temp_machine; 
CREATE TABLE #temp_machine(machine_id bigint, start_date DATE); 