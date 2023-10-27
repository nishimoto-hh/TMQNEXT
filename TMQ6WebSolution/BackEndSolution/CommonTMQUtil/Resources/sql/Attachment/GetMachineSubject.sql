SELECT
    machine_name AS subject,
    @FunctionTypeId AS function_type_id,
    @KeyId AS key_id,
    location_structure_id,
    @DocumentTypeValNo AS document_type_val_no
FROM
    mc_machine machine
WHERE
    machine_id = @KeyId
