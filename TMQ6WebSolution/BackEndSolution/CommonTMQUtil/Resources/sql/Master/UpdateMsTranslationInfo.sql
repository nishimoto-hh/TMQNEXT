UPDATE
    ms_translation
SET
    translation_text = @TranslationText,
/*@TranslationItemDescription
    translation_item_description = @TranslationItemDescription,
@TranslationItemDescription*/
	update_serialid = update_serialid + 1,
	update_datetime = @UpdateDatetime,
	update_user_id = @UpdateUserId
WHERE
    location_structure_id = @LocationStructureId
	AND translation_id = @TranslationId
	AND language_id = @LanguageId
