SELECT COUNT(*)
FROM mc_machine_parent_info
WHERE machine_id = @MachineId
AND parent_moto_id IS NOT NULL
 
 
