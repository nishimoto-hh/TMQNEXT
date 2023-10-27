--******************************************************************
--ユーザーIDよりパスワードを複合化して取得する
--******************************************************************
SELECT user_id
    ,REPLACE(CONVERT(NVARCHAR, DECRYPTBYPASSPHRASE('G9ifhNTt', login_password)), '_xS5ifag9', '') AS password
FROM ms_login
WHERE user_id = @UserId
