SELECT COUNT(*)
FROM mc_management_standards_component mcp -- 機器別管理基準部位
    ,mc_management_standards_content msc   -- 機器別管理基準内容
WHERE mcp.management_standards_component_id = msc.management_standards_component_id
AND mcp.is_management_standard_conponent = 1    -- 機器別管理基準フラグ
AND mcp.machine_id = @MachineId -- 機番ID
AND mcp.inspection_site_structure_id = @InspectionSiteStructureId -- 部位ID
AND (msc.inspection_site_importance_structure_id  <> @InspectionSiteImportanceStructureId  -- 部位重要度
OR msc.inspection_site_conservation_structure_id <> @InspectionSiteConservationStructureId) -- 保全方式
