INSERT 
INTO hm_mc_machine( 
    [hm_machine_id]                             -- 機番情報変更管理ID
    , [history_management_detail_id]            -- 変更管理詳細ID
    , [machine_id]                              -- 機番ID
    , [location_structure_id]                   -- 機能場所階層ID
    , [job_structure_id]                        -- 職種機種階層ID
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
/*@NewMachineId
OUTPUT inserted.hm_machine_id
@NewMachineId*/
VALUES ( 
    NEXT VALUE FOR seq_hm_mc_machine_hm_machine_id -- 機番情報変更管理ID
    , @HistoryManagementDetailId                   -- 変更管理詳細ID

      /*@NewMachineId
      -- 機器の新規登録・複写の場合
    , NEXT VALUE FOR seq_mc_machine_machine_id     -- 機番ID
      @NewMachineId*/

      /*@DefaultMachineId
      -- 機器の修正・削除の場合は既存の機番ID
    , @MachineId                                   -- 機番ID
      @DefaultMachineId*/

    , @LocationStructureId                         -- 機能場所階層ID
    , @JobStructureId                              -- 職種機種階層ID
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
