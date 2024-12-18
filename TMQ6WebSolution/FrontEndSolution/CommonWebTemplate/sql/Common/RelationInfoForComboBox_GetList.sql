WITH TargetConduct AS (
    -- 対象機能
    SELECT /*PgmId*/'MC0001' AS conduct_id
    -- 共通機能を追加
    UNION
    SELECT VALUE AS conduct_id
    FROM cm_conduct cd
    CROSS APPLY STRING_SPLIT(cd.common_conduct_id, '|')
    WHERE 
        cd.conduct_id = /*PgmId*/'MC0001'
    AND cd.common_conduct_id is not null 
    AND cd.common_conduct_id != ''
)

-- コンボボックス用関連情報データ取得
-- 通常レイアウト
SELECT DISTINCT
     fcd.relation_id AS RELATIONID
    ,fcd.relation_parameters AS RELATIONPARAM
FROM cm_form_define fd
INNER JOIN cm_form_control_define fcd
ON  fd.program_id = fcd.program_id
AND fd.form_no = fcd.form_no
AND fd.control_group_id = fcd.control_group_id
INNER JOIN cm_control_define cd
ON fcd.control_id = cd.control_id
AND fcd.control_type = cd.control_type
LEFT JOIN cm_control_unused cu
ON fcd.location_structure_id = cu.location_structure_id
AND fcd.control_id = cu.control_id
AND fcd.control_type = cu.control_type

WHERE
    fd.program_id IN (SELECT conduct_id FROM TargetConduct)
AND fd.delete_flg != 1
AND fcd.delete_flg != 1
AND (fcd.control_type in ('0501', '0701')
    OR  (fcd.detailed_search_division != 0 and fcd.detailed_search_control_type = '0701')) -- 詳細検索条件用のコンボ
AND fcd.relation_id != '-'
AND (fcd.relation_parameters IS NULL OR fcd.relation_parameters NOT LIKE '%@%')  -- 連動コンボ以外
AND fd.program_id != '0'
AND cu.control_id IS NULL

UNION

-- 共通レイアウト
SELECT DISTINCT 
     fcd.relation_id AS RELATIONID
    ,fcd.relation_parameters AS RELATIONPARAM
FROM cm_form_define fd
INNER JOIN cm_form_control_define fcd
ON fd.common_form_no = fcd.form_no
INNER JOIN cm_control_define cd
ON fcd.control_id = cd.control_id
AND fcd.control_type = cd.control_type
LEFT JOIN cm_control_unused cu
ON fcd.location_structure_id = cu.location_structure_id
AND fcd.control_id = cu.control_id
AND fcd.control_type = cu.control_type

WHERE
    fd.program_id IN (SELECT conduct_id FROM TargetConduct)
AND fcd.control_group_id = 'CommonCtrl'
AND fcd.delete_flg != 1
AND (fcd.control_type in ('0501', '0701')
    OR  (fcd.detailed_search_division != 0 and fcd.detailed_search_control_type = '0701')) -- 詳細検索条件用のコンボ
AND fcd.relation_id != '-'
AND (fcd.relation_parameters IS NULL OR fcd.relation_parameters NOT LIKE '%@%')  -- 連動コンボ以外
AND cu.control_id IS NULL
