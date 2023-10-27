/*
 ユーザー機能権限マスタ(ms_user_conduct_authority)新規登録SQL
*/
INSERT 
INTO ms_user_conduct_authority( 
    user_id
    , conduct_id
    , update_serialid
    , insert_datetime
    , insert_user_id
    , update_datetime
    , update_user_id
) 
VALUES ( 
    @UserId
    , @ConductId
    , @UpdateSerialid
    , @InsertDatetime
    , @InsertUserId
    , @UpdateDatetime
    , @UpdateUserId
)