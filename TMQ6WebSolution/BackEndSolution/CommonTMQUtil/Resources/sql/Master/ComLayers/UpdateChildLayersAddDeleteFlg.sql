UPDATE ms_structure
SET delete_flg = 1
   ,update_serialid = update_serialid + 1
   ,update_datetime = @UpdateDatetime
   ,update_user_id = @UpdateUserId
WHERE
    structure_group_id = @StructureGroupId 
    AND delete_flg = 0
    AND structure_id IN ( 
        SELECT
            * 
        FROM
            dbo.get_splitText(@StructureIdList, default, default)
    ) 
