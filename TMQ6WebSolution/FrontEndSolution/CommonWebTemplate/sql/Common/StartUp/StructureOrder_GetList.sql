-- 工場別表示順
-- 構成マスタ内の全工場
WITH factory AS(
    SELECT
        st.structure_id AS factory_id
    FROM
        ms_structure AS st
    WHERE
        st.structure_group_id = 1000
    AND st.structure_layer_no = 1
    AND st.delete_flg != 1
    UNION
    -- 共通工場は構成マスタに無いので追加
    SELECT
         0
)
-- 画面項目定義の関連情報パラメータ
,params AS(
    SELECT DISTINCT 
         relation_id
        ,relation_parameters
       ,value AS param
    FROM cm_form_control_define
  CROSS APPLY STRING_SPLIT(relation_parameters, ',')
    WHERE
        control_type = '0701'
    AND relation_id IN ('C0001', 'C0002')
    AND relation_parameters NOT LIKE '%@%'
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
-- 構成グループID
,structure_group AS (
    SELECT 
         params.param AS id
    FROM params
    LEFT JOIN
        ordinal
    ON  params.relation_id = ordinal.relation_id
    AND params.relation_parameters = ordinal.relation_parameters
    AND params.param = ordinal.param
    WHERE
        ordinal.idx = 1
)
SELECT 
     structure_id
    ,factory_id
    ,display_order
FROM
    ms_structure_order
WHERE
    structure_group_id IN (SELECT id FROM structure_group)
AND factory_id IN (SELECT factory_id FROM factory)
ORDER BY structure_id, factory_id
