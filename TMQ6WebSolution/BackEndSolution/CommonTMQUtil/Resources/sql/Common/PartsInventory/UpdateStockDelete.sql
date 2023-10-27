/*
* 在庫データ削除SQL(論理削除なのでUpdate)
* UpdateStockDelete.sql
*/
UPDATE
    pt_location_stock
SET
     stock_quantity = 0
    ,update_serialid = update_serialid + 1
    ,update_datetime = @UpdateDatetime
    ,update_user_id = @UpdateUserId
    ,delete_flg = 1
WHERE
    inventory_control_id = @InventoryControlId