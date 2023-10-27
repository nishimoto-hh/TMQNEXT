/*
 ユーザー役割マスタ(ms_user_role)新規登録SQL
*/
INSERT 
INTO ms_user_role( 
    user_id
    , role_id
    , delete_flg
    , insert_datetime
    , insert_user_id
    , update_datetime
    , update_user_id
) 
VALUES ( 
    @UserId
    , @RoleId
    , @DeleteFlg
    , @InsertDatetime
    , @InsertUserId
    , @UpdateDatetime
    , @UpdateUserId
)
