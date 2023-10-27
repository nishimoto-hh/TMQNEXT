INSERT 
INTO pt_inventory_difference( 
    [inventory_difference_id]
    , [inventory_id]
    , [inout_history_id]
    , [inout_division_structure_id]
    , [work_division_structure_id]
    , [work_no]
    , [lot_control_id]
    , [inventory_control_id]
    , [department_structure_id]
    , [account_structure_id]
    , [management_division]
    , [management_no]
    , [inout_datetime]
    , [inout_quantity]
    , [creation_division_structure_id]
    , [unit_price]
    , [update_serialid]
    , [delete_flg]
    , [insert_datetime]
    , [insert_user_id]
    , [update_datetime]
    , [update_user_id]
) 
VALUES ( 
    NEXT VALUE FOR seq_pt_inventory_difference_inventory_difference_id
    , @InventoryId
    , @InoutHistoryId
    , @InoutDivisionStructureId
    , @WorkDivisionStructureId
    , @WorkNo
    , @LotControlId
    , @InventoryControlId
    , @DepartmentStructureId
    , @AccountStructureId
    , @ManagementDivision
    , @ManagementNo
    , @InoutDatetime
    , @InoutQuantity
    , @CreationDivisionStructureId
    , @UnitPrice
    , @UpdateSerialid
    , 0
    , @InsertDatetime
    , @InsertUserId
    , @UpdateDatetime
    , @UpdateUserId
)
