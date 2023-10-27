SELECT
    machine.hm_machine_id
FROM
    hm_history_management history
    LEFT JOIN
        hm_mc_machine machine
    ON  history.history_management_id = machine.history_management_id
WHERE
    history.history_management_id = @HistoryManagementId
AND machine.execution_division IN(1, 2)