-- 条件に指定された場所階層IDはリストで渡した構成IDが2100個以上だとエラーになるため一時テーブルに格納する
DROP TABLE IF EXISTS #temp_location_stcucture_id; 

CREATE TABLE #temp_location_stcucture_id(location_stcucture_id int); 

INSERT 
INTO #temp_location_stcucture_id 
SELECT
    * 
FROM
    STRING_SPLIT(@StrLocationStructureIdList, ',');


-- 条件に指定された職種階層IDはリストで渡した構成IDが2100個以上だとエラーになるため一時テーブルに格納する
DROP TABLE IF EXISTS #temp_job_stcucture_id; 

CREATE TABLE #temp_job_stcucture_id(job_stcucture_id int); 

INSERT 
INTO #temp_job_stcucture_id 
SELECT
    * 
FROM
    STRING_SPLIT(@StrJobStcuctureIdList, ',');

WITH item AS ( 
    --循環対象の拡張データを取得
    SELECT
        structure_id
        , ie.extension_data 
    FROM
        ms_structure si 
        INNER JOIN ms_item_extension ie 
            ON si.structure_item_id = ie.item_id 
            AND si.structure_group_id = 1920 
            AND ie.sequence_no = 1
),
structure_factory AS (
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1170,1200,1030,1150,1210) 
        AND language_id = @LanguageId
) 
