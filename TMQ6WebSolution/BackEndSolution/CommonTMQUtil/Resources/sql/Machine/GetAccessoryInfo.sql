SELECT COUNT(*)
FROM mc_accessory_info
WHERE machine_id = @MachineId
AND accessory_moto_id is not null
 
 
