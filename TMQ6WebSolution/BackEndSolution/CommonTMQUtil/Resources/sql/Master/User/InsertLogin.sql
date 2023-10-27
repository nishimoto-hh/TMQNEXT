/*
 ユーザーログインマスタ(ms_login)新規登録SQL
*/
INSERT 
INTO ms_login( 
    user_id
    , login_password
    , delete_flg
    , insert_datetime
    , insert_user_id
    , update_datetime
    , update_user_id
) 
VALUES ( 
    @UserId
    , ENCRYPTBYPASSPHRASE(@EncryptKey, @PassWord)
    , @DeleteFlg
    , @InsertDatetime
    , @InsertUserId
    , @UpdateDatetime
    , @UpdateUserId
)