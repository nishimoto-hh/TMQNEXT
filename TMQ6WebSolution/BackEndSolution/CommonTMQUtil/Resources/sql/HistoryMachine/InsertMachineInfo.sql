INSERT 
INTO hm_mc_machine( 
    [hm_machine_id]                             -- 機番情報変更管理ID
    , [history_management_id]                   -- 変更管理ID
    , [machine_id]                              -- 機番ID
    , [execution_division]                      -- 処理区分
    , [location_structure_id]                   -- 機能場所階層ID
    , [location_district_structure_id]          -- 地区ID
    , [location_factory_structure_id]           -- 工場ID
    , [location_plant_structure_id]             -- プラントID
    , [location_series_structure_id]            -- 系列ID
    , [location_stroke_structure_id]            -- 工程ID
    , [location_facility_structure_id]          -- 設備ID
    , [job_structure_id]                        -- 職種機種階層ID
    , [job_kind_structure_id]                   -- 職種ID
    , [job_large_classfication_structure_id]    -- 機種大分類ID
    , [job_middle_classfication_structure_id]   -- 機種中分類ID
    , [job_small_classfication_structure_id]    -- 機種小分類ID
    , [machine_no]                              -- 機器番号
    , [machine_name]                            -- 機器名称
    , [installation_location]                   -- 設置場所
    , [number_of_installation]                  -- 設置台数
    , [equipment_level_structure_id]            -- 機器レベル
    , [date_of_installation]                    -- 設置日
    , [importance_structure_id]                 -- 重要度
    , [conservation_structure_id]               -- 保全方式
    , [machine_note]                            -- 機番メモ
    , [update_serialid]                         -- 更新シリアルID
    , [insert_datetime]                         -- 登録日時
    , [insert_user_id]                          -- 登録ユーザー
    , [update_datetime]                         -- 更新日時
    , [update_user_id]                          -- 更新ユーザー
) 
VALUES ( 
    NEXT VALUE FOR seq_hm_mc_machine_hm_machine_id -- 機番情報変更管理ID
    , @HistoryManagementId                         -- 変更管理ID
    , @MachineId                                   -- 機番ID
    , @ExecutionDivision                           -- 処理区分
    , @LocationStructureId                         -- 機能場所階層ID
    , @DistrictId                                  -- 地区ID
    , @FactoryId                                   -- 工場ID
    , @PlantId                                     -- プラントID
    , @SeriesId                                    -- 系列ID
    , @StrokeId                                    -- 工程ID
    , @FacilityId                                  -- 設備ID
    , @JobStructureId                              -- 職種機種階層ID
    , @JobId                                       -- 職種ID
    , @LargeClassficationId                        -- 機種大分類ID
    , @MiddleClassficationId                       -- 機種中分類ID
    , @SmallClassficationId                        -- 機種小分類ID
    , @MachineNo                                   -- 機器番号
    , @MachineName                                 -- 機器名称
    , @InstallationLocation                        -- 設置場所
    , @NumberOfInstallation                        -- 設置台数
    , @EquipmentLevelStructureId                   -- 機器レベル
    , @DateOfInstallation                          -- 設置日
    , @ImportanceStructureId                       -- 重要度
    , @ConservationStructureId                     -- 保全方式
    , @MachineNote                                 -- 機番メモ
    , 0                                            -- 更新シリアルID
    , @InsertDatetime                              -- 登録日時
    , @InsertUserId                                -- 登録ユーザー
    , @UpdateDatetime                              -- 更新日時
    , @UpdateUserId                                -- 更新ユーザー
); 
