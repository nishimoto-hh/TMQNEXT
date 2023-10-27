DELETE 
FROM
    ms_translation 
WHERE
    location_structure_id = @LocationStructureId 
    AND translation_id = @TranslationId 
    AND language_id = @LanguageId
