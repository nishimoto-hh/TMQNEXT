-- 割当時の登録情報を取得
SELECT
    detail.management_standards_detail_id              -- 機器別管理基準標準詳細ID
    , detail.inspection_site_name                      -- 部位名称
    , detail.inspection_site_importance_structure_id   -- 部位重要度ID
    , detail.inspection_site_conservation_structure_id -- 保全方式ID
    , detail.maintainance_division                     -- 保全区分ID
    , detail.inspection_content_name                   -- 保全項目名称
    , detail.maintainance_kind_structure_id            -- 点検種別ID
    , detail.budget_amount                             -- 予算金額
    , detail.schedule_type_structure_id                -- スケジュール管理基準ID
    , detail.preparation_period                        -- 準備期間(日)
    , detail.cycle_year                                -- 周期(年)
    , detail.cycle_month                               -- 周期(月)
    , detail.cycle_day                                 -- 周期(日)
    , detail.disp_cycle                                -- 表示周期
    , detail.remarks                                   -- 機器別管理基準備考
FROM
    mc_management_standards_detail detail 
WHERE
    detail.management_standards_id = @ManagementStandardsId 
ORDER BY
    -- 点検種別の昇順
    detail.maintainance_kind_structure_id ASC

    -- 周期の小さい順(年・月・日を加算した日付の昇順)
    , dateadd( 
        DAY
        , coalesce(detail.cycle_day, 0)
        , dateadd( 
            MONTH
            , coalesce(detail.cycle_month, 0)
            , dateadd( 
                YEAR
                , coalesce(detail.cycle_year, 0)
                , '1999/01/01'
            )
        )
    ) ASC
