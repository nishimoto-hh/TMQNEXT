INSERT INTO ma_history(
    history_id,
    summary_id,
    maintenance_season_structure_id,
    construction_personnel_id,
    construction_personnel_name,
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
    @ConstructionPersonnelId,
    (
        SELECT
            mu.display_name
        FROM
            ms_user AS mu
        WHERE
            mu.user_id = @ConstructionPersonnelId
    ),
    @UpdateSerialid,
    @InsertDatetime,
    @InsertUserId,
    @UpdateDatetime,
    @UpdateUserId
FROM
    ln_long_plan AS lp
WHERE
    lp.long_plan_id = @LongPlanId
