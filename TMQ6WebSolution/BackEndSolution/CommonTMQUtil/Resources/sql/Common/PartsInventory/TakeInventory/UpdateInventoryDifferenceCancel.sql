/*
* 棚卸 棚差調整データに確定で登録したキー値を更新
* UpdateInventoryDifference.sql
*/
UPDATE
    pt_inventory_difference
SET
     inout_history_id = 0
    ,work_no = NULL
    /*@IsInput
    ,lot_control_id = NULL
    ,inventory_control_id = NULL
    @IsInput*/
    ,update_serialid = update_serialid + 1
    ,update_datetime = @UpdateDatetime
    ,update_user_id = @UpdateUserId
WHERE
    inventory_difference_id = @InventoryDifferenceId