SELECT TOP 1
    tr.translation_text
FROM
    ms_translation tr
WHERE
    tr.translation_id = 111090061
AND tr.language_id = @LanguageId
AND tr.location_structure_id = 0