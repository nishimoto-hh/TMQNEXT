UPDATE
    ms_item_extension
SET
    extension_data = @ExtensionData,
	update_serialid = update_serialid + 1,
	update_datetime = @UpdateDatetime,
	update_user_id = @UpdateUserId
WHERE
    item_id = @ItemId
	AND sequence_no = @SequenceNo
