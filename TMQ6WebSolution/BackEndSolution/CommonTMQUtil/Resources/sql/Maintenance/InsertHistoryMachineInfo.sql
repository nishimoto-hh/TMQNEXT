INSERT INTO ma_history_machine(
    [history_machine_id]
    ,[history_id]
    ,[machine_id]
    ,[equipment_id]
    ,[used_days_machine]
    ,[update_serialid]
    ,[insert_datetime]
    ,[insert_user_id]
    ,[update_datetime]
    ,[update_user_id]
) OUTPUT inserted.history_machine_id
VALUES(
    NEXT VALUE FOR seq_ma_history_machine_history_machine_id
    ,@HistoryId
    ,@MachineId
    ,@EquipmentId
    ,@UsedDaysMachine
    ,@UpdateSerialid
    ,@InsertDatetime
    ,@InsertUserId
    ,@UpdateDatetime
    ,@UpdateUserId
)
