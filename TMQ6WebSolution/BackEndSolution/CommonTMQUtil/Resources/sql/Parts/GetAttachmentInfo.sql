SELECT
    COUNT(at.attachment_id)
FROM
    attachment at
WHERE
    at.key_id = @PartsId
AND
    at.function_type_id = @FunctionTypeId
