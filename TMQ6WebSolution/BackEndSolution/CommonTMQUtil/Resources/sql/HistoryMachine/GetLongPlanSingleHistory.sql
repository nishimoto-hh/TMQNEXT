SELECT
    COUNT(*)
FROM
    hm_mc_management_standards_component mcp,
    hm_mc_management_standards_content msc
WHERE
    mcp.management_standards_component_id = msc.management_standards_component_id
AND msc.long_plan_id IS NOT NULL
AND mcp.history_management_id = @HistoryManagementId