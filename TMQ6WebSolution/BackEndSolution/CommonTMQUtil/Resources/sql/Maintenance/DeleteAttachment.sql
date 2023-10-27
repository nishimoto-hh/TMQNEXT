DELETE
FROM
    attachment
WHERE
    function_type_id IN @FunctionTypeIdList
AND key_id = @KeyId
