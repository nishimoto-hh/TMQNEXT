FROM mc_machine mc
LEFT JOIN mc_equipment eq
ON mc.machine_id = eq.machine_id
WHERE mc.machine_id = @MachineId 
 
