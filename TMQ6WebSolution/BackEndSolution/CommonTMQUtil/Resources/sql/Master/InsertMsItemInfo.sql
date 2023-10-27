INSERT INTO ms_item(
    [item_id]
    ,[structure_group_id]
    ,[item_translation_id]
    ,[update_serialid]
    ,[insert_datetime]
    ,[insert_user_id]
    ,[update_datetime]
    ,[update_user_id]
) OUTPUT inserted.item_id
VALUES(
    NEXT VALUE FOR seq_ms_item_item_id
    ,@StructureGroupId
    ,@ItemTranslationId
    ,@UpdateSerialid
    ,@InsertDatetime
    ,@InsertUserId
    ,@UpdateDatetime
    ,@UpdateUserId
)
