SELECT
    manager.family_name AS manager                                                              -- 課長
    , chief.family_name AS chief                                                                -- 係長
    , personnel.family_name AS person                                                           -- 担当
    , '''' + FORMAT(ma_request.issue_date, 'yyyy年MM月dd日') AS drafting                        -- 起票
    , '''' + FORMAT(ma_request.desired_start_date, 'yyyy年MM月dd日') AS construction            -- 着工
    , ma_summary.location_structure_id                                                          -- 機能場所階層id(工程取得用)
    , '' AS stroke_name                                                                         -- 工程
    , ma_summary.subject AS subject                                                             -- 件名
FROM
    ma_request                                                          -- 保全依頼
    INNER JOIN #temp temp
         ON ma_request.request_id = temp.Key1                           -- 依頼no
    LEFT JOIN ma_summary                                                -- 保全活動件名
        ON ma_request.summary_id = ma_summary.summary_id                -- 保全活動件名id
    LEFT JOIN ma_plan                                                   -- 保全計画
        ON ma_request.summary_id = ma_plan.summary_id                   -- 保全活動件名id
    LEFT JOIN ma_history                                                -- 保全履歴
        ON ma_request.summary_id = ma_history.summary_id                -- 保全活動件名id
    LEFT JOIN ms_user manager                                           -- 課長
        ON ma_request.request_department_manager_id = manager.user_id
    LEFT JOIN ms_user chief                                             -- 係長
        ON ma_request.request_department_chief_id = chief.user_id
    LEFT JOIN ms_user personnel                                         -- 担当
        ON ma_request.request_personnel_id = personnel.user_id

 
