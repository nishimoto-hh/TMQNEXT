SELECT
    ms.location_structure_id                            -- 機種階層id ※共通処理にて使用
    , '' AS factory_name                                -- 工場 ※共通処理にて設定
    , '' AS plant_name                                  -- プラント ※共通処理にて設定
    , '' AS series_name                                 -- 系列 ※共通処理にて設定
    , '' AS stroke_name                                 -- 工程 ※共通処理にて設定
    , '' AS facility_name                               -- 設備 ※共通処理にて設定
    , ms.job_structure_id                               -- 職種機種階層id ※共通処理にて使用
    , '' AS job_name                                    -- 職種
    , ms.subject                                        -- 件名
--    , mh.work_failure_division_structure_id             -- 作業目的
--    , vwkp.translation_text AS work_purpose_name        -- 作業目的名称
--    , mh.maintenance_season_structure_id                -- 保全時期
--    , vmts.translation_text AS work_purpose_name        -- 保全時期名称
    , [dbo].[get_v_structure_item](mh.work_failure_division_structure_id, temp.factoryId, temp.languageId) AS work_purpose_name         -- 作業目的名称
    , [dbo].[get_v_structure_item](mh.maintenance_season_structure_id, temp.factoryId, temp.languageId) AS maintenance_season_name      -- 保全時期名称
    , CASE
        WHEN ms.completion_date IS NOT NULL THEN '''' + FORMAT(ms.completion_date, 'yyyy/MM/dd') 
        ELSE ''
    END AS completion_date                              -- 完了年月日
    , CASE
        WHEN mp.total_budget_cost IS NOT NULL THEN FORMAT(mp.total_budget_cost, '#,##0.###') 
        ELSE ''
    END AS budget_amount                                -- 予算金額
    , CASE
        WHEN mh.expenditure IS NOT NULL THEN FORMAT(mh.expenditure, '#,##0.###') 
        ELSE ''
    END AS actual_amount                                -- 実績金額
--    , ms.sudden_division_structure_id                   -- 突発区分
--    , vsdv.translation_text AS sudden_division_name     -- 突発区分名称
--    , ms.mq_class_structure_id                          -- MQ分類
--    , vmqc.translation_text AS mq_class_name            -- MQ分類名称
--    , ms.budget_management_structure_id                 -- 予算管理区分
--    , vbdm.translation_text AS budget_management_name   -- 予算管理区分名称
--    , ms.budget_personality_structure_id                -- 予算性格区分
--    , vbdp.translation_text AS budget_personality_name  -- 予算性格区分名称
    , [dbo].[get_v_structure_item](ms.sudden_division_structure_id, temp.factoryId, temp.languageId) AS sudden_division_name     -- 突発区分名称
    , [dbo].[get_v_structure_item](ms.mq_class_structure_id, temp.factoryId, temp.languageId) AS mq_class_name            -- MQ分類名称
    , [dbo].[get_v_structure_item](ms.budget_management_structure_id, temp.factoryId, temp.languageId) AS budget_management_name   -- 予算管理区分名称
    , [dbo].[get_v_structure_item](ms.budget_personality_structure_id, temp.factoryId, temp.languageId) AS budget_personality_name  -- 予算性格区分名称
FROM
    ma_summary ms                       -- 保全活動件名
    INNER JOIN #temp temp
        ON ms.summary_id = temp.Key1
    LEFT JOIN ma_history mh             -- 保全履歴
        ON ms.summary_id = mh.summary_id
    LEFT JOIN ma_plan mp                -- 保全計画
        ON ms.summary_id = mp.summary_id
--    LEFT JOIN v_structure_item vwkp     -- 作業目的
--        ON mh.work_failure_division_structure_id = vwkp.structure_id
--    LEFT JOIN v_structure_item vmts     -- 保全時期
--        ON mh.maintenance_season_structure_id = vmts.structure_id
--    LEFT JOIN v_structure_item vsdv     -- 突発区分
--        ON ms.sudden_division_structure_id = vsdv.structure_id
--    LEFT JOIN v_structure_item vmqc     -- MQ分類
--        ON ms.mq_class_structure_id = vmqc.structure_id
--    LEFT JOIN v_structure_item vbdm     -- 予算管理区分
--        ON ms.budget_management_structure_id = vbdm.structure_id
--    LEFT JOIN v_structure_item vbdp     -- 予算性格区分
--        ON ms.budget_personality_structure_id = vbdp.structure_id
ORDER BY
    mp.occurrence_date DESC                   -- 発生日
    , ms.summary_id DESC                      -- 保全活動件名ID

