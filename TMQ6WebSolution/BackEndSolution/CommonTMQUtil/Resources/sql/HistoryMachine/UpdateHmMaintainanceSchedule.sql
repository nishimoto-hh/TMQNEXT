UPDATE hm_mc_maintainance_schedule 
SET
    management_standards_content_id = @ManagementStandardsContentId -- 機器別管理基準内容id
    , is_cyclic = @IsCyclic                                         -- 周期ありフラグ
    , cycle_year = @CycleYear                                       -- 周期(年)
    , cycle_month = @CycleMonth                                     -- 周期(月)
    , cycle_day = @CycleDay                                         -- 周期(日)
    , disp_cycle = @DispCycle                                       -- 表示周期
    , start_date = @StartDate                                       -- 開始日
    , update_serialid = update_serialid+1                           -- 更新シリアルID
    , update_datetime = @UpdateDatetime                             -- 登録日時
    , update_user_id = @UpdateUserId                                -- 登録ユーザー
WHERE
    hm_maintainance_schedule_id = @HmMaintainanceScheduleId
