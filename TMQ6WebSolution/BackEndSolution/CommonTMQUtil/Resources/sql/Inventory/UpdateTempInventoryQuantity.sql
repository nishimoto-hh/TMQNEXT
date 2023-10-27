UPDATE pt_inventory 
SET
    [temp_inventory_quantity] = @InventoryQuantity
    , [temp_inventory_datetime] = @InventoryDatetime
    , [temp_rftag_id] = @RftagId
    , [temp_work_user_name] = @WorkUserName
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
WHERE
    [inventory_id] = @InventoryId
