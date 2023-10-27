SELECT
    machine.equipment_level_structure_id
FROM
    mc_machine machine
WHERE
    machine.machine_id = @MachineId
