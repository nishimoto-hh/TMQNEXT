UPDATE pt_inventory 
SET
    [temp_inventory_quantity] = @InventoryQuantity 
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
WHERE
    [inventory_id] = @InventoryId
