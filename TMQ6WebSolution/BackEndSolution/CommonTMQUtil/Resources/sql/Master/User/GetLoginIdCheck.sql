/*
 ログインID重複チェック
*/
SELECT
    user_id
    , login_id 
FROM
    ms_user 
WHERE
    login_id = @LoginId