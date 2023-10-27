UPDATE mc_equipment 
SET 
  machine_id = @MachineId
 ,circulation_target_flg = @CirculationTargetFlg
 ,manufacturer_structure_id = @ManufacturerStructureId
 ,manufacturer_type = @ManufacturerType
 ,model_no = @ModelNo
 ,serial_no = @SerialNo
 ,date_of_manufacture = @DateOfManufacture
 ,delivery_date = @DeliveryDate
 ,equipment_note = @EquipmentNote
 ,use_segment_structure_id = @UseSegmentStructureId
 ,fixed_asset_no = @FixedAssetNo
 ,maintainance_kind_manage = @MaintainanceKindManage
 ,budget_management_structure_id = @BudgetManagementStructureId
 ,diagram_storage_location_structure_id = @DiagramStorageLocationStructureId
 ,update_serialid = update_serialid+1
 ,update_datetime = @UpdateDatetime
 ,update_user_id = @UpdateUserId

WHERE
    equipment_id = @EquipmentId
