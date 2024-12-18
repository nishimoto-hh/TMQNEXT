/*
 * 名称マスタ コンボ用データリスト用SQL.(絞り込み条件付き、拡張データとの紐付け有）
 * C0001を拡張.
 * exparam1:アイテム拡張マスタの拡張データ
 * exparam2:アイテム拡張マスタの拡張データの値を構成アイテムIDに持つ、構成マスタの構成ID
 */
-- C0002
-- 構成マスタ内の全工場
WITH factory AS(
    SELECT
        st.structure_id AS factory_id
    FROM
        ms_structure AS st
    WHERE
        st.structure_group_id = 1000    -- 場所階層
    AND st.structure_layer_no = 1       -- 工場
    AND st.delete_flg != 1
    -- 共通工場は構成マスタに無いので追加
    UNION
    SELECT 0 AS factory_id
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
    AND relation_id = 'C0002'
    /*IF RelationParam == null || RelationParam == ''*/
    AND relation_parameters NOT LIKE '%@%'  -- 連動コンボは除外
    /*END*/
    /*IF RelationParam != null && RelationParam != ''*/
    AND relation_parameters LIKE /*RelationParam*/'1100%'  -- 連動コンボは除外
    /*END*/
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
    WHERE params.param IS NOT NULL AND params.param != ''
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
-- 条件
,conditions AS (
    SELECT
         param1.relation_id
        ,param1.relation_parameters
        ,param1.param AS param1     -- 第一パラメータ：構成グループID
        ,param2.param AS param2     -- 第二パラメータ：階層番号
        ,param3.param AS param3     -- 第三パラメータ：連番(アイテムマスタ拡張)
    FROM
        (SELECT *  FROM param_idx WHERE idx = 1) param1         -- 第一パラメータ
    LEFT JOIN
        (SELECT *  FROM param_idx WHERE idx = 2) param2         -- 第二パラメータ
    ON  param1.relation_id = param2.relation_id
    AND param1.relation_parameters = param2.relation_parameters
    LEFT JOIN
        (SELECT *  FROM param_idx WHERE idx = 3) param3         -- 第三パラメータ
    ON  param1.relation_id = param3.relation_id
    AND param1.relation_parameters = param3.relation_parameters
)

SELECT
     conditions.relation_parameters AS paramKey
    ,item.factory_id AS factoryId
    ,item.location_structure_id AS translationFactoryId
    ,item.item_translation_id AS translationId
    ,item.structure_id AS 'values'
    ,item.translation_text AS labels
    ,item_ex.extension_data AS exparam1
    ,ex_item_1.structure_id AS exparam2
    ,ex_item_1.translation_text AS exparam3
    ,item.delete_flg AS deleteFlg
     -- 表示順用工場ID
    ,coalesce(ft.factory_id, 0) AS orderFactoryId
FROM
    v_structure_item_all AS item
INNER JOIN  conditions
ON item.structure_group_id = conditions.param1
AND ((conditions.param2 IS NULL OR conditions.param2 = '' OR conditions.param2 = '''''') OR item.structure_layer_no = conditions.param2)

-- 工場ごとに工場別表示順を取得する
CROSS JOIN factory AS ft
LEFT OUTER JOIN ms_structure_order AS order_factory
ON  item.structure_id = order_factory.structure_id
AND order_factory.factory_id = ft.factory_id

-- 全工場共通の表示順
LEFT OUTER JOIN ms_structure_order AS order_common
ON  item.structure_id = order_common.structure_id
AND order_common.factory_id = 0

LEFT OUTER JOIN ms_item_extension AS item_ex
ON (item.structure_item_id = item_ex.item_id AND item_ex.sequence_no = conditions.param3) 
LEFT OUTER JOIN v_structure_item_all AS  ex_item_1
ON (item_ex.extension_data = CAST(ex_item_1.structure_item_id AS varchar) AND item.language_id = ex_item_1.language_id)

WHERE 
    item.language_id = /*languageId*/'ja'

AND
    NOT EXISTS(
        SELECT
            *
        FROM
            ms_structure_unused AS unused
        WHERE
            unused.factory_id = ft.factory_id
        AND unused.structure_id = item.structure_id
    )

ORDER BY item.structure_group_id, item.structure_layer_no
-- 工場ID毎の表示順
,row_number() over(partition BY coalesce(ft.factory_id, 0) ORDER BY coalesce(coalesce(order_factory.display_order, order_common.display_order), 32768), item.structure_id)
