/*
 所属マスタより場所階層を取得する
*/
SELECT
    mub.location_structure_id 
    , mub.location_structure_id AS job_structure_id
    , mub.duty_flg
FROM
    ms_user_belong mub 
    INNER JOIN ms_structure msr 
        ON mub.location_structure_id = msr.structure_id 
        AND msr.structure_group_id = @StructureGroupId
WHERE
    mub.user_id = @UserId