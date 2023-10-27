/*
* 棚卸 確定取消 受払履歴を更新
* UpdateHistoryForConfirmCancel.sql
*/
UPDATE
    pt_inout_history
SET
     inventory_datetime = NULL
    ,update_serialid = update_serialid + 1
    ,update_datetime = @UpdateDatetime
    ,update_user_id = @UpdateUserId
WHERE
    inventory_datetime IS NOT NULL
AND inventory_datetime = (
        SELECT
            fixed_datetime
        FROM
            pt_inventory
        WHERE
            inventory_id = @InventoryId
    )
AND inout_datetime <= (
        SELECT
            fixed_datetime
        FROM
            pt_inventory
        WHERE
            inventory_id = @InventoryId
    )
AND delete_flg = 0