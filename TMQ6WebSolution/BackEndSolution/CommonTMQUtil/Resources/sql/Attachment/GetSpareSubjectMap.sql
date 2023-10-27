    SELECT DISTINCT
        location.translation_text + '_予備品地図' as subject,
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