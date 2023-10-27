INSERT INTO ma_history(
    history_id,
    summary_id,
    maintenance_season_structure_id,
    update_serialid,
    insert_datetime,
    insert_user_id,
    update_datetime,
    update_user_id
) output inserted.history_id
SELECT
    NEXT VALUE FOR seq_ma_history_history_id,
    @SummaryId,
    lp.maintenance_season_structure_id,
    @UpdateSerialid,
    @InsertDatetime,
    @InsertUserId,
    @UpdateDatetime,
    @UpdateUserId
FROM
    ln_long_plan AS lp
WHERE
    lp.long_plan_id = @LongPlanId
