SELECT
    machine_name AS subject,
    @FunctionTypeId AS function_type_id,
    @KeyId AS key_id,
    location_structure_id,
    @DocumentTypeValNo AS document_type_val_no
FROM
    mc_management_standards_content content
    LEFT JOIN
        mc_management_standards_component component
    ON  content.management_standards_component_id = component.management_standards_component_id
    LEFT JOIN
        mc_machine machine
    ON  component.machine_id = machine.machine_id
WHERE
    content.management_standards_content_id = @KeyId
