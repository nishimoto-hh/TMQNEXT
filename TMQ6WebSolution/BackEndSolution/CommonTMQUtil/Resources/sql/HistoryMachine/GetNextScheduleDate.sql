with after_date as ( 
    select
        top 1 detail.schedule_date 
    from
        mc_management_standards_content cont 
        left join mc_maintainance_schedule sche 
            on cont.management_standards_content_id = sche.management_standards_content_id 
        left join mc_maintainance_schedule_detail detail 
            on sche.maintainance_schedule_id = detail.maintainance_schedule_id 
    where
        cont.management_standards_content_id = @ManagementStandardsContentId
        and sche.maintainance_schedule_id = @MaintainanceScheduleId
        and detail.schedule_date > @ScheduleDateBefore
    order by
        detail.schedule_date
) 
, before_date as ( 
    select
        top 1 detail.schedule_date 
    from
        mc_management_standards_content cont 
        left join mc_maintainance_schedule sche 
            on cont.management_standards_content_id = sche.management_standards_content_id 
        left join mc_maintainance_schedule_detail detail 
            on sche.maintainance_schedule_id = detail.maintainance_schedule_id 
    where
        cont.management_standards_content_id = @ManagementStandardsContentId 
        and sche.maintainance_schedule_id = @MaintainanceScheduleId
        and detail.schedule_date < @ScheduleDateBefore
    order by
        detail.schedule_date desc
) 
, main as ( 
    select
        1 as number
        , schedule_date 
    from
        after_date 
    union 
    select
        2 as number
        , schedule_date 
    from
        before_date
) 
select
    * 
from
    main 
order by
    number
