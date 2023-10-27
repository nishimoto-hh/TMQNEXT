/*
* 棚卸 棚差調整データに確定で登録したキー値を更新
* UpdateInventoryDifference.sql
*/
UPDATE
    pt_inventory_difference
SET
     inout_history_id = @InoutHistoryId
    ,work_no = @WorkNo
    /*@IsInput
    ,lot_control_id = @LotControlId
    ,inventory_control_id = @InventoryControlId
    @IsInput*/
    ,update_serialid = update_serialid + 1
    ,update_datetime = @UpdateDatetime
    ,update_user_id = @UpdateUserId
WHERE
    inventory_difference_id = @InventoryDifferenceId