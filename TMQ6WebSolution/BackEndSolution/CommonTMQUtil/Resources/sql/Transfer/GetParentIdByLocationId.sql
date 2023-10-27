SELECT
    ms.parent_structure_id
FROM
    ms_structure ms
WHERE
    ms.structure_id = @PartsLocationId