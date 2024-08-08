-- 保全スケジュール詳細テーブル登録
INSERT INTO mc_maintainance_schedule_detail(
    [maintainance_schedule_detail_id], -- 保全スケジュール詳細ID
    [maintainance_schedule_id],        -- 保全スケジュールID
    [sequence_count],                  -- 繰り返し回数
    [schedule_date],                   -- スケジュール日
    [complition],                      -- 完了フラグ
    [summary_id],                      -- 保全活動件名ID
    [update_serialid],                 -- 更新シリアルID
    [insert_datetime],                 -- 登録日時
    [insert_user_id],                  -- 登録ユーザー
    [update_datetime],                 -- 更新日時
    [update_user_id]                   -- 更新ユーザー
)
VALUES(
    NEXT VALUE FOR seq_mc_maintainance_schedule_detail_maintainance_schedule_detail_id, -- 保全スケジュール詳細ID
    @MaintainanceScheduleId,                                                            -- 保全スケジュールID
    @SequenceCount,                                                                     -- 繰り返し回数
    @ScheduleDate,                                                                      -- スケジュール日
    @Complition,                                                                        -- 完了フラグ
    @SummaryId,                                                                         -- 保全活動件名ID
    @UpdateSerialid,                                                                    -- 更新シリアルID
    @InsertDatetime,                                                                    -- 登録日時
    @InsertUserId,                                                                      -- 登録ユーザー
    @UpdateDatetime,                                                                    -- 更新日時
    @UpdateUserId                                                                       -- 更新ユーザー
)
