UPDATE ma_history 
SET
    [call_count] = @CallCount
    , [expenditure] = @Expenditure
    , [cost_note] = @CostNote
    , [loss_absence] = @LossAbsence
    , [loss_absence_type_count] = @LossAbsenceTypeCount
    , [occurrence_time] = @OccurrenceTime
    , [discovery_personnel] = @DiscoveryPersonnel
    , [construction_personnel_id] = @ConstructionPersonnelId
    , [construction_personnel_name] = CASE 
        WHEN @ConstructionPersonnelId IS NULL 
            THEN NULL 
        ELSE COALESCE( 
            ( 
                SELECT
                    display_name 
                FROM
                    ms_user 
                WHERE
                    user_id = @ConstructionPersonnelId
            ) 
            , [construction_personnel_name]
        ) 
        END
    , [maintenance_season_structure_id] = @MaintenanceSeasonStructureId
    , [total_working_time] = @TotalWorkingTime
    , [working_time_self] = @WorkingTimeSelf
    , [working_time_research] = @WorkingTimeResearch
    , [working_time_procure] = @WorkingTimeProcure
    , [working_time_repair] = @WorkingTimeRepair
    , [working_time_test] = @WorkingTimeTest
    , [construction_company] = @ConstructionCompany
    , [working_time_company] = @WorkingTimeCompany
    , [actual_result_structure_id] = @ActualResultStructureId
    , [maintenance_opinion] = @MaintenanceOpinion
    , [manufacturing_personnel_id] = @ManufacturingPersonnelId
    , [manufacturing_personnel_name] = CASE 
        WHEN @ManufacturingPersonnelId IS NULL 
            THEN NULL 
        ELSE COALESCE( 
            ( 
                SELECT
                    display_name 
                FROM
                    ms_user 
                WHERE
                    user_id = @ManufacturingPersonnelId
            ) 
            , [manufacturing_personnel_name]
        ) 
        END
    , [work_failure_division_structure_id] = @WorkFailureDivisionStructureId
    , [stop_count] = @StopCount
    , [effect_production_structure_id] = @EffectProductionStructureId
    , [effect_quality_structure_id] = @EffectQualityStructureId
    , [failure_site] = @FailureSite
    , [parts_existence_flg] = @PartsExistenceFlg
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
OUTPUT
    inserted.history_id 
WHERE
    [history_id] = @HistoryId
