SELECT
    machine_name AS subject,
    @FunctionTypeId AS function_type_id,
    @KeyId AS key_id,
    location_structure_id,
    @DocumentTypeValNo AS document_type_val_no
FROM
    mc_equipment equip
    LEFT JOIN
        mc_machine machine
    ON  equip.machine_id = machine.machine_id
WHERE equip.equipment_id = @KeyId
