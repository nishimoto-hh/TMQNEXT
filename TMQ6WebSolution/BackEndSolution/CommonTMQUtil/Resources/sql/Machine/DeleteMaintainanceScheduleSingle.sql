DELETE ms
FROM mc_management_standards_component mcp
    ,mc_management_standards_content msc
    ,mc_maintainance_schedule ms
WHERE mcp.management_standards_component_id = msc.management_standards_component_id
AND msc.management_standards_content_id = ms.management_standards_content_id
AND mcp.management_standards_component_id = @ManagementStandardsComponentId 
