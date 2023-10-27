SELECT
    COUNT(pi.inventory_id) 
FROM
    pt_parts pp 
    INNER JOIN pt_inventory pi 
        ON pp.parts_location_id = pi.parts_location_id 
WHERE
    pi.target_month = @TargetYearMonth 
    AND pi.parts_location_id IN @PartsLocationIdList 
    AND pi.department_structure_id IN @DepartmentIdList 
/*@FactoryIdList
    AND pp.factory_id IN @FactoryIdList
@FactoryIdList*/
/*@JobIdList
    AND pp.job_structure_id IN @JobIdList
@JobIdList*/
