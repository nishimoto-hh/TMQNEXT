DELETE msc
FROM mc_management_standards_component mcp
    ,mc_management_standards_content msc
WHERE mcp.management_standards_component_id = msc.management_standards_component_id
AND mcp.management_standards_component_id = @ManagementStandardsComponentId
