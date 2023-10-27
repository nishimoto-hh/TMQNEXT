SELECT COUNT(*)
FROM mc_machine_use_parts
WHERE parts_id = @PartsId
AND machine_id = @MachineId
AND machine_use_parts_id <> @MachineUsePartsId
