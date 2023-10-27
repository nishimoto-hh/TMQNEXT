INSERT 
INTO mc_management_standards_component(
      management_standards_component_id        -- 機器別管理基準部位ID
     ,machine_id                               -- 機番ID
     ,inspection_site_structure_id             -- 部位ID
     --,inspection_site_importance_structure_id  -- 部位重要度
     --,inspection_site_conservation_structure_id -- 部位保全方式
     ,is_management_standard_conponent         -- 機器別管理基準フラグ
     ,update_serialid                          -- 更新シリアルID
     ,insert_datetime                          -- 登録日時
     ,insert_user_id                           -- 登録ユーザー
     ,update_datetime                          -- 更新日時
     ,update_user_id                           -- 更新ユーザー
) 
OUTPUT inserted.management_standards_component_id
VALUES ( 
    NEXT VALUE FOR seq_mc_management_standards_component_management_standards_component_id      -- 機番id
	, @MachineId                                -- 機番ID
	, @InspectionSiteStructureId                -- 部位ID
	--, @InspectionSiteImportanceStructureId      -- 部位重要度
	--, @InspectionSiteConservationStructureId    -- 部位保全方式
	, @IsManagementStandardConponent            -- 機器別管理基準フラグ
    , 0                                         -- 更新シリアルID
    , @InsertDatetime                           -- 登録日時
    , @InsertUserId                             -- 登録ユーザー
    , @UpdateDatetime                           -- 更新日時
    , @UpdateUserId                             -- 更新ユーザー
); 
