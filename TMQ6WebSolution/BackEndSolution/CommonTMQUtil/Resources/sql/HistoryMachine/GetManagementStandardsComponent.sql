SELECT
    component.management_standards_component_id, -- 機器別管理基準部位変更管理ID
    component.machine_id,                        -- 機番ID
    component.inspection_site_structure_id,      -- 部位ID
    component.is_management_standard_conponent   -- 機器別管理基準フラグ
FROM
    mc_management_standards_component component
WHERE
    component.machine_id = @MachineId