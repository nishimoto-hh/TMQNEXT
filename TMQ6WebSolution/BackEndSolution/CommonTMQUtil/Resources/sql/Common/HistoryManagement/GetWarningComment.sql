-- 一覧画面の上部に表示する警告コメント「（2024年3月以前は出力出来ません。）」を取得する
SELECT
    tra.translation_text AS comment 
FROM
    ms_translation tra 
WHERE
    location_structure_id = 0 
    AND tra.translation_id = 141100003 
    AND tra.language_id = @LanguageId
