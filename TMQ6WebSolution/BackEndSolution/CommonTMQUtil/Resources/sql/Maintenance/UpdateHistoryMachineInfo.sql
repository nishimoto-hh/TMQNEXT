UPDATE ma_history_machine 
SET
    [used_days_machine] = @UsedDaysMachine
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
WHERE
    [history_machine_id] = @HistoryMachineId
