INSERT INTO ma_history_inspection_site(
    [history_inspection_site_id]
    ,[history_machine_id]
    ,[inspection_site_structure_id]
    ,[update_serialid]
    ,[insert_datetime]
    ,[insert_user_id]
    ,[update_datetime]
    ,[update_user_id]
) OUTPUT inserted.history_inspection_site_id
VALUES(
    NEXT VALUE FOR seq_ma_history_inspection_site_history_inspection_site_id
    ,@HistoryMachineId
    ,@InspectionSiteStructureId
    ,@UpdateSerialid
    ,@InsertDatetime
    ,@InsertUserId
    ,@UpdateDatetime
    ,@UpdateUserId
)
