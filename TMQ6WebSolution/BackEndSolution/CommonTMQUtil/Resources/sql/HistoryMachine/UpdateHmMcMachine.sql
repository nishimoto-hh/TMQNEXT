UPDATE hm_mc_machine 
SET
    location_structure_id = @LocationStructureId                     -- 機能場所階層ID
    , location_district_structure_id = @DistrictId                   -- 地区
    , location_factory_structure_id = @FactoryId                     -- 工場
    , location_plant_structure_id = @PlantId                         -- プラント
    , location_series_structure_id = @SeriesId                       -- 系列
    , location_stroke_structure_id = @StrokeId                       -- 工程
    , location_facility_structure_id = @FacilityId                   -- 設備
    , job_structure_id = @JobStructureId                             -- 職種機種階層ID
    , job_kind_structure_id = @JobId                                 -- 職種
    , job_large_classfication_structure_id = @LargeClassficationId   -- 機種大分類
    , job_middle_classfication_structure_id = @MiddleClassficationId -- 機種中分類
    , job_small_classfication_structure_id = @SmallClassficationId   -- 機種小分類
    , machine_no = @MachineNo                                        -- 機器番号
    , machine_name = @MachineName                                    -- 機器名称
    , installation_location = @InstallationLocation                  -- 設置場所
    , number_of_installation = @NumberOfInstallation                 -- 設置台数
    , equipment_level_structure_id = @EquipmentLevelStructureId      -- 機器レベル
    , date_of_installation = @DateOfInstallation                     -- 設置日
    , importance_structure_id = @ImportanceStructureId               -- 重要度
    , conservation_structure_id = @ConservationStructureId           -- 保全方式
    , machine_note = @MachineNote                                    -- 機番メモ
    , update_serialid = update_serialid+1                            -- 更新シリアルID
    , update_datetime = @UpdateDatetime                              -- 登録日時
    , update_user_id = @UpdateUserId                                 -- 登録ユーザー
WHERE
    hm_machine_id = @HmMachineId
