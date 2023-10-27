/*
 * 機能ID一覧取得(ログイン処理用)
 */
SELECT DISTINCT
	 cc.conduct_id AS ConductId
FROM cm_conduct cc

/*IF UserId != null */
 INNER JOIN ms_user_conduct_authority mu
     ON cc.conduct_id = mu.conduct_id
/*END*/

WHERE cc.menu_division != 0
/*IF UserId != null */
    AND mu.user_id = /*UserId*/1001
/*END*/

ORDER BY cc.conduct_id
