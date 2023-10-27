WITH factory_job AS( -- 工場IDと職種IDの組み合わせ
SELECT DISTINCT
    factory.factory_id,
    factory.job_id
FROM
    (
        SELECT
            factory.factory_id,
            job.job_id
        FROM
            (
                SELECT
                    ms.structure_id AS factory_id
                FROM
                    ms_structure ms
                WHERE
                    ms.structure_group_id = 1000
                AND ms.structure_layer_no = 1
                AND EXISTS(
                 SELECT * 
                 FROM #temp_location temp
                 WHERE ms.structure_id = temp.structure_id
               )
            ) factory
            INNER JOIN
                (
                    SELECT
                        ms.factory_id,
                        ms.structure_id AS job_id
                    FROM
                        ms_structure ms
                    WHERE
                        ms.structure_group_id = 1010
                    AND ms.structure_layer_no = 0
                    AND EXISTS(
                     SELECT * 
                     FROM #temp_job temp
                     WHERE ms.structure_id = temp.structure_id
                    )
                ) job
            ON  factory.factory_id = job.factory_id
        UNION
        SELECT
            factory.factory_id,
            job.job_id
        FROM
            (
                SELECT
                    ms.structure_id AS factory_id
                FROM
                    ms_structure ms
                WHERE
                    ms.structure_group_id = 1000
                AND ms.structure_layer_no = 1
                AND EXISTS(
                 SELECT * 
                 FROM #temp_location temp
                 WHERE ms.structure_id = temp.structure_id
                )
            ) factory,
            (
                SELECT
                    ms.structure_id AS job_id
                FROM
                    ms_structure ms
                WHERE
                    ms.structure_group_id = 1010
                AND ms.structure_layer_no = 0
                AND ms.factory_id = 0
                AND EXISTS(
                     SELECT * 
                     FROM #temp_job temp
                     WHERE ms.structure_id = temp.structure_id
                    )
            ) job
    ) factory
)
SELECT
    COUNT(*)
FROM
    factory_job
