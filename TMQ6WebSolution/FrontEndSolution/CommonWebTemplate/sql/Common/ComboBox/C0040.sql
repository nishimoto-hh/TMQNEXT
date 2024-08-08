-- 機器別管理基準標準-割当画面の検索条件にある「機器別管理基準」コンボボックスを生成するためのもの
-- アイテムは「あり」「なし」の2つ

-- 「あり」のデータを取得
SELECT
    0 AS factoryId
    , 0 AS translationFactoryId
    , 2 AS 'values'
    , ( 
        SELECT
            tra.translation_text 
        FROM
            ms_translation tra 
        WHERE
            tra.location_structure_id = 0 
            AND tra.language_id = /*languageId*/'ja'
            AND tra.translation_id = 131010008
    ) AS labels
    , 0 AS deleteFlg
    , 0 AS orderFactoryId

-- 「なし」のデータを取得
UNION 
SELECT
    0 AS factoryId
    , 0 AS translationFactoryId
    , 1 AS 'values'
    , ( 
        SELECT
            tra.translation_text 
        FROM
            ms_translation tra 
        WHERE
            tra.location_structure_id = 0 
            AND tra.language_id = /*languageId*/'ja'
            AND tra.translation_id = 131210001
    ) AS labels
    , 0 AS deleteFlg
    , 0 AS orderFactoryId 
ORDER BY
    'values'
