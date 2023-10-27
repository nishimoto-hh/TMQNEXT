/*
 ユーザー役割マスタ(ms_user_role)削除SQL
*/
DELETE FROM
    ms_user_role
WHERE
   user_id = @UserId 