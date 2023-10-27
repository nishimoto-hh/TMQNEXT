SELECT
    COUNT(*)
FROM
    ms_machine_spec_relation re
WHERE
    spec_id = @SpecId
AND job_structure_id IS NOT NULL
AND machine_spec_relation_id != @MachineSpecRelationId