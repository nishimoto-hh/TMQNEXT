UPDATE pt_inventory 
SET
    [inventory_quantity] = @InventoryQuantity 
    , [temp_inventory_quantity] = NULL 
    , [inventory_datetime] = @InventoryDatetime
    , [temp_inventory_datetime] = NULL
    , [difference_datetime] = NULL
    /*@UploadFlg
    , [rftag_id] = @RftagId
    @UploadFlg*/
    , [temp_rftag_id] = NULL
    /*@UploadFlg
    , [work_user_name] = @WorkUserName
    @UploadFlg*/
    , [temp_work_user_name] = NULL
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
WHERE
    [inventory_id] = @InventoryId