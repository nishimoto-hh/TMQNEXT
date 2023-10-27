SELECT
    MAX(target_month) 
FROM
    pt_fixed_stock 
WHERE
    parts_id = @PartsId
