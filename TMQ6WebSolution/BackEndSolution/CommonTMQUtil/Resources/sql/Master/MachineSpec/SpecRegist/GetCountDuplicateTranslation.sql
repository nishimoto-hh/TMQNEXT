/*
 機種別仕様関連付けマスタに設定されている翻訳で、同一工場で同じものがあるかどうかを判定するSQL
 ある場合は登録エラーとする
*/
-- 翻訳マスタ
WITH tr AS(
    SELECT
        translation_id
    FROM
        ms_translation
    WHERE
        (
            location_structure_id IN (0,@LocationStructureId)
        )
    AND language_id = @LanguageId
    AND translation_text = @TranslationText COLLATE Japanese_BIN
    AND delete_flg = 0
)
select
    count(*)
from
    ms_spec as spec
    INNER JOIN
        tr
    ON  spec.translation_id = tr.translation_id