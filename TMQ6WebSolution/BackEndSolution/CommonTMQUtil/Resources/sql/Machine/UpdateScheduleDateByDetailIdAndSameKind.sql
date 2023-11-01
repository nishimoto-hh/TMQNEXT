with schedule_date_by_machine as ( 
    -- 機IDに対するスケジュール情報を取得
    select
	    sch.maintainance_schedule_id
        , cont.management_standards_content_id
        , sch.start_date
        , det.complition
        , det.schedule_date 
    from
        mc_management_standards_component comp 
        left join mc_management_standards_content cont 
            on comp.management_standards_component_id = cont.management_standards_component_id 
        left join mc_maintainance_schedule sch 
            on cont.management_standards_content_id = sch.management_standards_content_id 
        left join mc_maintainance_schedule_detail det 
            on sch.maintainance_schedule_id = det.maintainance_schedule_id 
    where
        comp.machine_id = @MachineId
) 
, max_schedule_date_by_content as ( 
    -- 保全スケジュールID・内容ごとの保全活動が完了したデータの最大のスケジュール日を取得
    select
	    maintainance_schedule_id
        , management_standards_content_id
        , max(schedule_date) schedule_date 
    from
        schedule_date_by_machine 
    where
        complition = 1 
    group by
        maintainance_schedule_id, management_standards_content_id
) 
, next_date_exists_comp as ( 
    -- 保全スケジュールID・内容ごとの保全活動が完了したデータの最大のスケジュール日の次のスケジュール日を取得
    select
	    schedule_date_by_machine.maintainance_schedule_id
        , schedule_date_by_machine.management_standards_content_id
        , min(schedule_date_by_machine.schedule_date) schedule_date
    from
        schedule_date_by_machine 
        inner join max_schedule_date_by_content 
            on schedule_date_by_machine.maintainance_schedule_id = max_schedule_date_by_content.maintainance_schedule_id
			and schedule_date_by_machine.management_standards_content_id = max_schedule_date_by_content.management_standards_content_id
    where
        schedule_date_by_machine.schedule_date > max_schedule_date_by_content.schedule_date
	group by
	    schedule_date_by_machine.maintainance_schedule_id, schedule_date_by_machine.management_standards_content_id
) 
, next_date_not_exists_comp as ( 
    -- 保全スケジュールID・内容ごとの開始日より後のスケジュール日を取得(保全活動が完了していない)
    select
	    schedule_date_by_machine.maintainance_schedule_id
        , schedule_date_by_machine.management_standards_content_id
        , min(schedule_date_by_machine.schedule_date) schedule_date 
    from
        schedule_date_by_machine 
        left join ( 
            select
			    maintainance_schedule_id
                , management_standards_content_id
                , max(start_date) start_date 
            from
                schedule_date_by_machine 
            group by
                maintainance_schedule_id, management_standards_content_id
        ) max_start_date 
            on schedule_date_by_machine.maintainance_schedule_id = max_start_date.maintainance_schedule_id
			and schedule_date_by_machine.management_standards_content_id = max_start_date.management_standards_content_id
    where
        schedule_date_by_machine.start_date >= max_start_date.start_date 
    group by
        schedule_date_by_machine.maintainance_schedule_id, schedule_date_by_machine.management_standards_content_id
)
update mc_maintainance_schedule_detail 
set
    [schedule_date] = @scheduledate             -- 画面で入力された次回実施予定日
    , [update_serialid] = update_serialid + 1   -- 更新シリアルID
    , [update_datetime] = @updatedatetime       -- 更新日時
    , [update_user_id] = @updateuserid          -- 更新ユーザー
where
    maintainance_schedule_detail_id = ( 
        -- 初期表示時の次回実施予定日の保全スケジュール詳細IDを取得する
        select
            top 1 maintainance_schedule_detail_id 
        from
            mc_maintainance_schedule_detail detail 
        where
            maintainance_schedule_id = @MaintainanceScheduleId
            and schedule_date = ( 
                select
                    coalesce( 
                        next_date_exists_comp.schedule_date
                        , next_date_not_exists_comp.schedule_date
                    ) schedule_date 
                from
                    next_date_not_exists_comp 
                    left join next_date_exists_comp 
                        on next_date_not_exists_comp.maintainance_schedule_id = next_date_exists_comp.maintainance_schedule_id
                        and next_date_not_exists_comp.management_standards_content_id = next_date_exists_comp.management_standards_content_id
                where
                    next_date_not_exists_comp.maintainance_schedule_id = @MaintainanceScheduleId
                and next_date_not_exists_comp.management_standards_content_id = @ManagementStandardsContentId
            ) 
        order by
            schedule_date
    )
    -- 未完了のデータが更新対象
    and complition = 0
