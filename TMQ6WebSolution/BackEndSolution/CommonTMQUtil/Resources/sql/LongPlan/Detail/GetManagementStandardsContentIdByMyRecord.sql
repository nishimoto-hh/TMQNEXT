select
    * 
from
    mc_management_standards_component component 
    inner join mc_management_standards_content content 
        on component.management_standards_component_id = content.management_standards_component_id 
where
    content.long_plan_id = @LongPlanId -- 自身のレコードと同一の長計件名ID
    and component.machine_id = @MachineId -- 自身のレコードと同一の機番ID
    and content.maintainance_kind_structure_id = @MaintainanceKindStructureId -- 自身のレコードと同一の点検種別 
    and content.management_standards_content_id != @ManagementStandardsContentId -- 自身のレコードは除く
