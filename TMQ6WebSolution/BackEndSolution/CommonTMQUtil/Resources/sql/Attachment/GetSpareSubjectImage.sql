SELECT
    parts_name + '_' +
    (
        SELECT
            tra.translation_text
        FROM
            ms_translation tra
        WHERE
            tra.location_structure_id = 0
        AND tra.translation_id = 111060021
        AND tra.language_id = @LanguageId
    ) AS subject,
    @FunctionTypeId AS function_type_id,
    @KeyId AS key_id,
    factory_id AS location_structure_id,
    @DocumentTypeValNo AS document_type_val_no
FROM
    pt_parts parts
WHERE
    parts_id = @KeyId
