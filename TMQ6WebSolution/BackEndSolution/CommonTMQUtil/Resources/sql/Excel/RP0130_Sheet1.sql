WITH DateFormat AS (--「yyyy年MM月dd日」の翻訳を取得
    SELECT
        tra.translation_text
    FROM
        ms_translation tra
    WHERE
        tra.location_structure_id = 0
    AND tra.translation_id = 150000014
    AND tra.language_id = (SELECT DISTINCT languageId FROM #temp)
)
SELECT
    manager.family_name AS manager                                                                  -- 課長
    , chief.family_name AS chief                                                                    -- 係長
    , personnel.family_name AS person                                                               -- 担当
    , foreman.family_name AS foreman                                                                -- 職長
    , FORMAT(ma_request.issue_date, DateFormat.translation_text) AS drafting                                   -- 起票（マスタのデータタイプが文字列なのでそのまま）
    , '''' + FORMAT(ma_request.desired_start_date, DateFormat.translation_text) AS construction                -- 着工
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
    , '''' + FORMAT(ma_plan.expected_construction_date, DateFormat.translation_text) AS expected_construction  -- 着工予定
    , '''' + FORMAT(ma_plan.expected_completion_date, DateFormat.translation_text) AS expected_completion      -- 完了予定    
    , SUBSTRING(ma_history.maintenance_opinion,  1, 40) AS final_report_1                           -- 完了報告１
    , SUBSTRING(ma_history.maintenance_opinion, 41, 40) AS final_report_2                           -- 完了報告２
    , SUBSTRING(ma_history.maintenance_opinion, 81, 40) AS final_report_3                           -- 完了報告３
    , ma_request.request_content AS request_content                                                 -- 依頼内容
    , ma_summary.plan_implementation_content AS work_plan                                           -- 作業計画
    , ma_history.maintenance_opinion AS final_report                                                -- 完了報告
    , [dbo].[get_v_structure_item](ma_summary.change_management_structure_id, ma_summary.location_factory_structure_id, temp.languageId) AS change_management_name               -- 変更管理(翻訳はデータの工場)
    , [dbo].[get_v_structure_item](ma_summary.env_safety_management_structure_id, ma_summary.location_factory_structure_id, temp.languageId) AS env_safety_management_name       -- 環境安全管理区分(翻訳はデータの工場)
    , [dbo].[get_v_structure_item](ma_request.urgency_structure_id, ma_summary.location_factory_structure_id, temp.languageId) AS urgency_name                                   -- 緊急度(翻訳はデータの工場)
    , [dbo].[get_v_structure_item](ma_request.discovery_methods_structure_id, ma_summary.location_factory_structure_id, temp.languageId) AS discovery_methods_name               -- 発見方法(翻訳はデータの工場)
    , [dbo].[get_v_structure_item](ma_request.request_department_clerk_id, ma_summary.location_factory_structure_id, temp.languageId) AS request_department_clerk_name           -- 依頼部課係(翻訳はデータの工場)
    , [dbo].[get_v_structure_item](ma_request.maintenance_department_clerk_id, ma_summary.location_factory_structure_id, temp.languageId) AS maintenance_department_clerk_name   -- 保全部課係(翻訳はデータの工場)
    , '' AS factory_name    -- 工場
    , '' AS plant_name      -- プラント
    , '' AS series_name     -- 系列
    , '' AS facility_name   -- 設備

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
    LEFT JOIN ms_user foreman                                           -- 職長
        ON ma_request.request_department_foreman_id = foreman.user_id
    CROSS JOIN
       DateFormat -- 「yyyy年MM月dd日」の翻訳