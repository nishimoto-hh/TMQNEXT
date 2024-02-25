UPDATE
    pt_parts
SET
    --[parts_no] = @PartsNo,                                          --予備品No
    [parts_name] = @PartsName,                                      --予備品名
    [manufacturer_structure_id] = @ManufacturerStructureId,         --メーカー
    [materials] = @Materials,                                       --材質
    [model_type] = @ModelType,                                      --型式
    [standard_size] = @StandardSize,                                --規格寸法
    [parts_service_space] = @PartsServiceSpace,                     --使用場所
    [factory_id] = @PartsFactoryId,                                 --管理工場
    [job_structure_id] = @JobStructureId,                           --職種
    [use_segment_structure_id] = @UseSegmentStructureId,            --使用区分
    [parts_location_detail_no] = @PartsLocationDetailNo,            --標準棚枝番
    [lead_time] = @LeadTimeExceptUnit,                              --発注点
    [order_quantity] = @OrderQuantityExceptUnit,                    --発注量
    [parts_location_id] = @PartsLocationId,                         --標準棚番ID
    [location_district_structure_id] = @LocationDistrictStructureId, --標準棚_地区ID
    [location_factory_structure_id] = @LocationFactoryStructureId,  --標準棚_工場ID
    [location_warehouse_structure_id] = @LocationWarehouseStructureId, --標準棚_倉庫ID
    [location_rack_structure_id] = @LocationRackStructureId,        --標準棚_棚ID
    [unit_structure_id] = @UnitStructureId,                         --数量管理単位ID
    [vender_structure_id] = @VenderStructureId,                     --標準仕入先
    [currency_structure_id] = @CurrencyStructureId,                 --金額管理単位ID
    [unit_price] = @UnitPriceExceptUnit,                            --標準単価
    [purchasing_no] = @PurchasingNo,                                --購買システムコード
    [parts_memo] = @PartsMemo,                                      --メモ
    [department_structure_id] = @DepartmentStructureId,             --標準部門
    [account_structure_id] = @AccountStructureId,                   --標準勘定科目
    [update_serialid] = update_serialid + 1,                        --更新シリアルID
    [update_datetime] = @UpdateDatetime,                            --更新日時
    [update_user_id] = @UpdateUserId                                --更新ユーザー
WHERE
    parts_id = @PartsId
