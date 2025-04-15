-- 詳細画面の保全項目一覧で選択された機器別管理基準標準詳細IDを格納する一時テーブルを作成
DROP TABLE IF EXISTS #temp_selected; 
CREATE TABLE #temp_selected(management_standards_detail_id int); 