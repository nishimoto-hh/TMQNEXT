-- コンボボックス用翻訳
-- 関連情報パラメータ
WITH params AS(
    SELECT DISTINCT 
         relation_id
        ,relation_parameters
       ,value AS param
    FROM cm_form_control_define
  CROSS APPLY STRING_SPLIT(relation_parameters, ',')
    WHERE
        control_type = '0701'
    AND relation_id IN ('C0001', 'C0002', 'C0035')
    AND relation_parameters IS NOT NULL
    AND relation_parameters NOT LIKE '%@%'
)
-- 関連情報パラメータの分割時の出現位置
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
-- 日本語翻訳用の翻訳ID
,translation_ja AS(
    SELECT
        st.item_translation_id
    FROM 
        v_structure_item_all AS st
    WHERE 
        st.language_id = /*languageId*/'ja'
    AND CONVERT(VARCHAR,st.structure_group_id) IN (
        SELECT param_idx.param AS id FROM param_idx WHERE idx = 1)  -- 第一パラメータ：構成グループID
)

-- 日本語翻訳用の翻訳IDに対応する他言語の翻訳を取得する
SELECT
     tr.location_structure_id AS factoryId
    ,tr.translation_id AS messageId
    ,tr.translation_text AS value
    ,tr.language_id AS languageCd
FROM
    ms_translation tr
WHERE
    tr.language_id != /*languageId*/'ja'
AND tr.delete_flg != 1
AND tr.translation_id IN (SELECT item_translation_id FROM translation_ja)
AND tr.translation_text IS NOT NULL


