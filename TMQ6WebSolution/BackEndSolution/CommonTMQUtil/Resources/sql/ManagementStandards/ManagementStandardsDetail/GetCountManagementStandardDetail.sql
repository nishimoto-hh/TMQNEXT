SELECT
    count(*) 
FROM
    mc_management_standards_detail 
WHERE
    management_standards_id = @ManagementStandardsId 
    AND management_standards_detail_id <> @ManagementStandardsDetailId 
    AND inspection_site_name = @InspectionSiteName 
    AND inspection_content_name = @InspectionContentName
