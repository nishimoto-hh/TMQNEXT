/*
 ユーザー機能権限マスタ(ms_user_conduct_authority)削除SQL
*/
DELETE FROM
    ms_user_conduct_authority
WHERE
    user_id = @UserId 