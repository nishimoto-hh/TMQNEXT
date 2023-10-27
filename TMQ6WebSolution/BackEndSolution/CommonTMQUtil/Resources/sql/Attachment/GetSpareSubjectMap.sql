    SELECT DISTINCT
        location.translation_text + '_' +
        (
            SELECT
                tra.translation_text
            FROM
                ms_translation tra
            WHERE
                tra.location_structure_id = 0
            AND tra.translation_id = 111380026
            AND tra.language_id = @LanguageId
        ) AS subject,
        @FunctionTypeId AS function_type_id,
        @KeyId AS key_id,
        @KeyId AS location_structure_id,
        @DocumentTypeValNo AS document_type_val_no,
        @KeyId AS trans_location_structure_id
        
    FROM
        v_structure_item_all location
    WHERE
        structure_group_id = 1000
    AND structure_layer_no = 0
    AND language_id = @LanguageId
    AND structure_id = @KeyId