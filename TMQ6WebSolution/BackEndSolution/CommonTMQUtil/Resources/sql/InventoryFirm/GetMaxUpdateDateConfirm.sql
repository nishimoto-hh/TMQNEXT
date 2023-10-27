SELECT
    max_date.max_update_datetime_confirm AS max_update_datetime
FROM
    (
        SELECT
            confirm.target_month,
            confirm.factory_id,
            confirm.parts_job_id,
            MAX(confirm.update_datetime) AS max_update_datetime_confirm
        FROM
            pt_stock_confirm confirm
        WHERE
            confirm.delete_flg = 0
        GROUP BY
            confirm.target_month,
            confirm.factory_id,
            confirm.parts_job_id
    ) max_date
WHERE
    FORMAT(max_date.target_month, 'yyyy/MM') = @TargetMonth
AND max_date.factory_id = @FactoryId
AND COALESCE(max_date.parts_job_id, 0) = COALESCE(@PartsJobId, 0)
