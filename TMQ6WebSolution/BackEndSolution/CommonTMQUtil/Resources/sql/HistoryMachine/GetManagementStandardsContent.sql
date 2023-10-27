SELECT
    content.management_standards_content_id,           -- 機器別管理基準内容ID
    content.management_standards_component_id,         -- 機器別管理基準部位ID
    content.inspection_content_structure_id,           -- 点検内容id
    content.inspection_site_importance_structure_id,   -- 部位重要度
    content.inspection_site_conservation_structure_id, -- 部位保全方式
    content.maintainance_division,                     -- 保全区分
    content.maintainance_kind_structure_id,            -- 点検種別
    content.budget_amount,                             -- 予算金額
    content.preparation_period,                        -- 準備期間(日)
    content.long_plan_id,                              -- 長計件名ID
    content.order_no,                                  -- 並び順
    content.schedule_type_structure_id                 -- スケジュール管理基準
FROM
    mc_management_standards_content content
WHERE
    content.management_standards_component_id = @ManagementStandardsComponentId