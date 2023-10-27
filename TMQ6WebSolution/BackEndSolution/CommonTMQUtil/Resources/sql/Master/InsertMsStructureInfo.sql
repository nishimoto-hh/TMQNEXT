INSERT INTO ms_structure(
    [structure_id]
    ,[factory_id]
    ,[structure_group_id]
    ,[parent_structure_id]
    ,[structure_layer_no]
    ,[structure_item_id]
    ,[delete_flg]
    ,[update_serialid]
    ,[insert_datetime]
    ,[insert_user_id]
    ,[update_datetime]
    ,[update_user_id]
) OUTPUT inserted.structure_id
VALUES(
    NEXT VALUE FOR seq_ms_structure_structure_id
    ,@FactoryId
    ,@StructureGroupId
    ,@ParentStructureId
    ,@StructureLayerNo
    ,@StructureItemId
    ,@DeleteFlg
    ,@UpdateSerialid
    ,@InsertDatetime
    ,@InsertUserId
    ,@UpdateDatetime
    ,@UpdateUserId
)
