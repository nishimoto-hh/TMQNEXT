-- 検索対象の仕様項目IDを一時テーブルに格納する
INSERT 
INTO #temp_spec_id 
SELECT
    * 
FROM
    STRING_SPLIT(@SpecIdList, ',');