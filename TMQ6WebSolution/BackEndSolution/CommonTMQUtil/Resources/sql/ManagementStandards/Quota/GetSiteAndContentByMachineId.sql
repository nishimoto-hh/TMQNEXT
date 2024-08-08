-- Š„“–‘ÎÛ‚Ì‹@Ší‚É•R•t‚­•”ˆÊE€–Ú‚ğæ“¾
SELECT
    comp.inspection_site_structure_id
    , cont.inspection_content_structure_id 
FROM
    mc_management_standards_component comp 
    INNER JOIN mc_management_standards_content cont 
        ON comp.management_standards_component_id = cont.management_standards_component_id 
WHERE
    comp.machine_id = @MachineId