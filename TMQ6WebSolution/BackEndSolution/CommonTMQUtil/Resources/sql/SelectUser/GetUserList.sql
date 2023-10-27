SELECT
    users.display_name AS user_name, -- 担当者名(ユーザ表示名)
    users.user_id                    -- 担当者ID(ユーザID)
FROM
    ms_user users
WHERE
    EXISTS(
        SELECT
            *
        FROM
            (
                SELECT DISTINCT
                    user_id
                FROM
                    ms_user_belong belong
                WHERE
                    EXISTS(
                        SELECT
                            *
                        FROM
                            ms_user_belong my_belong
                        WHERE
                            belong.location_structure_id = my_belong.location_structure_id
                        AND my_belong.user_id = @UserId
                    )
            ) belong
        WHERE
            users.user_id = belong.user_id
    )

    AND users.delete_flg = 0

    /*@UserName
     -- 担当者名
    AND users.display_name LIKE '%'+ @UserName +'%'
    @UserName*/

    /*@MailAddress
     -- メールアドレス
    AND users.mail_address LIKE '%'+ @MailAddress +'%'
    @MailAddress*/
