INSERT 
INTO pt_inventory( 
    [inventory_id]                              -- 棚卸id
    , [target_month]                            -- 対象年月
    , [parts_id]                                -- 予備品id
    , [parts_location_id]                       -- 棚id
    , [parts_location_detail_no]                -- 棚枝番
    , [old_new_structure_id]                    -- 新旧区分id
    , [department_structure_id]                 -- 部門id
    , [account_structure_id]                    -- 勘定科目id
    , [stock_quantity]                          -- 在庫数
    , [inventory_quantity]                      -- 棚卸数
    , [temp_inventory_quantity]                 -- 棚卸取込値
    , [unit_structure_id]                       -- 数量管理単位ID
    , [currency_structure_id]                   -- 金額管理単位ID
    , [preparation_datetime]                    -- 棚卸準備日時
    , [inventory_datetime]                      -- 棚卸実施日時
    , [temp_inventory_datetime]                 -- 棚卸実施日時取込値
    , [difference_datetime]                     -- 棚差調整日時
    , [fixed_datetime]                          -- 棚卸確定日時
    , [creation_division_structure_id]          -- 作成区分
    , [rftag_id]                                -- rfidタグ
    , [temp_rftag_id]                           -- rfidタグ取込値
    , [work_user_name]                          -- 作業者
    , [temp_work_user_name]                     -- 作業者取込値
    , [update_serialid]                         -- 更新シリアルID
    , [delete_flg]                              -- 削除フラグ
    , [insert_datetime]                         -- 登録日時
    , [insert_user_id]                          -- 登録ユーザー
    , [update_datetime]                         -- 更新日時
    , [update_user_id]                          -- 更新ユーザー
) OUTPUT inserted.inventory_id
VALUES ( 
    NEXT VALUE FOR seq_pt_inventory_inventory_id -- 棚卸id
    , @TargetMonth                              -- 対象年月
    , @PartsId                                  -- 予備品id
    , @PartsLocationId                          -- 棚id
    , @PartsLocationDetailNo                    -- 棚枝番
    , @OldNewStructureId                        -- 新旧区分id
    , @DepartmentStructureId                    -- 部門id
    , @AccountStructureId                       -- 勘定科目id
    , @StockQuantity                            -- 在庫数
    , @InventoryQuantity                        -- 棚卸数
    , @TmpInventoryQuantity                     -- 棚卸取込値
    , @UnitStructureId                          -- 数量単位名称
    , @CurrencyStructureId                      -- 金額単位名称
    , @PreparationDatetime                      -- 棚卸準備日時
    , @InventoryDatetime                        -- 棚卸実施日時
    , @TempInventoryDatetime                    -- 棚卸実施日時取込値
    , @DifferenceDatetime                       -- 棚卸調整日時
    , @FixedDatetime                            -- 棚卸確定日時
    , @CreationDivisionStructureId              -- 作成区分
    , @RftagId                                  -- rfidタグ
    , @TempRftagId                              -- rfidタグ取込値
    , @WorkUserName                             -- 作業者
    , @TempWorkUserName                         -- 作業者取込値
    , @UpdateSerialid                           -- 更新シリアルID
    , 0                                         -- 削除フラグ
    , @InsertDatetime                           -- 登録日時
    , @InsertUserId                             -- 登録ユーザー
    , @UpdateDatetime                           -- 更新日時
    , @UpdateUserId                             -- 更新ユーザー
)
