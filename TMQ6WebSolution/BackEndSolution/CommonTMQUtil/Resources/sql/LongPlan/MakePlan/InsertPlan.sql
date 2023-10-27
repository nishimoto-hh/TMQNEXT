INSERT INTO ma_plan(
    plan_id,
    summary_id,
    subject,
    update_serialid,
    insert_datetime,
    insert_user_id,
    update_datetime,
    update_user_id
)
SELECT
    NEXT VALUE FOR seq_ma_plan_plan_id,
    @SummaryId,
    lp.subject,
    @UpdateSerialid,
    @InsertDatetime,
    @InsertUserId,
    @UpdateDatetime,
    @UpdateUserId
FROM
    ln_long_plan AS lp
WHERE
    lp.long_plan_id = @LongPlanId
