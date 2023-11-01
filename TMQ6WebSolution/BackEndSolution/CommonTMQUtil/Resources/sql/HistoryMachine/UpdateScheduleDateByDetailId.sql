update mc_maintainance_schedule_detail 
set
    [schedule_date] = @ScheduleDate             -- 画面で入力された次回実施予定日
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
            and schedule_date = @ScheduleDateTransaction
        order by
            schedule_date
    )

-- 未完了のデータが更新対象
and complition = 0