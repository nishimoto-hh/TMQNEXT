--******************************************************************
--棚差調整データに入庫単価を登録するSQL
--******************************************************************
UPDATE
    pt_inventory_difference
SET
    unit_price = @UnitPrice --入庫単価
    ,update_serialid = update_serialid + 1
    ,update_datetime = @UpdateDatetime
    ,update_user_id = @UpdateUserId
WHERE
    inventory_difference_id = @InventoryDifferenceId
