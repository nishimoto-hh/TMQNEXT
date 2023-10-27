/*
* スケジュールの内容を機器別管理基準内容IDより取得するSQL
*/
SELECT
    schedule.management_standards_content_id,
    schedule.cycle_year,
    schedule.cycle_month,
    schedule.cycle_day,
    schedule.start_date
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
