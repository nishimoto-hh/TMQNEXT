/*
 * 予備品の名称を取得
 * オートコンプリートだが、コード+翻訳のために定義
 */
SELECT
     parts_id AS 'values'
    ,parts_name AS 'labels'
FROM
    pt_parts
WHERE
    1 = 1
/*IF getNameFlg */
AND parts_id =/*param1*/0
/*END*/
