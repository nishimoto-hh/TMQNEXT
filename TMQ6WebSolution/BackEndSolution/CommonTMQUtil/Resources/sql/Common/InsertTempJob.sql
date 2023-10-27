INSERT 
INTO #temp_job 
SELECT
    * 
FROM
    STRING_SPLIT(@LocationIds, ',');