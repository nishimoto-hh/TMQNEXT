INSERT 
INTO #temp_job_id 
SELECT
    * 
FROM
    STRING_SPLIT(@JobIdList, ',');