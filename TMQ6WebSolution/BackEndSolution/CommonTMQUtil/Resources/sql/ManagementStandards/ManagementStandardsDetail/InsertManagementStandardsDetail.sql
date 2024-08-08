INSERT 
INTO mc_management_standards_detail( 
    [management_standards_detail_id]
    , [management_standards_id]
    , [inspection_site_name]
    , [inspection_site_importance_structure_id]
    , [inspection_site_conservation_structure_id]
    , [maintainance_division]
    , [inspection_content_name]
    , [maintainance_kind_structure_id]
    , [budget_amount]
    , [schedule_type_structure_id]
    , [preparation_period]
    , [cycle_year]
    , [cycle_month]
    , [cycle_day]
    , [disp_cycle]
    , [remarks]
    , [update_serialid]
    , [insert_datetime]
    , [insert_user_id]
    , [update_datetime]
    , [update_user_id]
) 
VALUES ( 
    NEXT VALUE FOR seq_mc_management_standards_detail_management_standards_detail_id
    , @ManagementStandardsId
    , @InspectionSiteName
    , @InspectionSiteImportanceStructureId
    , @InspectionSiteConservationStructureId
    , @MaintainanceDivision
    , @InspectionContentName
    , @MaintainanceKindStructureId
    , @BudgetAmount
    , @ScheduleTypeStructureId
    , @PreparationPeriod
    , @CycleYear
    , @CycleMonth
    , @CycleDay
    , @DispCycle
    , @Remarks
    , @UpdateSerialid
    , @InsertDatetime
    , @InsertUserId
    , @UpdateDatetime
    , @UpdateUserId
)
