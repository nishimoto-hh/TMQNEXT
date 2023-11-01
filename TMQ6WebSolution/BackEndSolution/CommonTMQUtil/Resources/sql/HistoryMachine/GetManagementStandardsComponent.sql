SELECT
    component.management_standards_component_id, -- 機器別管理基準部位ID
    component.machine_id,                        -- 機番ID
    component.inspection_site_structure_id,      -- 部位ID
    component.is_management_standard_conponent,  -- 機器別管理基準フラグ
    component.remarks                            -- 機器別管理基準備考
FROM
    mc_management_standards_component component
WHERE
    /*@MachineId
    -- 機番IDを指定
    component.machine_id = @MachineId
    @MachineId*/

    /*@ComponentId
    -- 機器別管理基準部位IDを指定
    component.management_standards_component_id = @ManagementStandardsComponentId
    @ComponentId*/