UPDATE hm_mc_equipment 
SET 
  machine_id = @MachineId                              -- 機番ID
 ,circulation_target_flg = @CirculationTargetFlg       -- 循環対象
 ,manufacturer_structure_id = @ManufacturerStructureId -- メーカー
 ,manufacturer_type = @ManufacturerType                -- メーカー型式
 ,model_no = @ModelNo                                  -- 型式コード
 ,serial_no = @SerialNo                                -- シリアル番号
 ,date_of_manufacture = @DateOfManufacture             -- 製造日
 ,delivery_date = @DeliveryDate                        -- 納期
 ,equipment_note = @EquipmentNote                      -- 機器メモ
 ,use_segment_structure_id = @UseSegmentStructureId    -- 使用区分
 ,fixed_asset_no = @FixedAssetNo                       -- 固定資産番号
 ,maintainance_kind_manage = @MaintainanceKindManage   -- 点検種別毎管理
 ,update_serialid = update_serialid+1                  -- 更新シリアルID
 ,update_datetime = @UpdateDatetime                    -- 登録日時
 ,update_user_id = @UpdateUserId                       -- 登録ユーザー
WHERE
    hm_equipment_id = @HmEquipmentId
