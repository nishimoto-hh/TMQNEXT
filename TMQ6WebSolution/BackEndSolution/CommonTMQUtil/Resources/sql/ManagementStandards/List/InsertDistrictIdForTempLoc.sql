-- 検索に使用する一時テーブルに登録されているデータの地区ID、工場IDを登録する
WITH factory_list AS ( 
    SELECT
        ms.factory_id 
    FROM
        ms_structure ms 
        INNER JOIN #temp_location tmp 
            ON ms.structure_id = tmp.structure_id 
    WHERE
        ms.structure_group_id = 1000
) 
INSERT 
INTO #temp_location 
SELECT
    ms.parent_structure_id 
FROM
    ms_structure ms 
    INNER JOIN factory_list factory 
        ON ms.structure_id = factory.factory_id 
WHERE
    ms.structure_group_id = 1000 
    AND ms.structure_layer_no = 1 
UNION ALL 
SELECT
    * 
FROM
    factory_list
;