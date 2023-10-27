UPDATE pt_inventory 
SET
    [difference_datetime] = @DifferenceDatetime
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
WHERE
    [inventory_id] = @InventoryId