SELECT
    machine.location_structure_id,
    machine.job_structure_id
FROM
    mc_machine machine
WHERE
    machine.machine_id = @MachineId
