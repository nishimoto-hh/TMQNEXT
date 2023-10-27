/*
 * ExcelPort 担当者コンボ用データリスト用SQL
 */
-- EPC0007
SELECT
    users.user_id AS id         -- 担当者ID(ユーザID)
    ,0 AS factory_id
    ,NULL AS parent_id
    ,users.display_name AS name -- 担当者名(ユーザ表示名)
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
                        AND my_belong.user_id = /*userId*/1001
                    )
            ) belong
        WHERE
            users.user_id = belong.user_id
    )

    AND users.delete_flg = 0
ORDER BY
    users.display_name
