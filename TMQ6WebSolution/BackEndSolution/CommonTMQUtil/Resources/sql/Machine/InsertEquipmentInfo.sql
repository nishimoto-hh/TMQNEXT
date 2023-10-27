INSERT 
INTO mc_equipment( 
      equipment_id                            -- 機器ID
    , machine_id                              -- 機番ID
    , circulation_target_flg                  -- 循環対象
    , manufacturer_structure_id               -- メーカー
    , manufacturer_type                       -- メーカー型式
    , model_no                                -- 型式コード
    , serial_no                               -- シリアル番号
    , date_of_manufacture                     -- 製造日
    , delivery_date                           -- 納期
    , equipment_note                          -- 機器メモ
    , use_segment_structure_id                -- 使用区分
    , fixed_asset_no                          -- 固定資産番号
    , maintainance_kind_manage                -- 点検種別毎管理
    , update_serialid                         -- 更新シリアルID
    , insert_datetime                         -- 登録日時
    , insert_user_id                          -- 登録ユーザー
    , update_datetime                         -- 更新日時
    , update_user_id                          -- 更新ユーザー
) 
OUTPUT inserted.equipment_id
VALUES ( 
    NEXT VALUE FOR seq_mc_equipment_equipment_id -- 機器ID
    , @MachineId                              -- 機番ID
    , @CirculationTargetFlg                   -- 循環対象
    , @ManufacturerStructureId                -- メーカー
    , @ManufacturerType                       -- メーカー型式
    , @ModelNo                                -- 型式コード
    , @SerialNo                               -- シリアル番号
    , @DateOfManufacture                      -- 製造日
    , @DeliveryDate                           -- 納期
    , @EquipmentNote                          -- 機器メモ
    , @UseSegmentStructureId                  -- 使用区分
    , @FixedAssetNo                           -- 固定資産番号
    , @MaintainanceKindManage                 -- 点検種別毎管理
    , 0                                       -- 更新シリアルID
    , @InsertDatetime                         -- 登録日時
    , @InsertUserId                           -- 登録ユーザー
    , @UpdateDatetime                         -- 更新日時
    , @UpdateUserId                           -- 更新ユーザー
); 
