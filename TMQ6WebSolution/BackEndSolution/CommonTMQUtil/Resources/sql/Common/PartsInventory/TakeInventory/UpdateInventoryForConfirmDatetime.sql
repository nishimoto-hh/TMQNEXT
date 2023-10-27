/*
* 棚卸 棚卸確定日時で棚卸データを更新
* UpdateInventoryForConfirmDatetime.sql
*/
UPDATE
    pt_inventory
SET
     fixed_datetime = @InoutDatetime
    ,update_serialid = update_serialid + 1
    ,update_datetime = @UpdateDatetime
    ,update_user_id = @UpdateUserId
WHERE
    inventory_id = @InventoryId