--******************************************************************
--ユーザーIDより役割IDを取得する
--******************************************************************
SELECT
     CONVERT(nvarchar,role_id) AS role_id
FROM
    ms_user_role 
WHERE
    user_id = @UserId
