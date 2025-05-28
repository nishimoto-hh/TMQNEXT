SELECT
    ( 
        SELECT
            count(*) 
        FROM
            ma_history_machine 
        WHERE
            machine_id = @MachineId
    ) + ( 
        SELECT
            count(*) 
        FROM
            ma_history_failure 
        WHERE
            machine_id = @MachineId
    )
