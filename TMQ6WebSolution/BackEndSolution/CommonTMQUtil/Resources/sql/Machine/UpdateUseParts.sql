UPDATE mc_machine_use_parts 
SET
	  parts_id = @PartsId
	, use_quantity = @UseQuantity
    , update_serialid = update_serialid+1
    , update_datetime = @UpdateDatetime
    , update_user_id = @UpdateUserId
WHERE
    machine_use_parts_id = @MachineUsePartsId
