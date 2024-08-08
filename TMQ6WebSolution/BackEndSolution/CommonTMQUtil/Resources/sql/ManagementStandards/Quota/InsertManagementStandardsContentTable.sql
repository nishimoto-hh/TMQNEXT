-- 機器別管理基準内容テーブル登録
INSERT INTO mc_management_standards_content(
    [management_standards_content_id],           -- 機器別管理基準内容ID
    [management_standards_component_id],         -- 機器別管理基準部位ID
    [inspection_content_structure_id],           -- 点検内容ID
    [inspection_site_importance_structure_id],   -- 部位重要度ID
    [inspection_site_conservation_structure_id], -- 部位保全方式ID
    [maintainance_division],                     -- 保全区分ID
    [maintainance_kind_structure_id],            -- 点検種別ID
    [budget_amount],                             -- 予算金額
    [preparation_period],                        -- 準備期間(日)
    [long_plan_id],                              -- 長計件名ID
    [order_no],                                  -- 並び順
    [schedule_type_structure_id],                -- スケジュール管理基準ID
    [update_serialid],                           -- 更新シリアルID
    [insert_datetime],                           -- 登録日時
    [insert_user_id],                            -- 登録ユーザー
    [update_datetime],                           -- 更新日時
    [update_user_id]                             -- 更新ユーザー
)
VALUES