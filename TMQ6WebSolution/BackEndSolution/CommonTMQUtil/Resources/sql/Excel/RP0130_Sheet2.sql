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
    isnull(manager.family_name,ma_request.request_department_manager_name) AS manager               -- 課長
    , isnull(chief.family_name,request_department_chief_name) AS chief                              -- 係長
    , isnull(personnel.family_name,request_personnel_name) AS person                                -- 担当
    , isnull(foreman.family_name,request_department_foreman_name) AS foreman                        -- 職長
    , '''' + FORMAT(ma_request.issue_date, DateFormat.translation_text) AS drafting                        -- 起票
    , '''' + FORMAT(ma_request.desired_start_date, DateFormat.translation_text) AS construction            -- 着工
    , ma_summary.location_structure_id                                                          -- 機能場所階層id(工程取得用)
    , [dbo].[get_v_structure_item](ma_summary.location_stroke_structure_id, ma_summary.location_factory_structure_id, temp.languageId) AS stroke_name                            -- 工場
    , [dbo].[get_v_structure_item](ma_summary.location_factory_structure_id, ma_summary.location_factory_structure_id, temp.languageId) AS factory_name                          -- 工場
    , [dbo].[get_v_structure_item](ma_summary.location_plant_structure_id, ma_summary.location_factory_structure_id, temp.languageId) AS plant_name                              -- プラント
    , [dbo].[get_v_structure_item](ma_summary.location_series_structure_id, ma_summary.location_factory_structure_id, temp.languageId) AS series_name                            -- 系列
    , [dbo].[get_v_structure_item](ma_summary.location_facility_structure_id, ma_summary.location_stroke_structure_id, temp.languageId) AS facility_name                         -- 設備
    , ma_summary.subject AS subject                                                             -- 件名
FROM
    ma_summary                                                          -- 保全依頼
    INNER JOIN #temp temp
         ON ma_summary.summary_id = temp.Key1                           -- summary_id
    LEFT JOIN ma_request                                                -- 保全活動件名
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
 
