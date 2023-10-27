SELECT
    max_date.max_update_datetime_fixed AS max_update_datetime
FROM
    (
        SELECT
            parts.factory_id,
            parts.job_structure_id,
            fixed.target_month,
            MAX(fixed.update_datetime) AS max_update_datetime_fixed
        FROM
            pt_fixed_stock fixed
            LEFT JOIN
                pt_parts parts
            ON  fixed.parts_id = parts.parts_id
        GROUP BY
            parts.factory_id,
            parts.job_structure_id,
            fixed.target_month
    ) max_date
WHERE
    format(max_date.target_month, 'yyyy/MM') = @TargetMonth
AND max_date.factory_id = @FactoryId
AND max_date.job_structure_id = @PartsJobId
