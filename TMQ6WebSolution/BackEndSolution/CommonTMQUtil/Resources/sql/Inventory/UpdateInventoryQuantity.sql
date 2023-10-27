UPDATE pt_inventory 
SET
    [inventory_quantity] = @InventoryQuantity 
    , [temp_inventory_quantity] = NULL 
    , [inventory_datetime] = @InventoryDatetime
    , [difference_datetime] = NULL
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
WHERE
    [inventory_id] = @InventoryId