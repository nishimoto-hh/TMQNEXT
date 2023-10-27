SELECT
    machine_name AS subject,
    @FunctionTypeId AS function_type_id,
    @KeyId AS key_id,
    location_structure_id,
    @DocumentTypeValNo AS document_type_val_no
FROM
    mc_mp_information mp
    LEFT JOIN
        mc_machine mahine
    ON  mp.machine_id = mahine.machine_id
WHERE
    mp_information_id = @KeyId
