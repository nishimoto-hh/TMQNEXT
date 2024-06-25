/*
* スケジュールの内容を機器別管理基準内容IDより取得するSQL
*/
WITH schedule_start_date AS(
SELECT
    schedule.management_standards_content_id,
    schedule.cycle_year,
    schedule.cycle_month,
    schedule.cycle_day,
    schedule.start_date,
    schedule.update_datetime
FROM
    mc_maintainance_schedule AS schedule
WHERE
    schedule.management_standards_content_id IN @KeyIdList
    -- 同一の機器別管理基準内容IDで最大の開始日時のものを選択
AND NOT EXISTS(
        SELECT
            *
        FROM
            mc_maintainance_schedule AS sub
        WHERE
            schedule.management_standards_content_id = sub.management_standards_content_id
        AND schedule.start_date < sub.start_date
    )
)

    SELECT
    main.management_standards_content_id,
    main.cycle_year,
    main.cycle_month,
    main.cycle_day,
    main.start_date,
    main.update_datetime
    FROM
        schedule_start_date AS main
    WHERE
        NOT EXISTS(
            SELECT
                *
            FROM
                schedule_start_date AS sub
            WHERE
                main.management_standards_content_id = sub.management_standards_content_id
            AND main.start_date = sub.start_date
            AND main.update_datetime < sub.update_datetime
        )