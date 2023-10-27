UPDATE mc_management_standards_content 
SET
     management_standards_component_id = @ManagementStandardsComponentId
    ,inspection_content_structure_id = @InspectionContentStructureId
    ,inspection_site_importance_structure_id = @InspectionSiteImportanceStructureId
    ,inspection_site_conservation_structure_id = @InspectionSiteConservationStructureId
    ,maintainance_kind_structure_id = @MaintainanceKindStructureId
    ,budget_amount = @BudgetAmount
    ,preparation_period = @PreparationPeriod
    ,maintainance_division = @MaintainanceDivision
    ,schedule_type_structure_id = @ScheduleTypeStructureId
    , update_serialid = update_serialid+1
    , update_datetime = @UpdateDatetime
    , update_user_id = @UpdateUserId
WHERE
    management_standards_content_id = @ManagementStandardsContentId
