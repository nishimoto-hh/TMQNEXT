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
VALUES(
    NEXT VALUE FOR seq_ma_plan_plan_id,
    @SummaryId,
    @Subject,
    @UpdateSerialid,
    @InsertDatetime,
    @InsertUserId,
    @UpdateDatetime,
    @UpdateUserId
)
