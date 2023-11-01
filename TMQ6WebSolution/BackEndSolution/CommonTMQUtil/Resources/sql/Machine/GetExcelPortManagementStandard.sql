select
    main.*
	-- 以下は次回実施予定日
	-- ●(保全活動が完了したデータ)が存在する場合は、最新の●の次の○のデータの日付
	-- ●が存在しない場合は開始日より後の○の日付
    , coalesce( 
        next_date_exists_comp.schedule_date
        , next_date_not_exists_comp.schedule_date
    ) schedule_date 
    , coalesce( 
        next_date_exists_comp.schedule_date
        , next_date_not_exists_comp.schedule_date
    ) schedule_date_before 
from
    main 
    left join next_date_exists_comp 
        on main.maintainance_schedule_id = next_date_exists_comp.maintainance_schedule_id
		and main.management_standards_content_id = next_date_exists_comp.management_standards_content_id
    left join next_date_not_exists_comp 
        on main.maintainance_schedule_id = next_date_not_exists_comp.maintainance_schedule_id
		and main.management_standards_content_id = next_date_not_exists_comp.management_standards_content_id 