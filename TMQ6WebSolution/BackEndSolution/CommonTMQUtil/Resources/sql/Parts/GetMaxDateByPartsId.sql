SELECT
    MAX(ac.update_datetime) AS max_update_datetime
FROM
    attachment ac
WHERE
    ac.key_id = @PartsId
AND
    ac.function_type_id IN @FunctionTypeId
