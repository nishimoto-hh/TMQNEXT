SELECT
    child.parent_structure_id
FROM
    ms_structure child
WHERE
    child.structure_id = @LocationStructureId