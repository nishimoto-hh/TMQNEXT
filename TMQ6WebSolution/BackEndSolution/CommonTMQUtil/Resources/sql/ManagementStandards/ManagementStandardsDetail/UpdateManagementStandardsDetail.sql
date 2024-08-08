UPDATE mc_management_standards_detail 
SET
    [inspection_site_name] = @InspectionSiteName
    , [inspection_site_importance_structure_id] = @InspectionSiteImportanceStructureId
    , [inspection_site_conservation_structure_id] = @InspectionSiteConservationStructureId
    , [maintainance_division] = @MaintainanceDivision
    , [inspection_content_name] = @InspectionContentName
    , [maintainance_kind_structure_id] = @MaintainanceKindStructureId
    , [budget_amount] = @BudgetAmount
    , [schedule_type_structure_id] = @ScheduleTypeStructureId
    , [preparation_period] = @PreparationPeriod
    , [cycle_year] = @CycleYear
    , [cycle_month] = @CycleMonth
    , [cycle_day] = @CycleDay
    , [disp_cycle] = @DispCycle
    , [remarks] = @Remarks
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
WHERE
    [management_standards_detail_id] = @ManagementStandardsDetailId
