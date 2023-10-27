SELECT
    display_name AS user_name
FROM
    ms_user mu
WHERE
    user_id = @UserId
