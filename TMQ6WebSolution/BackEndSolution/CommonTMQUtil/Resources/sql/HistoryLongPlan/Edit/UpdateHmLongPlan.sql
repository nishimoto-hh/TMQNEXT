UPDATE
    hm_ln_long_plan
SET
    subject = @Subject,
    location_structure_id = @LocationStructureId,
    job_structure_id = @JobStructureId,
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
    history_management_id = @HistoryManagementId
