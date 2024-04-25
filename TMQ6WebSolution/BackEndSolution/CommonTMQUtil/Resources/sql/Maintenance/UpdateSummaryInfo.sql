UPDATE ma_summary 
SET
    [activity_division] = @ActivityDivision
    , [subject] = @Subject
    , [plan_implementation_content] = @PlanImplementationContent
    , [subject_note] = @SubjectNote
    , [location_structure_id] = @LocationStructureId
    , [location_district_structure_id] = @DistrictId
    , [location_factory_structure_id] = @FactoryId
    , [location_plant_structure_id] = @PlantId
    , [location_series_structure_id] = @SeriesId
    , [location_stroke_structure_id] = @StrokeId
    , [location_facility_structure_id] = @FacilityId
    , [job_structure_id] = @JobStructureId
    , [mq_class_structure_id] = @MqClassStructureId
    , [repair_cost_class_structure_id] = @RepairCostClassStructureId
    , [budget_management_structure_id] = @BudgetManagementStructureId
    , [budget_personality_structure_id] = @BudgetPersonalityStructureId
    , [sudden_division_structure_id] = @SuddenDivisionStructureId
    , [stop_system_structure_id] = @StopSystemStructureId
    , [stop_time] = @StopTime
    , [maintenance_count] = @MaintenanceCount
    , [change_management_structure_id] = @ChangeManagementStructureId
    , [env_safety_management_structure_id] = @EnvSafetyManagementStructureId
    , [construction_date] = @ConstructionDate
    , [completion_date] = @CompletionDate
    , [completion_time] = @CompletionTime
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
OUTPUT inserted.summary_id
WHERE
    [summary_id] = @SummaryId
