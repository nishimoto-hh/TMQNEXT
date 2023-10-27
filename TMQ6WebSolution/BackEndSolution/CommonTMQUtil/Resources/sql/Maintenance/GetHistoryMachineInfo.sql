SELECT
    history_machine_id
FROM
    ma_history_machine
WHERE
    history_id = @HistoryId
AND machine_id = @MachineId
AND equipment_id = @EquipmentId
