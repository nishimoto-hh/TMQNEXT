DELETE 
FROM
    mc_maintainance_schedule_detail 
WHERE
    maintainance_schedule_id = @MaintainanceScheduleId 
    AND schedule_date >= @ScheduleDate
    AND complition <> 'True'