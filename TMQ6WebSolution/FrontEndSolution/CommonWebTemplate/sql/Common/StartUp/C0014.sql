/*
 * マスタメンテナンス 拡張項目　コンボ用データリストSQL
 */
-- C0014
-- 画面項目定義の関連情報パラメータ
WITH params AS(
    SELECT DISTINCT 
         relation_id
        ,relation_parameters
       ,value AS param
    FROM cm_form_control_define
    CROSS APPLY STRING_SPLIT(relation_parameters, ',')
    WHERE
        control_type = '0701'
    AND relation_id = 'C0014'
)
-- カンマ区切り時の出現位置
,ordinal AS (
    SELECT *
        ,CASE WHEN CHARINDEX(',' + param, relation_parameters) = 0 THEN
            CHARINDEX(param, relation_parameters)
         ELSE 
            CHARINDEX(',' + param, relation_parameters)
         END idx
    FROM params
)
-- パラメータ(順番付き)
,param_idx AS(
SELECT 
     params.*
    ,ROW_NUMBER() OVER (PARTITION BY params.relation_id, params.relation_parameters ORDER BY ordinal.idx) AS idx
FROM params
LEFT JOIN
    ordinal
ON  params.relation_id = ordinal.relation_id
AND params.relation_parameters = ordinal.relation_parameters
AND params.param = ordinal.param
)

SELECT
     param_idx.relation_parameters AS ParamKey
    ,0 AS factoryId
    ,0 AS translationFactoryId
    ,CONVERT(int, param_idx.param) AS 'values'
    ,param_idx.param AS labels 
    ,0 AS orderFactoryId
FROM
    param_idx
ORDER BY param_idx.relation_parameters, param_idx.idx