SELECT
    llp.subject,                        -- 件名
    llp.subject_note,                   -- 件名メモ
    llp.location_structure_id,          -- 場所情報
    llp.job_structure_id,               -- 職種情報
    llp.budget_personality_structure_id,-- 予算性格区分
    llp.budget_management_structure_id, -- 予算管理区分
    llp.maintenance_season_structure_id -- 保全時期 
-- 保全時期
FROM
    mc_maintainance_schedule_detail masd -- 保全スケジュール詳細
    INNER JOIN
        mc_maintainance_schedule mas -- 保全スケジュール
    ON  masd.maintainance_schedule_id = mas.maintainance_schedule_id
    INNER JOIN
        mc_management_standards_content mmsc -- 機器別管理基準内容
    ON  mas.management_standards_content_id = mmsc.management_standards_content_id
    INNER JOIN
        ln_long_plan llp -- 長計件名
    ON  mmsc.long_plan_id = llp.long_plan_id
WHERE
    masd.maintainance_schedule_detail_id = @MaintainanceScheduleDetailId