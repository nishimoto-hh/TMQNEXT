UPDATE
    ln_long_plan
SET
    subject = @Subject,
    location_structure_id = @LocationStructureId,
    location_district_structure_id = @LocationDistrictStructureId,
    location_factory_structure_id = @LocationFactoryStructureId,
    location_plant_structure_id = @LocationPlantStructureId,
    location_series_structure_id = @LocationSeriesStructureId,
    location_stroke_structure_id = @LocationStrokeStructureId,
    location_facility_structure_id = @LocationFacilityStructureId,
    job_structure_id = @JobStructureId,
    job_kind_structure_id = @JobKindStructureId,
    job_large_classfication_structure_id = @JobLargeClassficationStructureId,
    job_middle_classfication_structure_id = @JobMiddleClassficationStructureId,
    job_small_classfication_structure_id = @JobSmallClassficationStructureId,
    subject_note = @SubjectNote,
    person_id = @PersonId,
    person_name = CASE
        WHEN @PersonId IS NULL THEN NULL
        ELSE COALESCE((
                SELECT
                    display_name
                FROM
                    ms_user
                WHERE
                    user_id = @PersonId
            ), person_name)
    END,
    work_item_structure_id = @WorkItemStructureId,
    budget_management_structure_id = @BudgetManagementStructureId,
    budget_personality_structure_id = @BudgetPersonalityStructureId,
    maintenance_season_structure_id = @MaintenanceSeasonStructureId,
    purpose_structure_id = @PurposeStructureId,
    work_class_structure_id = @WorkClassStructureId,
    treatment_structure_id = @TreatmentStructureId,
    facility_structure_id = @FacilityStructureId,
    update_serialid = update_serialid + 1,
    update_datetime = @UpdateDatetime,
    update_user_id = @UpdateUserId
WHERE
    long_plan_id = @LongPlanId