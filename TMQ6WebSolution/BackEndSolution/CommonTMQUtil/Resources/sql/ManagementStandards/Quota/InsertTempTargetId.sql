-- 詳細画面の保全項目一覧で選択された機器別管理基準標準詳細IDを一時テーブルに格納
INSERT 
INTO #temp_selected 
SELECT
    * 
FROM
    STRING_SPLIT(@ManagementStandardsDetailIdList, ','); 