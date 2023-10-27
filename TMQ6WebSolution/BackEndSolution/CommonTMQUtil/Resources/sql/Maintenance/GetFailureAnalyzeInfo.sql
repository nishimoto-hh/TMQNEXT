SELECT
    hf.history_failure_id               -- 保全履歴故障情報id
    , hf.failure_status                 -- 故障状況
    , hf.failure_cause_addition_note    -- 故障原因補足
    , hf.previous_situation             -- 故障前の保全実施状況
    , hf.recovery_action                -- 復旧処置
    , hf.improvement_measure            -- 改善対策
    , hf.system_feed_back               -- 保全システムへのフィードバック
    , hf.lesson                         -- 教訓
    , hf.failure_note                   -- 特記(メモ)
    , dbo.get_file_download_info(1660, hf.history_failure_id) AS file_link_diagram -- 略図
    , dbo.get_file_download_info(1670, hf.history_failure_id) AS file_link_failure -- 故障原因分析書
    , hf.update_serialid                --更新シリアルID
FROM
    ma_history_failure hf
    INNER JOIN ma_history hi
    ON  hf.history_id = hi.history_id
WHERE
    hi.summary_id = @SummaryId
