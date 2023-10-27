/*
* 機種別仕様登録画面
* 仕様項目マスタ　新規登録
*/
INSERT INTO ms_spec(
     spec_id
    ,spec_type_id
    ,spec_unit_type_id
    ,spec_unit_id
    ,spec_num_decimal_places
    ,translation_id
    ,insert_datetime
    ,insert_user_id
    ,update_datetime
    ,update_user_id
) OUTPUT inserted.spec_id
VALUES(
     NEXT VALUE FOR seq_ms_spec_spec_id
    ,@SpecTypeId
    ,@SpecUnitTypeId
    ,@SpecUnitId
    ,@SpecNumDecimalPlaces
    ,@TranslationId
    ,@InsertDatetime
    ,@InsertUserId
    ,@UpdateDatetime
    ,@UpdateUserId
)