UPDATE mc_machine 
SET
    location_structure_id = @LocationStructureId
    , job_structure_id = @JobStructureId
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
