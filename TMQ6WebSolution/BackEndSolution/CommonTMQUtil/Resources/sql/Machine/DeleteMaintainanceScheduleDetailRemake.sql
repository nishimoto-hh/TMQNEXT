DELETE msd
FROM mc_maintainance_schedule ms
    ,mc_maintainance_schedule_detail msd
WHERE ms.maintainance_schedule_id = msd.maintainance_schedule_id
AND ms.maintainance_schedule_id = @MaintainanceScheduleId
AND msd.schedule_date >= @StartDate
AND msd.complition <> 'True'
