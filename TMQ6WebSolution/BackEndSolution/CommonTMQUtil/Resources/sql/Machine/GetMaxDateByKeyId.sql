SELECT
    MAX(ac.update_datetime) AS max_update_datetime
FROM
    attachment ac
WHERE
    function_type_id = @FunctionTypeId
AND key_id = @KeyId
