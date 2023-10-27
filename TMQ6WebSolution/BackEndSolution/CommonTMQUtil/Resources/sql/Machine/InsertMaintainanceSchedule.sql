INSERT 
INTO mc_maintainance_schedule(
	  maintainance_schedule_id	--	保全スケジュールID
     ,management_standards_content_id	--	機器別管理基準内容ID
     ,is_cyclic	    --	周期ありフラグ
     ,cycle_year	--	周期(年)
     ,cycle_month	--	周期(月
     ,cycle_day	    --	周期(日)
     ,disp_cycle	--	表示周期
     ,start_date	--	開始日
     ,update_serialid                   -- 更新シリアルID
     ,insert_datetime                   -- 登録日時
     ,insert_user_id                    -- 登録ユーザー
     ,update_datetime                   -- 更新日時
     ,update_user_id                    -- 更新ユーザー
) 
OUTPUT inserted.maintainance_schedule_id
VALUES ( 
    NEXT VALUE FOR seq_mc_maintainance_schedule_maintainance_schedule_id     -- スケジュールid
     ,@ManagementStandardsContentId	--	機器別管理基準内容ID
     ,@IsCyclic	       --	周期ありフラグ
     ,@CycleYear	   --	周期(年)
     ,@CycleMonth	   --	周期(月
     ,@CycleDay	       --	周期(日)
     ,@DispCycle	   --	表示周期
     ,@StartDate	   --	開始日
    , 0                                         -- 更新シリアルID
    , @InsertDatetime                           -- 登録日時
    , @InsertUserId                             -- 登録ユーザー
    , @UpdateDatetime                           -- 更新日時
    , @UpdateUserId                             -- 更新ユーザー
); 
