SELECT
    lp.location_structure_id                            -- 機種階層id ※共通処理にて使用
    , '' AS factory_name                                -- 工場 ※共通処理にて設定
    , '' AS plant_name                                  -- プラント ※共通処理にて設定
    , '' AS series_name                                 -- 系列 ※共通処理にて設定
    , '' AS stroke_name                                 -- 工程 ※共通処理にて設定
    , '' AS facility_name                               -- 設備 ※共通処理にて設定
    , '' AS long_plan_group                             -- 長計グループ
    , '' AS subject_group_no                            -- 件名グループ内No
--    , lp.maintenance_season_structure_id              -- 時期
--    , vssn.translation_text AS season_name              -- 時期名称
    , [dbo].[get_v_structure_item](lp.maintenance_season_structure_id, temp.factoryId, temp.languageId) AS season_name              -- 時期名称
    , lp.person_name                                    -- 長計担当者
    , '' AS subject_no                                  -- 件名NO
    , lp.subject                                        -- 長計件名
    , '' AS long_plan                                   -- 長計
    , lp.job_structure_id                               -- 職種機種階層id ※共通処理にて使用
    , '' AS large_classfication_name                    -- 機種大分類 ※共通処理にて設定
    , '' AS middle_classfication_name                   -- 機種中分類 ※共通処理にて設定
    , '' AS small_classfication_name                    -- 機種小分類 ※共通処理にて設定
--    , lp.work_item_structure_id                       -- 作業項目
--    , vwki.translation_text AS work_item_name           -- 作業項目名称
--    , lp.budget_management_structure_id               -- 予算管理区分
--    , vbdm.translation_text AS budget_management_name   -- 予算管理区分名称
--    , lp.budget_personality_structure_id              -- 予算性格区分
--    , vbdp.translation_text AS budget_personality_name  -- 予算性格区分名称
--    , lp.purpose_structure_id                         -- 目的区分
--    , vpps.translation_text AS purpose_name             -- 目的区分名称
--    , lp.work_class_structure_id                      -- 作業区分
--    , vwkc.translation_text AS work_class_name          -- 作業区分名称
--    , lp.treatment_structure_id                       -- 処置区分
--    , vtrt.translation_text AS treatment_name           -- 処置区分名称
--    , lp.facility_structure_id                        -- 設備区分
--    , vfac.translation_text AS facility_class_name      -- 設備区分名称
    , [dbo].[get_v_structure_item](lp.work_item_structure_id, temp.factoryId, temp.languageId) AS work_item_name                    -- 作業項目名称
    , [dbo].[get_v_structure_item](lp.budget_management_structure_id, temp.factoryId, temp.languageId) AS budget_management_name    -- 予算管理区分名称
    , [dbo].[get_v_structure_item](lp.budget_personality_structure_id, temp.factoryId, temp.languageId) AS budget_personality_name  -- 予算性格区分名称
    , [dbo].[get_v_structure_item](lp.purpose_structure_id, temp.factoryId, temp.languageId) AS purpose_name                        -- 目的区分名称
    , [dbo].[get_v_structure_item](lp.work_class_structure_id, temp.factoryId, temp.languageId) AS work_class_name                  -- 作業区分名称
    , [dbo].[get_v_structure_item](lp.treatment_structure_id, temp.factoryId, temp.languageId) AS treatment_name                    -- 処置区分名称
    , [dbo].[get_v_structure_item](lp.facility_structure_id, temp.factoryId, temp.languageId) AS facility_class_name                -- 設備区分名称
    , lp.subject_note                                   -- 件名メモ欄
FROM
    ln_long_plan lp
    INNER JOIN #temp temp
        ON lp.long_plan_id = temp.Key1
--    LEFT JOIN v_structure_item vssn     -- 時期
--        ON lp.maintenance_season_structure_id = vssn.structure_id
--    LEFT JOIN v_structure_item vwki     -- 作業項目
--        ON lp.work_item_structure_id = vwki.structure_id
--    LEFT JOIN v_structure_item vbdm     -- 予算管理区分
--        ON lp.budget_management_structure_id = vbdm.structure_id
--    LEFT JOIN v_structure_item vbdp     -- 予算性格区分
--        ON lp.budget_personality_structure_id = vbdp.structure_id
--    LEFT JOIN v_structure_item vpps     -- 目的区分
--        ON lp.purpose_structure_id = vpps.structure_id
--    LEFT JOIN v_structure_item vwkc     -- 作業区分
--        ON lp.work_class_structure_id = vwkc.structure_id
--    LEFT JOIN v_structure_item vtrt     -- 処置区分
--        ON lp.treatment_structure_id = vtrt.structure_id
--    LEFT JOIN v_structure_item vfac     -- 設備区分
--        ON lp.facility_structure_id = vfac.structure_id
ORDER BY
    -- 画面に合わせて件名で並び替え
    -- lp.long_plan_id                                     -- 機器番号
    lp.subject, lp.long_plan_id
