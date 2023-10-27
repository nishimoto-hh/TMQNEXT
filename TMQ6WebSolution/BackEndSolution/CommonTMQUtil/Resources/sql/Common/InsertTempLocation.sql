INSERT 
INTO #temp_location 
SELECT
    * 
FROM
    STRING_SPLIT(@LocationIds, ',');