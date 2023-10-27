SELECT COUNT(*)
FROM mc_management_standards_component mcp -- 機器別管理基準部位
    ,mc_management_standards_content msc   -- 機器別管理基準内容
WHERE mcp.management_standards_component_id = msc.management_standards_component_id
AND mcp.is_management_standard_conponent = 1    -- 機器別管理基準フラグ
AND mcp.machine_id = @MachineId -- 機番ID
AND mcp.inspection_site_structure_id = @InspectionSiteStructureId -- 部位ID
AND msc.inspection_content_structure_id = @InspectionContentStructureId -- 点検内容ID(保全項目)
AND msc.management_standards_content_id <> @ManagementStandardsContentId -- 自分自身以外
