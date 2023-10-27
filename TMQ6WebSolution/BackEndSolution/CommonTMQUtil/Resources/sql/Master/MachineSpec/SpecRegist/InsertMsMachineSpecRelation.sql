/*
* 機種別仕様登録画面
* 機種別仕様関連付けマスタ　新規登録
*/
INSERT INTO ms_machine_spec_relation(
     machine_spec_relation_id
    ,location_structure_id
    ,job_structure_id
    ,spec_id
    ,display_order
    ,insert_datetime
    ,insert_user_id
    ,update_datetime
    ,update_user_id
) OUTPUT inserted.machine_spec_relation_id
VALUES(
     NEXT VALUE FOR seq_ms_machine_spec_relation_machine_spec_relation_id
    ,@LocationStructureId
    ,@JobStructureId
    ,@SpecId
    ,@DisplayOrder
    ,@InsertDatetime
    ,@InsertUserId
    ,@UpdateDatetime
    ,@UpdateUserId
)