SELECT
    manager.family_name AS manager                                                                  -- 課長
    , chief.family_name AS chief                                                                    -- 係長
    , personnel.family_name AS person                                                               -- 担当
    , FORMAT(ma_request.issue_date, 'yyyy年MM月dd日') AS drafting                                   -- 起票（マスタのデータタイプが文字列なのでそのまま）
    , '''' + FORMAT(ma_request.desired_start_date, 'yyyy年MM月dd日') AS construction                -- 着工
    , ma_summary.location_structure_id                                                              -- 機能場所階層id(工程取得用)
    , '' AS stroke_name                                                                             -- 工程
    , ma_summary.subject AS subject                                                                 -- 件名
    , SUBSTRING(ma_request.request_content,  1, 50) AS request_content_1                            -- 依頼内容１
    , SUBSTRING(ma_request.request_content, 51,50) AS request_content_2                            -- 依頼内容２
    , SUBSTRING(ma_request.request_content,101,50) AS request_content_3                            -- 依頼内容３
    , SUBSTRING(ma_request.request_content,151,50) AS request_content_4                            -- 依頼内容４
    , SUBSTRING(ma_request.request_content,201,50) AS request_content_5                            -- 依頼内容５
    , SUBSTRING(ma_request.request_content,251,50) AS request_content_6                            -- 依頼内容６
    , SUBSTRING(ma_summary.plan_implementation_content,  1, 50) AS work_plan_1                      -- 作業計画１
    , SUBSTRING(ma_summary.plan_implementation_content, 51,50) AS work_plan_2                      -- 作業計画２
    , SUBSTRING(ma_summary.plan_implementation_content,101,50) AS work_plan_3                      -- 作業計画３
    , SUBSTRING(ma_summary.plan_implementation_content,151,50) AS work_plan_4                      -- 作業計画４
    , SUBSTRING(ma_summary.plan_implementation_content,201,50) AS work_plan_5                      -- 作業計画５
    , SUBSTRING(ma_summary.plan_implementation_content,251,50) AS work_plan_6                      -- 作業計画６
    , '''' + FORMAT(ma_plan.expected_construction_date, 'yyyy年MM月dd日') AS expected_construction  -- 着工予定
    , '''' + FORMAT(ma_plan.expected_completion_date, 'yyyy年MM月dd日') AS expected_completion      -- 完了予定    
    , SUBSTRING(ma_history.maintenance_opinion,  1, 40) AS final_report_1                           -- 完了報告１
    , SUBSTRING(ma_history.maintenance_opinion, 41, 40) AS final_report_2                           -- 完了報告２
    , SUBSTRING(ma_history.maintenance_opinion, 81, 40) AS final_report_3                           -- 完了報告３
    , ma_request.request_content AS request_content                                                 -- 依頼内容
    , ma_summary.plan_implementation_content AS work_plan                                           -- 作業計画
    , ma_history.maintenance_opinion AS final_report                                                -- 完了報告
FROM
    ma_request                                                          -- 保全依頼
    INNER JOIN #temp temp
         ON ma_request.request_id = temp.Key1                           -- 依頼NO
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
