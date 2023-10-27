/*
* オートコンプリート
* 翻訳マスタより名称に合致する値を候補に表示して選択させる
*/
WITH main AS ( 
    SELECT
        -- 工場ID
        tr.location_structure_id as factoryId,
        -- 翻訳工場ID
        tr.location_structure_id as translationFactoryId,
        -- コード
        tr.translation_id AS id,
        -- 名称
        tr.translation_text AS name,
        -- 行番号、表示行数を制限するのでソート順を指定
        row_number() over (ORDER BY tr.translation_id) row_num 
    FROM
        ms_translation AS tr 
    WHERE
        tr.language_id = /*languageId*/'ja'     -- 名称で検索
        /*IF param1 != null && param1 != ''*/
	        /*IF !getNameFlg */
	            -- 名称で検索
	            AND tr.translation_text LIKE '%'+/*param1*/'%'
	        /*END*/
        /*END*/

        /*IF getNameFlg */
            -- 翻訳なのでID
            AND tr.translation_id =/*param1*/0  
        /*END*/
        AND tr.location_structure_id = 0 
) 
SELECT
    factoryId,
    translationFactoryId,
    id AS 'values',
    name AS 'labels'
FROM
    main 
WHERE
/*IF rowLimit != null && rowLimit != ''*/
    row_num < /*rowLimit*/30 
/*END*/
