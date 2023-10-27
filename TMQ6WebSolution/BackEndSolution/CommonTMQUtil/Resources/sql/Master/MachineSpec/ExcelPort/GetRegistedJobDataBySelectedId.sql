SELECT
    ms.structure_id
FROM
    ms_structure ms
WHERE
    ms.structure_group_id = 1010
AND ms.delete_flg = 0
AND ms.parent_structure_id = (
        SELECT
            ms.parent_structure_id
        FROM
            ms_structure ms
        WHERE
            ms.structure_group_id = 1010
        AND ms.structure_id = @StructureId
    )