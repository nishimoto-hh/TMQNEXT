SELECT COUNT(*)
FROM mc_management_standards_component mcp
    ,mc_management_standards_content msc
WHERE mcp.management_standards_component_id = msc.management_standards_component_id
AND msc.long_plan_id IS NOT NULL
AND mcp.management_standards_component_id = @ManagementStandardsComponentId
 
