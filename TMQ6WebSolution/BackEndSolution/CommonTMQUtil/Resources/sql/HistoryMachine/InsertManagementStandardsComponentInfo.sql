INSERT 
INTO hm_mc_management_standards_component(
    [hm_management_standards_component_id]      -- 機器別管理基準部位変更管理ID
    , [history_management_detail_id]            -- 変更管理詳細ID
    , [management_standards_component_id]       -- 機器別管理基準部位ID
    , [machine_id]                              -- 機番ID
    , [inspection_site_structure_id]            -- 部位ID
    , [is_management_standard_conponent]        -- 機器別管理基準フラグ
    , [update_serialid]                         -- 更新シリアルID
    , [insert_datetime]                         -- 登録日時
    , [insert_user_id]                          -- 登録ユーザー
    , [update_datetime]                         -- 更新日時
    , [update_user_id]                          -- 更新ユーザー
) 
VALUES ( 
    NEXT VALUE FOR seq_hm_mc_management_standards_component_hm_management_standards_component_id -- 機器別管理基準部位変更管理ID
    , @HistoryManagementDetailId                -- 変更管理詳細ID
    , @ManagementStandardsComponentId           -- 機器別管理基準部位ID
    , @MachineId                                -- 機番ID
    , @InspectionSiteStructureId                -- 部位ID
    , @IsManagementStandardConponent            -- 機器別管理基準フラグ
    , 0                                         -- 更新シリアルID
    , @InsertDatetime                           -- 登録日時
    , @InsertUserId                             -- 登録ユーザー
    , @UpdateDatetime                           -- 更新日時
    , @UpdateUserId                             -- 更新ユーザー
); 
