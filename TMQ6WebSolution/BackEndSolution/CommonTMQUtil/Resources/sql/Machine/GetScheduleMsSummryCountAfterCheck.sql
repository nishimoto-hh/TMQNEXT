SELECT COUNT(*)
FROM mc_maintainance_schedule ms           -- 保全スケジュール
    ,mc_maintainance_schedule_detail msd   -- 保全スケジュール詳細
WHERE ms.maintainance_schedule_id = msd.maintainance_schedule_id
AND ms.maintainance_schedule_id = @MaintainanceScheduleId
AND msd.summary_id IS NOT NULL
AND msd.schedule_date >= @StartDate
