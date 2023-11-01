update mc_maintainance_schedule 
set
    [disp_cycle] = @DispCycle                   -- 表示周期
    , [update_serialid] = update_serialid + 1   -- 更新シリアルID
    , [update_datetime] = @UpdateDatetime       -- 更新日時
    , [update_user_id] = @UpdateUserId          -- 更新ユーザー
where
    maintainance_schedule_id = @MaintainanceScheduleId
