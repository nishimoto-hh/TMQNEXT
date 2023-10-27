/*
* 機種別仕様登録画面
* 仕様項目マスタ　更新
*/
UPDATE
    ms_spec
SET
     spec_type_id = @SpecTypeId
    ,spec_unit_type_id = @SpecUnitTypeId
    ,spec_unit_id = @SpecUnitId
    ,spec_num_decimal_places = @SpecNumDecimalPlaces
    ,translation_id = @TranslationId
    ,delete_flg = @DeleteFlg
    ,update_serialid = update_serialid + 1
    ,update_datetime = @UpdateDatetime
    ,update_user_id = @UpdateUserId
WHERE
    spec_id = @SpecId