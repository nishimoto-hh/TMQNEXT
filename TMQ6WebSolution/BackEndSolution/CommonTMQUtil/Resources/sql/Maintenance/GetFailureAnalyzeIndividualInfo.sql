SELECT
    hf.history_failure_id                         -- 保全履歴故障情報id
    , hf.failure_status                           -- 故障状況
    , hf.failure_analysis_structure_id            -- 故障分析
    , hf.failure_personality_factor_structure_id  -- 故障性格要因
    , hf.failure_personality_class_structure_id   -- 故障性格分類
    , hf.failure_cause_addition_note              -- 故障原因
    , hf.previous_situation                       -- 故障前の保全実施状況
    , hf.treatment_status_structure_id            -- 完了/応急
    , hf.recovery_action                          -- 復旧処置
    , hf.necessity_measure_structure_id           -- 要/否
    , hf.measure_plan_date                        -- 実施予定日
    , hf.measure_class1_structure_id              -- 対策分類Ⅰ
    , hf.measure_class2_structure_id              -- 対策分類Ⅱ
    , hf.improvement_measure                      -- 再発防止対策
    , hf.lesson                                   -- 教訓
    , hf.failure_note                             -- 特記(メモ)
    , dbo.get_file_download_info(1680, hf.history_failure_id) AS file_link_diagram -- 略図
    , dbo.get_file_download_info(1690, hf.history_failure_id) AS file_link_failure -- 故障原因分析書
    , hf.update_serialid                          --更新シリアルID
FROM
    ma_history_failure hf
    INNER JOIN ma_history hi 
        ON hf.history_id = hi.history_id 
WHERE
    hi.summary_id = @SummaryId
