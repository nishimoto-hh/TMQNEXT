UPDATE ma_plan 
SET
    [subject] = @Subject
    , [occurrence_date] = @OccurrenceDate
    , [expected_construction_date] = @ExpectedConstructionDate
    , [expected_completion_date] = @ExpectedCompletionDate
    , [total_budget_cost] = @TotalBudgetCost
    , [plan_man_hour] = @PlanManHour
    , [responsibility_structure_id] = @ResponsibilityStructureId
    , [failure_effect] = @FailureEffect
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
WHERE
    [plan_id] = @PlanId
