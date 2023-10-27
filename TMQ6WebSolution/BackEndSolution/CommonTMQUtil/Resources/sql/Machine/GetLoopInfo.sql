SELECT COUNT(*)
FROM mc_loop_info
WHERE (machine_id = @MachineId AND loop_moto_id IS NOT NULL)
OR (loop_moto_id in (SELECT loop_id FROM mc_loop_info WHERE machine_id = @MachineId))
 