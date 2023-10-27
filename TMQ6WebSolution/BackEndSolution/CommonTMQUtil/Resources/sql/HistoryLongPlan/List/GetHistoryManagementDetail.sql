/*
* 変更管理の内容を取得するSQL
*/
SELECT
    history.history_management_id
    , hmlp.execution_division
    , history.key_id AS long_plan_id
    , NULL AS management_standards_content_id 
FROM
    hm_history_management history               -- 変更管理
    LEFT JOIN hm_ln_long_plan hmlp              -- 長計件名変更管理
        ON history.key_id = hmlp.long_plan_id 
        AND history.history_management_id = hmlp.history_management_id 
WHERE
    history.history_management_id = @HistoryManagementId 
UNION 
SELECT
    history.history_management_id
    , hmmsc.execution_division
    , history.key_id AS long_plan_id
    , hmmsc.management_standards_content_id 
FROM
    hm_history_management history               -- 変更管理
    LEFT JOIN hm_mc_management_standards_content hmmsc -- 機器別管理基準内容変更管理
        ON history.key_id = hmmsc.long_plan_id 
        AND history.history_management_id = hmmsc.history_management_id 
WHERE
    history.history_management_id = @HistoryManagementId
