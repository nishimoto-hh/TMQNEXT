--******************************************************************
--ユーザーIDより本務工場を取得する
--******************************************************************
SELECT
    location_structure_id AS factory_id
FROM
    ms_user_belong 
WHERE
    user_id = @UserId
    AND duty_flg = 1 --本務工場
