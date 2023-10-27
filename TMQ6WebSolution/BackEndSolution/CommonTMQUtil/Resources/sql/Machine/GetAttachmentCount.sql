SELECT
    COUNT(*)
FROM
    attachment
WHERE
    function_type_id = @FunctionTypeId
AND key_id = @KeyId
