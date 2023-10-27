UPDATE ma_history_inspection_site 
SET
    [history_machine_id] = @HistoryMachineId
    , [inspection_site_structure_id] = @InspectionSiteStructureId
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
WHERE
    [history_inspection_site_id] = @HistoryInspectionSiteId
