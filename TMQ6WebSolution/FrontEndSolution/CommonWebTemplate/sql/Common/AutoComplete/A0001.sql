/*
 * ユーザの名称を取得
 * オートコンプリートだが、コード+翻訳のために定義
 */
SELECT
     user_id AS 'values'
    ,display_name AS 'labels'
FROM
    ms_user
WHERE
    1 = 1
/*IF getNameFlg */
AND user_id =/*param1*/0
/*END*/
