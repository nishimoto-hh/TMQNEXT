WITH TraExists AS(-- 「有」の翻訳を取得
    SELECT
        tra.translation_text
    FROM
        ms_translation tra
    WHERE
        tra.location_structure_id = 0
    AND tra.translation_id = 131010001
    AND tra.language_id = (SELECT DISTINCT languageId FROM #temp)
)
, TraNotExists AS (-- 「無」の翻訳を取得
    SELECT
        tra.translation_text
    FROM
        ms_translation tra
    WHERE
        tra.location_structure_id = 0
    AND tra.translation_id = 131210002
    AND tra.language_id = (SELECT DISTINCT languageId FROM #temp)
)
SELECT
    sm.location_structure_id,                           -- 機能場所階層ID
    mc.job_structure_id AS job_structure_id2,           -- 職種機種階層ID(機器)
	mc.location_structure_id AS location_structure_id2, -- 機能場所階層ID(機器)
    [dbo].[get_v_structure_item](mc.importance_structure_id, mc.location_factory_structure_id, tmp.languageId) AS importance_name,      -- 重要度
    sm.subject,                          -- 故障件名
    (SELECT family_name from ms_user where hi.construction_personnel_id = user_id) AS  construction,  -- 対策実施
    (SELECT family_name from ms_user where rq.request_department_manager_id = user_id) AS  manager,   -- 課長
    (SELECT family_name from ms_user where rq.request_department_chief_id = user_id) AS  chief,       -- 係長
    (SELECT family_name from ms_user where rq.request_department_foreman_id = user_id) AS  foreman,   -- 職長
    (SELECT family_name from ms_user where rq.request_personnel_id = user_id) AS  person_name,        -- 担当

    SUBSTRING(hf.failure_status,   1, 20) AS failure_status_1,                            -- 故障状況１
    SUBSTRING(hf.failure_status,  21, 20) AS failure_status_2,                            -- 故障状況２
    SUBSTRING(hf.failure_status,  41, 20) AS failure_status_3,                            -- 故障状況３
    SUBSTRING(hf.failure_status,  61, 20) AS failure_status_4,                            -- 故障状況４
    SUBSTRING(hf.failure_status,  81, 20) AS failure_status_5,                            -- 故障状況５
    SUBSTRING(hf.failure_status, 101, 20) AS failure_status_6,                            -- 故障状況６
    SUBSTRING(hf.failure_status, 121, 20) AS failure_status_7,                            -- 故障状況７
    SUBSTRING(hf.failure_status, 141, 20) AS failure_status_8,                            -- 故障状況８
    hf.failure_status AS failure_status,                                                  -- 故障状況

    SUBSTRING(hf.failure_cause_addition_note,   1, 20) AS failure_cause_addition_note_1,  -- 故障原因１
    SUBSTRING(hf.failure_cause_addition_note,  21, 20) AS failure_cause_addition_note_2,  -- 故障原因２
    SUBSTRING(hf.failure_cause_addition_note,  41, 20) AS failure_cause_addition_note_3,  -- 故障原因３
    SUBSTRING(hf.failure_cause_addition_note,  61, 20) AS failure_cause_addition_note_4,  -- 故障原因４
    SUBSTRING(hf.failure_cause_addition_note,  81, 20) AS failure_cause_addition_note_5,  -- 故障原因５
    SUBSTRING(hf.failure_cause_addition_note, 101, 20) AS failure_cause_addition_note_6,  -- 故障原因６
    SUBSTRING(hf.failure_cause_addition_note, 121, 20) AS failure_cause_addition_note_7,  -- 故障原因７
    SUBSTRING(hf.failure_cause_addition_note, 141, 20) AS failure_cause_addition_note_8,  -- 故障原因８
    hf.failure_cause_addition_note AS failure_cause_addition_note_all,                        -- 故障原因

    SUBSTRING(hf.previous_situation,   1, 20) AS previous_situation_1,                    -- 故障前の保全実施状況１
    SUBSTRING(hf.previous_situation,  21, 20) AS previous_situation_2,                    -- 故障前の保全実施状況２
    SUBSTRING(hf.previous_situation,  41, 20) AS previous_situation_3,                    -- 故障前の保全実施状況３
    SUBSTRING(hf.previous_situation,  61, 20) AS previous_situation_4,                    -- 故障前の保全実施状況４
    SUBSTRING(hf.previous_situation,  81, 20) AS previous_situation_5,                    -- 故障前の保全実施状況５
    SUBSTRING(hf.previous_situation, 101, 20) AS previous_situation_6,                    -- 故障前の保全実施状況６
    hf.previous_situation AS previous_situation,                                          -- 故障前の保全実施状況

    SUBSTRING(hf.recovery_action,   1, 20) AS recovery_action_1,                          -- 復旧処置１
    SUBSTRING(hf.recovery_action,  21, 20) AS recovery_action_2,                          -- 復旧処置２
    SUBSTRING(hf.recovery_action,  41, 20) AS recovery_action_3,                          -- 復旧処置３
    SUBSTRING(hf.recovery_action,  61, 20) AS recovery_action_4,                          -- 復旧処置４
    SUBSTRING(hf.recovery_action,  81, 20) AS recovery_action_5,                          -- 復旧処置５
    SUBSTRING(hf.recovery_action, 101, 20) AS recovery_action_6,                          -- 復旧処置６
    SUBSTRING(hf.recovery_action, 121, 20) AS recovery_action_7,                          -- 復旧処置７
    SUBSTRING(hf.recovery_action, 141, 20) AS recovery_action_8,                          -- 復旧処置８
    hf.recovery_action AS recovery_action,                                                -- 復旧処置

    SUBSTRING(hf.improvement_measure,   1, 20) AS improvement_measure_1,                  -- 改善対策１
    SUBSTRING(hf.improvement_measure,  21, 20) AS improvement_measure_2,                  -- 改善対策２
    SUBSTRING(hf.improvement_measure,  41, 20) AS improvement_measure_3,                  -- 改善対策３
    SUBSTRING(hf.improvement_measure,  61, 20) AS improvement_measure_4,                  -- 改善対策４
    SUBSTRING(hf.improvement_measure,  81, 20) AS improvement_measure_5,                  -- 改善対策５
    SUBSTRING(hf.improvement_measure, 101, 20) AS improvement_measure_6,                  -- 改善対策６
    SUBSTRING(hf.improvement_measure, 121, 20) AS improvement_measure_7,                  -- 改善対策７
    SUBSTRING(hf.improvement_measure, 141, 20) AS improvement_measure_8,                  -- 改善対策８
    hf.improvement_measure AS improvement_measure,                                        -- 改善対策

    SUBSTRING(hf.system_feed_back,   1, 20) AS system_feed_back_1,                        -- 保全システムへのフィードバック１
    SUBSTRING(hf.system_feed_back,  21, 20) AS system_feed_back_2,                        -- 保全システムへのフィードバック２
    SUBSTRING(hf.system_feed_back,  41, 20) AS system_feed_back_3,                        -- 保全システムへのフィードバック３
    hf.system_feed_back AS system_feed_back,                                              -- 保全システムへのフィードバック

    hf.lesson,                         -- 教訓
    mc.date_of_installation,           -- 設置年月
    sm.stop_time,                      -- 停止時間
    hi.expenditure,                    -- 作業金額
    hi.total_working_time,             -- 作業時間
    hi.working_time_self,              -- 自社時間
    hi.working_time_company,           -- 業者時間
    mc.machine_no,                     -- 機器番号
    mc.machine_name,                   -- 機器名称
    pl.occurrence_date,                -- 発生日
    sm.completion_date,                -- 完了日
    CASE WHEN ISNULL(hi.stop_count, 0) >= 1 THEN TraExists.translation_text ELSE TraNotExists.translation_text END AS stop_count_name,  -- P停
    CASE WHEN ISNULL(hi.call_count, 0) >= 1 THEN TraExists.translation_text ELSE TraNotExists.translation_text END AS call_count_name,  -- 呼出          
    [dbo].[get_v_structure_item](rq.discovery_methods_structure_id, sm.location_factory_structure_id, tmp.languageId) AS discovery_methods_name,                  -- 発見方法
    [dbo].[get_v_structure_item](phenomenon_structure_id, sm.location_factory_structure_id, tmp.languageId) AS phenomenon_name,                                   -- 現象
    [dbo].[get_v_structure_item](hf.failure_cause_structure_id, sm.location_factory_structure_id, tmp.languageId) AS failure_cause_addition_note,                 -- 故障原因
    [dbo].[get_v_structure_item](hf.failure_cause_personality_structure_id, sm.location_factory_structure_id, tmp.languageId) AS failure_cause_personality_note,  -- 原因性格
	[dbo].[get_v_structure_item](hf.treatment_measure_structure_id, sm.location_factory_structure_id, tmp.languageId) AS treatment_measure_name,                  -- 処置・対策   
    
    SUBSTRING(hf.failure_note,   1, 12) AS failure_note_1,                        -- 特記(メモ)１
    SUBSTRING(hf.failure_note,  13, 12) AS failure_note_2,                        -- 特記(メモ)２
    hf.failure_note AS failure_note,                                              -- 特記(メモ)
    
    '' AS factory_name2,               -- 工場名(機器)
    '' AS job_name2,                   -- 職種名(機器)
    '' AS large_classfication_name2,   -- 大分類(機器)
    '' AS middle_classfication_name2,  -- 中分類(機器)
    '' AS small_classfication_name2,   -- 小分類(機器)

    dbo.get_file_download_info(1660, hf.history_failure_id) AS file_diagram -- 略図

FROM
    ma_history_failure hf
    INNER JOIN ma_history hi
    ON  hf.history_id = hi.history_id
    INNER JOIN ma_summary sm
    ON  hi.summary_id = sm.summary_id
    LEFT JOIN mc_machine mc
--    INNER JOIN mc_machine mc
    ON  mc.machine_id = hf.machine_id
    INNER JOIN ma_request rq
    ON rq.summary_id = sm.summary_id
    LEFT JOIN ma_plan pl
    ON pl.summary_id = sm.summary_id,
    (SELECT TOP 1 * FROM #temp) tmp
    CROSS JOIN
       TraExists --「有」の翻訳
    CROSS JOIN
       TraNotExists --「無」の翻訳
WHERE
    hi.summary_id = tmp.Key1

