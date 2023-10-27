SELECT
    COUNT(*)
FROM
    pt_parts
WHERE
    EXISTS(
        SELECT
            *
        FROM
            #temp_location temp
        WHERE
            factory_id = temp.structure_id
    )
AND EXISTS(
        SELECT
            *
        FROM
            #temp_job temp
        WHERE
            job_structure_id = temp.structure_id
    )