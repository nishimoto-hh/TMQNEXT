SELECT
    hf.maintenance_site                         --保全部位
    , hf.maintenance_content                    --保全内容
    , hf.follow_flg                             --フォロー有無
    , hf.follow_plan_date                       --フォロー予定年月
    , hf.follow_content                         --フォロー内容
    , hf.follow_completion_date                 --フォロー完了日
    , hf.phenomenon_structure_id                --現象
    , hf.phenomenon_note                        --現象メモ
    , hf.failure_cause_structure_id             --原因
    , hf.failure_cause_note                     --原因メモ
    , hf.failure_cause_personality_structure_id --原因性格1、原因性格2
    , hf.failure_cause_personality_note         --原因性格メモ
    , hf.treatment_measure_structure_id         --処置対策
    , hf.treatment_measure_note                 --処置対策メモ
    , hf.history_failure_id                     --保全履歴故障情報ID
    , hf.update_serialid                        --更新シリアルID
FROM
    ma_history hi
    LEFT JOIN ma_history_failure hf
    ON  hi.history_id = hf.history_id
WHERE
    summary_id = @SummaryId
