SELECT
    schedule.maintainance_schedule_id,        -- 保全スケジュールid
    schedule.management_standards_content_id, -- 機器別管理基準内容id
    schedule.is_cyclic,                       -- 周期ありフラグ
    schedule.cycle_year,                      -- 周期(年)
    schedule.cycle_month,                     -- 周期(月)
    schedule.cycle_day,                       -- 周期(日)
    schedule.disp_cycle,                      -- 表示周期
    schedule.start_date                       -- 開始日
FROM
    mc_maintainance_schedule schedule
WHERE
    schedule.management_standards_content_id = @ManagementStandardsContentId