/*
* オートコンプリートサンプル
* ファイル名はA0001.sqlのようにA+連番としないと、認識しない
* サンプルとしての説明なのでコメント文が多いですが、実装時は不要な内容は削除してください
*/
-- オートコンプリート対象のテーブル、基本的にこちらを変更
WITH main AS(
    SELECT
        user_id AS id, -- コード
        display_name AS name, -- 名称
        row_number() over(ORDER BY user_id) row_num -- 行番号、表示行数を制限するのでソート順を指定
    FROM
        ms_user
    WHERE
        1 = 1
        -- ①オートコンプリート時の条件
        -- 以下のparam1は画面の入力内容、SQLで他にparamが必要な場合は画面の入力は最後(項目定義で2つ値を指定したらparam3)
		/*IF param1 != null && param1 != ''*/
		/*IF !getNameFlg */
		-- APではオートコンプリートはコードまたは名称と一致したものを表示する仕様だった、TMQは名称のみで良い？
		    AND (user_id LIKE /*param1*/'%'
		         OR  display_name LIKE /*param1*/'%')
		/*END*/
		/*END*/
        
        -- ②指定されたコードより翻訳するときの条件、①でないときはこちら
		/*IF getNameFlg */
		   AND user_id =/*param1*/0
		/*END*/
)

-- 以下は固定、拡張項目が必要するなら追加する
SELECT
    id AS 'values',
    name AS 'labels'
FROM
    main
WHERE
/*IF rowLimit != null && rowLimit != ''*/
   row_num < /*rowLimit*/30
/*END*/
