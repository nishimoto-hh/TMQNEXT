/*
* 在庫データ更新SQL
* UpdateStock.sql
*/
UPDATE
    pt_location_stock
SET
    stock_quantity = @StockQuantity
    /*@IsInput
   ,parts_location_id = @PartsLocationId
   ,parts_location_detail_no = @PartsLocationDetailNo
   @IsInput*/
   ,update_serialid = update_serialid + 1
   ,update_datetime = @UpdateDatetime
   ,update_user_id = @UpdateUserId
WHERE
    inventory_control_id = @InventoryControlId