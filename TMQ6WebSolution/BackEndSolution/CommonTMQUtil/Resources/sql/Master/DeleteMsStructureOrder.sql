DELETE 
FROM
    ms_structure_order 
WHERE
    structure_group_id = @StructureGroupId 
    AND factory_id = @FactoryId
