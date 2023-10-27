SELECT
    location_structure_id 
FROM
    ms_user_belong 
WHERE
    user_id = @UserId 
    AND duty_flg = 1
