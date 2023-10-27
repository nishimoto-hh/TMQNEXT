UPDATE mc_management_standards_component 
SET
      inspection_site_structure_id = @InspectionSiteStructureId
    --, inspection_site_importance_structure_id = @InspectionSiteImportanceStructureId
    --, inspection_site_conservation_structure_id = @InspectionSiteConservationStructureId
    , is_management_standard_conponent = @IsManagementStandardConponent
    , update_serialid = update_serialid+1
    , update_datetime = @UpdateDatetime
    , update_user_id = @UpdateUserId
WHERE
    management_standards_component_id = @ManagementStandardsComponentId
