/*
 ユーザーログインマスタ(ms_login)削除SQL
*/
DELETE 
FROM
    ms_login 
WHERE
    user_id = @UserId

