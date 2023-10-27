/*
 ユーザーマスタ(ms_user)新規登録SQL
*/
INSERT 
INTO ms_user( 
    user_id
    , login_id
    , language_id
    , authority_level_id
    , display_name
    , family_name
    , first_name
    , mail_address
    , update_serialid
    , delete_flg
    , insert_datetime
    , insert_user_id
    , update_datetime
    , update_user_id
) 
OUTPUT
    inserted.user_id 
VALUES ( 
    NEXT VALUE FOR seq_ms_user_user_id
    , @LoginId
    , @LanguageId
    , @AuthorityLevelId
    , @DisplayName
    , @FamilyName
    , @FirstName
    , @MailAddress
    , @UpdateSerialid
    , @DeleteFlg
    , @InsertDatetime
    , @InsertUserId
    , @UpdateDatetime
    , @UpdateUserId
)