DELETE ac
FROM mc_management_standards_component mcp
    ,mc_management_standards_content msc
    ,attachment ac
WHERE mcp.management_standards_component_id = msc.management_standards_component_id
AND msc.management_standards_content_id = ac.key_id
AND ac.function_type_id = 1620
AND mcp.machine_id = @MachineId
