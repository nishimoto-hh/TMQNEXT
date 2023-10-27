SELECT
    plan_id                                   -- 保全計画id
    , subject                                 -- 件名
    , occurrence_date                         -- 発生日
    , expected_construction_date              -- 着工予定日
    , expected_completion_date                -- 完了予定日
    , total_budget_cost                       -- 全体予算金額
    , plan_man_hour                           -- 予定工数
    , responsibility_structure_id             -- 自・他責id
    , failure_effect                          -- 故障影響id
    , update_serialid                         -- 更新シリアルID
FROM
    ma_plan
WHERE
    summary_id = @SummaryId
