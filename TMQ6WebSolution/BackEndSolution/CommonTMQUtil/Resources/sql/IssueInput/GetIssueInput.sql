--******************************************************************
--出庫情報取得
--******************************************************************
WITH structure_factory AS(
-- 使用する構成グループの構成IDを絞込、工場の指定に用いる
    SELECT
         structure_id
        ,location_structure_id AS factory_id
    FROM
        v_structure_item
    WHERE
        structure_group_id IN(1730)
    AND language_id = @LanguageId
)
SELECT DISTINCT
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = ppt.unit_structure_id
                AND st_f.factory_id IN(0, ppt.factory_id)
            )
        AND tra.structure_id = ppt.unit_structure_id
    ) AS unit_translation_text, --単位
    ppt.parts_id
FROM
    pt_parts ppt
WHERE
    parts_id = @PartsId

