SELECT
    subject,
    @FunctionTypeId AS function_type_id,
    @KeyId AS key_id,
    location_structure_id,
    @DocumentTypeValNo AS document_type_val_no
FROM
    ma_summary summary
WHERE
    summary_id = @KeyId
