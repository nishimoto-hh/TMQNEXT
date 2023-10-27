SELECT
    re.machine_spec_relation_id,
    re.location_structure_id,
    re.job_structure_id,
    re.spec_id,
    re.display_order
FROM
    ms_machine_spec_relation re
WHERE
    re.spec_id = @SpecId
AND re.job_structure_id IS NOT NULL