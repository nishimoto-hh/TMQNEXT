/*
 * 機能権限有無チェック
 */
SELECT COUNT(*)
FROM cm_conduct cc

/*IF UserId != null */
 INNER JOIN ms_user_conduct_authority mu
     ON cc.conduct_id = mu.conduct_id
/*END*/

WHERE cc.conduct_id = /*ConductId*/'MS1000'
/*IF UserId != null */
    AND mu.user_id = /*UserId*/1001
--ELSE cc.delete_flg != 1
/*END*/
