select
    eq.* 
from
    mc_management_standards_content cont 
    inner join mc_management_standards_component comp 
        on cont.management_standards_component_id = comp.management_standards_component_id 
    inner join mc_equipment eq 
        on comp.machine_id = eq.machine_id 
where
    cont.management_standards_content_id = @ManagementStandardsContentId
