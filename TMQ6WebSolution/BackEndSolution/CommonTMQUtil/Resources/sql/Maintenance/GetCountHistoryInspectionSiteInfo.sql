SELECT
    COUNT(history_inspection_site_id)
FROM
    ma_history_inspection_site
WHERE
    history_machine_id = @HistoryMachineId
