/*
 * 件名別長期計画一覧　参照画面　保全情報一覧の検索　機器
*/
-- WITH句の続き
-- 機器で集計
,
base_group AS(
    SELECT
        base.machine_id
    FROM
        base
    GROUP BY
        base.machine_id
)
SELECT
    machine.machine_id,
    machine.machine_no,
    machine.machine_name,
    machine.importance_structure_id,
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
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = machine.importance_structure_id
                AND st_f.factory_id IN(0, machine.location_factory_structure_id)
            )
        AND tra.structure_id = machine.importance_structure_id
    ) AS importance_name,
    machine.attachment_update_datetime,
    machine.machine_id AS key_id -- スケジュールと同じ値
FROM
    base_group
    INNER JOIN
        machine
    ON  (
            machine.machine_id = base_group.machine_id
        )
ORDER BY
    -- ソートキーはビジネスロジックで指定
