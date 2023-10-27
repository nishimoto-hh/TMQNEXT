INSERT INTO ms_structure_unused(
    [structure_id]
    ,[factory_id]
    ,[structure_group_id]
    ,[insert_datetime]
    ,[insert_user_id]
    ,[update_datetime]
    ,[update_user_id]
)
VALUES(
    @StructureId
    ,@FactoryId
    ,@StructureGroupId
    ,@InsertDatetime
    ,@InsertUserId
    ,@UpdateDatetime
    ,@UpdateUserId
)
