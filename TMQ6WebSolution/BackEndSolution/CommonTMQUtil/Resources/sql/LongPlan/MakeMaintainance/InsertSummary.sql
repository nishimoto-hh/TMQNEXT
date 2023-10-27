INSERT INTO ma_summary(
    summary_id,
    long_plan_id,
    activity_division,
    subject,
    subject_note,
    location_structure_id,
    job_structure_id,
    mq_class_structure_id,
    budget_management_structure_id,
    budget_personality_structure_id,
    update_serialid,
    insert_datetime,
    insert_user_id,
    update_datetime,
    update_user_id
) output inserted.summary_id
SELECT
    NEXT VALUE FOR seq_ma_summary_summary_id,
    lp.long_plan_id,
    1 AS activity_division, -- 点検
    @Subject,
    lp.subject_note,
    lp.location_structure_id,
    lp.job_structure_id,
    dbo.get_structure_by_exitem(lp.budget_personality_structure_id,1,1850,lp.location_structure_id), -- MQ分類 予算性格区分より拡張項目で逆引き
    lp.budget_management_structure_id,
    lp.budget_personality_structure_id,
    @UpdateSerialid,
    @InsertDatetime,
    @InsertUserId,
    @UpdateDatetime,
    @UpdateUserId
FROM
    ln_long_plan AS lp
WHERE
    lp.long_plan_id = @LongPlanId
