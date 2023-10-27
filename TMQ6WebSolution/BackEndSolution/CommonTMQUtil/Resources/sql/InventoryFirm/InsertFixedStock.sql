INSERT INTO pt_fixed_stock(
    [stock_id],                                 -- 在庫ID
    [target_month],                             -- 対象年月
    [parts_id],                                 -- 予備品ID
    [lot_control_id],                           -- ロット管理ID
    [inventory_control_id],                     -- 在庫管理ID
    [unit_structure_id],                        -- 数量単位
    [unit_price],                               -- 入庫単価
    [currency_structure_id],                    -- 金額単位
    [storage_quantity],                         -- 入庫数
    [storage_amount],                           -- 入庫金額
    [shipping_quantity],                        -- 出庫数
    [shipping_amount],                          -- 出庫金額
    [inventory_quantity],                       -- 末在庫数
    [inventory_amount],                         -- 末在庫金額
    [update_serialid],                          -- 更新シリアルID
    [insert_datetime],                          -- 登録日時
    [insert_user_id],                           -- 登録ユーザー
    [update_datetime],                          -- 更新日時
    [update_user_id]                            -- 更新ユーザー
)
VALUES(
    NEXT VALUE FOR seq_pt_fixed_stock_stock_id, -- 在庫ID
    @TargetMonth,                               -- 対象年月
    @PartsId,                                   -- 予備品id
    @LotControlId,                              -- ロット管理id
    @InventoryControlId,                        -- 在庫管理id
    @UnitStructureId,                           -- 数量単位
    @UnitPrice,                                 -- 入庫単価
    @CurrencyStructureId,                      -- 金額単位
    @StorageQuantity,                           -- 入庫数
    @StorageAmount,                             -- 入庫金額
    @ShippingQuantity,                          -- 出庫数
    @ShippingAmount,                            -- 出庫金額
    @InventoryQuantity,                         -- 末在庫数
    @InventoryAmount,                           -- 末在庫金額
    @UpdateSerialid,                            -- 更新シリアルID
    @InsertDatetime,                            -- 登録日時
    @InsertUserId,                              -- 登録ユーザー
    @UpdateDatetime,                            -- 更新日時
    @UpdateUserId                               -- 更新ユーザー
)
