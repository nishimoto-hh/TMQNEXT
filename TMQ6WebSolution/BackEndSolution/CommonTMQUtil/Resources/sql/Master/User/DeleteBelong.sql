/*
 ユーザー所属マスタ(ms_user_belong)削除SQL
*/
DELETE 
FROM
    ms_user_belong 
WHERE
    user_id = @UserId 
