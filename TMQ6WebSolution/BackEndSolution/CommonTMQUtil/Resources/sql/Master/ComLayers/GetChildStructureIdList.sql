SELECT
    structure_id                                -- 構成ID
FROM
    ms_structure 
WHERE
    structure_group_id = @StructureGroupId 
    AND delete_flg = 0
    AND parent_structure_id IN ( 
        SELECT
            * 
        FROM
            dbo.get_splitText(@StructureIdList, default, default)
    ) 
