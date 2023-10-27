SELECT
    count(parts_id)
FROM
    pt_parts
WHERE
    parts_no = @PartsNo
