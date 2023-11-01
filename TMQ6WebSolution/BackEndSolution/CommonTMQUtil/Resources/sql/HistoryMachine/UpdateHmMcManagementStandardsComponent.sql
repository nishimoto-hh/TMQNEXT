UPDATE hm_mc_management_standards_component 
SET
    machine_id = @MachineId                                             -- 機番ID
    , inspection_site_structure_id = @InspectionSiteStructureId         -- 部位ID
    , is_management_standard_conponent = @IsManagementStandardConponent -- 機器別管理基準フラグ
    , update_serialid = update_serialid+1                               -- 更新シリアルID
    , update_datetime = @UpdateDatetime                                 -- 登録日時
    , update_user_id = @UpdateUserId                                    -- 登録ユーザー
    , remarks = @Remarks                                                -- 機器別管理基準備考
WHERE
    hm_management_standards_component_id = @HmManagementStandardsComponentId
