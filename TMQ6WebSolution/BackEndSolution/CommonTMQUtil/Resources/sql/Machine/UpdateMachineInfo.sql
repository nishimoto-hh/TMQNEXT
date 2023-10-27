UPDATE mc_machine 
SET
    location_structure_id = @LocationStructureId
    , location_district_structure_id = @LocationDistrictStructureId
    , location_factory_structure_id = @LocationFactoryStructureId
    , location_plant_structure_id = @LocationPlantStructureId
    , location_series_structure_id = @LocationSeriesStructureId
    , location_stroke_structure_id = @LocationStrokeStructureId
    , location_facility_structure_id = @LocationFacilityStructureId
    , job_structure_id = @JobStructureId
    , job_kind_structure_id = @JobKindStructureId
    , job_large_classfication_structure_id = @JobLargeClassficationStructureId
    , job_middle_classfication_structure_id = @JobMiddleClassficationStructureId
    , job_small_classfication_structure_id = @JobSmallClassficationStructureId
    , machine_no = @MachineNo
    , machine_name = @MachineName
    , installation_location = @InstallationLocation
    , number_of_installation = @NumberOfInstallation
    , equipment_level_structure_id = @EquipmentLevelStructureId
    , date_of_installation = @DateOfInstallation
    , importance_structure_id = @ImportanceStructureId
    , conservation_structure_id = @ConservationStructureId
    , machine_note = @MachineNote
    , update_serialid = update_serialid+1
    , update_datetime = @UpdateDatetime
    , update_user_id = @UpdateUserId
WHERE
    machine_id = @MachineId
