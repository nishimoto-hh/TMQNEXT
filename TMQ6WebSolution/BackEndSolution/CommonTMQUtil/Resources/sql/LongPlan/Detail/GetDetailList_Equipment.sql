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
