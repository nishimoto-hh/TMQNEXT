UPDATE hm_mc_management_standards_content 
SET
    management_standards_component_id = @ManagementStandardsComponentId                  -- 機器別管理基準部位ID
    , inspection_content_structure_id = @InspectionContentStructureId                    -- 点検内容id
    , inspection_site_importance_structure_id = @InspectionSiteImportanceStructureId     -- 部位重要度
    , inspection_site_conservation_structure_id = @InspectionSiteConservationStructureId -- 部位保全方式
    , maintainance_division = @MaintainanceDivision                                      -- 保全区分
    , maintainance_kind_structure_id = @MaintainanceKindStructureId                      -- 点検種別
    , budget_amount = @BudgetAmount                                                      -- 予算金額
    , preparation_period = @PreparationPeriod                                            -- 準備期間(日)
    , schedule_type_structure_id = @ScheduleTypeStructureId                              -- スケジュール管理基準ID
    , update_serialid = update_serialid+1                                                -- 更新シリアルID
    , update_datetime = @UpdateDatetime                                                  -- 登録日時
    , update_user_id = @UpdateUserId                                                     -- 登録ユーザー
WHERE
    hm_management_standards_content_id = @HmManagementStandardsContentId
