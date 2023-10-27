SELECT
    component.hm_management_standards_component_id
FROM
    hm_mc_management_standards_component component
WHERE
    component.history_management_id = @HistoryManagementId