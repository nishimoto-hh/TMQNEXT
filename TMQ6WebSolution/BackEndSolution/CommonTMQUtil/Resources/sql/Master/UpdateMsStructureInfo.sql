UPDATE
    ms_structure
SET
    delete_flg = @DeleteFlg,
	update_serialid = update_serialid + 1,
	update_datetime = @UpdateDatetime,
	update_user_id = @UpdateUserId
WHERE
    structure_id = @StructureId
