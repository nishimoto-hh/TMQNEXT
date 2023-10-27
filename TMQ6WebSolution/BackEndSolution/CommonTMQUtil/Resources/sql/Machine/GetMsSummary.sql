SELECT COUNT(*)
FROM mc_management_standards_component mcp
    ,mc_management_standards_content msc
    ,mc_maintainance_schedule ms
    ,mc_maintainance_schedule_detail msd
WHERE mcp.management_standards_component_id = msc.management_standards_component_id
AND msc.management_standards_content_id = ms.management_standards_content_id
AND ms.maintainance_schedule_id = msd.maintainance_schedule_id
AND msc.long_plan_id IS NOT NULL
AND msd.summary_id IS NOT NULL
AND mcp.machine_id = @MachineId
 
 
