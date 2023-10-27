DELETE 
FROM
    ms_structure_unused 
WHERE
    structure_id = @StructureId 
    AND factory_id = @FactoryId
