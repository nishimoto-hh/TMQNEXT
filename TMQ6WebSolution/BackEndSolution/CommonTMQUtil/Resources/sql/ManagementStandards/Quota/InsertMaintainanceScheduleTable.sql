-- 保全スケジュールテーブル登録
INSERT INTO mc_maintainance_schedule(
    [maintainance_schedule_id],        -- 保全スケジュールID
    [management_standards_content_id], -- 機器別管理基準内容ID
    [is_cyclic],                       -- 周期ありフラグ
    [cycle_year],                      -- 周期(年)
    [cycle_month],                     -- 周期(月)
    [cycle_day],                       -- 周期(日)
    [disp_cycle],                      -- 表示周期
    [start_date],                      -- 開始日
    [update_serialid],                 -- 更新シリアルID
    [insert_datetime],                 -- 登録日時
    [insert_user_id],                  -- 登録ユーザー
    [update_datetime],                 -- 更新日時
    [update_user_id]                   -- 更新ユーザー
)
VALUES