DELETE ac
FROM mc_mp_information mp
    ,attachment ac
WHERE mp.mp_information_id = ac.key_id
AND ac.function_type_id = 1630
AND mp.machine_id = @MachineId
