SELECT COUNT(*)
FROM mc_loop_info
WHERE machine_id = @MachineId
AND loop_moto_id IS NOT NULL
 
 
