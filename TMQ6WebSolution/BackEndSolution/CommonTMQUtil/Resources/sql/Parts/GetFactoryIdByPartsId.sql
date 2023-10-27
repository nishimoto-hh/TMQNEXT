SELECT
    parts.factory_id
FROM
    pt_parts parts
WHERE
    parts.parts_id = @PartsId
