SELECT
    parts_name + '_画像' AS subject,
    @FunctionTypeId AS function_type_id,
    @KeyId AS key_id,
    factory_id AS location_structure_id,
    @DocumentTypeValNo AS document_type_val_no
FROM
    pt_parts parts
WHERE
    parts_id = @KeyId
