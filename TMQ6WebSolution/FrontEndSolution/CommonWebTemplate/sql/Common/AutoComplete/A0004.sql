/*
 * アイテム翻訳を取得
 * オートコンプリートだが、コード+翻訳のために定義
 */
WITH main AS ( 
    SELECT
        structure_item_id AS id
        , translation_text AS name
        , row_number() over (ORDER BY location_structure_id DESC) row_num 
    FROM
        v_structure_item_all 
    WHERE
        language_id = /*languageId*/'ja' 
    /*IF param1 == null && param1 == ''*/
        AND location_structure_id = 0 
    /*END*/
    /*IF param1 != null && param1 != ''*/
        AND ( 
            location_structure_id = 0 
            OR CONVERT(NVARCHAR, location_structure_id) = /*param1*/'5'
        ) 
    /*END*/
    /*IF getNameFlg */
        AND structure_item_id = /*param2*/0 
    /*END*/
) 
SELECT
    id AS 'values'
    , name AS 'labels' 
FROM
    main 
WHERE
    row_num = 1
