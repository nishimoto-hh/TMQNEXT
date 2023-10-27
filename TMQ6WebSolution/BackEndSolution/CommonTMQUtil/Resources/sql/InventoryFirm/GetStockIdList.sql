SELECT
    stock.stock_id
FROM
    pt_fixed_stock stock
    LEFT JOIN
        pt_parts parts
    ON  stock.parts_id = parts.parts_id
WHERE
    format(stock.target_month, 'yyyy/MM') = @TargetMonth
AND parts.factory_id = @FactoryId
AND parts.job_structure_id = @PartsJobId
