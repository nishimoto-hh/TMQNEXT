UPDATE
    ms_item
SET
    item_translation_id = @ItemTranslationId,
	update_serialid = update_serialid + 1,
	update_datetime = @UpdateDatetime,
	update_user_id = @UpdateUserId
WHERE
    item_id = @ItemId
