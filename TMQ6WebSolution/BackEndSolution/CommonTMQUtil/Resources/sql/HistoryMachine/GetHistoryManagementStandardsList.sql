SELECT
    cmachine.equipment_level_structure_id,              -- 機器レベル
    cmachine.machine_no,                                -- 機器番号
    cmachine.machine_name,                              -- 機器名称
    component.inspection_site_structure_id,            -- 保全部位
    content.inspection_site_importance_structure_id,   -- 部位重要度
    content.inspection_site_conservation_structure_id, -- 保全方式
    content.inspection_content_structure_id,           -- 保全項目
    content.maintainance_division,                     -- 保全区分
    content.maintainance_kind_structure_id,            -- 点検種別
    content.budget_amount,                             -- 予算金額
    content.preparation_period,                        -- 準備期間(日)
    content.schedule_type_structure_id                 -- スケジュール基準
FROM

    hm_history_management history -- 変更管理
    LEFT JOIN
        hm_history_management_detail detail -- 変更管理詳細
    ON  history.history_management_id = detail.history_management_id
    LEFT JOIN
        hm_mc_management_standards_component component -- 機器別管理機運部位変更管理
    ON  detail.history_management_detail_id = component.history_management_detail_id
    LEFT JOIN
        mc_machine cmachine -- 機番情報(トランザクション)
    ON  component.machine_id = cmachine.machine_id
    LEFT JOIN
        hm_mc_management_standards_content content -- 機器別管理基準内容変更管理
    ON  detail.history_management_detail_id = content.history_management_detail_id
    LEFT JOIN
    (
        SELECT
            schedule.*
        FROM
            hm_mc_maintainance_schedule AS schedule
            -- 保全スケジュール
            INNER JOIN
                -- 変更管理詳細IDごとの開始日最新データを取得
                (
                    SELECT
                        schedule_b.history_management_detail_id,
                        MAX(schedule_b.start_date) AS start_date,
                        MAX(schedule_b.update_datetime) AS update_datetime
                    FROM
                        hm_mc_maintainance_schedule schedule_b
                    GROUP BY
                        schedule_b.history_management_detail_id
                ) schedule_c
            ON  schedule.history_management_detail_id = schedule_c.history_management_detail_id
            AND (
                    schedule.start_date = schedule_c.start_date
                OR  schedule.start_date IS NULL
                AND schedule_c.start_date IS NULL
                    --null結合を考慮
                )
            AND (
                    schedule.update_datetime = schedule_c.update_datetime
                OR  schedule.update_datetime IS NULL
                AND schedule_c.update_datetime IS NULL
                    --null結合を考慮
                )
    ) schedule
ON  content.history_management_detail_id = schedule.history_management_detail_id
WHERE
    history.history_management_id = @HistoryManagementId
AND component.is_management_standard_conponent = 1 -- 機器別管理基準フラグ