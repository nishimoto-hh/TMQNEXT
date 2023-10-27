/*
 ユーザー所属マスタ(ms_user_belong)新規登録SQL
*/
INSERT 
INTO ms_user_belong( 
    user_id
    , location_structure_id
    , duty_flg
    , update_serialid
    , delete_flg
    , insert_datetime
    , insert_user_id
    , update_datetime
    , update_user_id
) 
SELECT
    @UserId
    , @FactoryId
    , @DutyFlg
    , @UpdateSerialid
    , @DeleteFlg
    , @InsertDatetime
    , @InsertUserId
    , @UpdateDatetime
    , @UpdateUserId
WHERE
    NOT EXISTS ( 
        SELECT
            1 
        FROM
            ms_user_belong 
        WHERE
            user_id = @UserId
            AND location_structure_id = @FactoryId
    )