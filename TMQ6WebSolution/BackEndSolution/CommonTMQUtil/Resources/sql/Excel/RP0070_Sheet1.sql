SELECT
    lp.location_structure_id                            -- 機種階層id ※共通処理にて使用
    -- , '' AS factory_name                                -- 工場 ※共通処理にて設定
    -- , '' AS plant_name                                  -- プラント ※共通処理にて設定
    -- , '' AS series_name                                 -- 系列 ※共通処理にて設定
    -- , '' AS stroke_name                                 -- 工程 ※共通処理にて設定
    -- , '' AS facility_name                               -- 設備 ※共通処理にて設定
    --地区(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.location_district_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.location_district_structure_id
    ) AS district_name
    --工場(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.location_factory_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.location_factory_structure_id
    ) AS factory_name
    --プラント(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.location_plant_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.location_plant_structure_id
    ) AS plant_name
    --系列(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.location_series_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.location_series_structure_id
    ) AS series_name
    --工程(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.location_stroke_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.location_stroke_structure_id
    ) AS stroke_name
    --設備(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.location_facility_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.location_facility_structure_id
    ) AS facility_name

    , '' AS long_plan_group                             -- 長計グループ
    , '' AS subject_group_no                            -- 件名グループ内No
--    , lp.maintenance_season_structure_id              -- 時期
--    , vssn.translation_text AS season_name              -- 時期名称
    -- , [dbo].[get_v_structure_item](lp.maintenance_season_structure_id, temp.factoryId, temp.languageId) AS season_name              -- 時期名称
    --時期名称(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.maintenance_season_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.maintenance_season_structure_id
    ) AS season_name

    , lp.person_name                                    -- 長計担当者
    , '' AS subject_no                                  -- 件名NO
    , lp.subject                                        -- 長計件名
    , '' AS long_plan                                   -- 長計
    , lp.job_structure_id                               -- 職種機種階層id ※共通処理にて使用
    -- , '' AS large_classfication_name                    -- 機種大分類 ※共通処理にて設定
    -- , '' AS middle_classfication_name                   -- 機種中分類 ※共通処理にて設定
    -- , '' AS small_classfication_name                    -- 機種小分類 ※共通処理にて設定
    --職種(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.job_kind_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.job_kind_structure_id
    ) AS job_name
    --機種大分類(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.job_large_classfication_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.job_large_classfication_structure_id
    ) AS large_classfication_name
    --機種中分類(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.job_middle_classfication_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.job_middle_classfication_structure_id
    ) AS middle_classfication_name
    --機種小分類(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.job_small_classfication_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.job_small_classfication_structure_id
    ) AS small_classfication_name

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
    -- , [dbo].[get_v_structure_item](lp.work_item_structure_id, temp.factoryId, temp.languageId) AS work_item_name                    -- 作業項目名称
    --作業項目名称(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.work_item_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.work_item_structure_id
    ) AS work_item_name
    -- , [dbo].[get_v_structure_item](lp.budget_management_structure_id, temp.factoryId, temp.languageId) AS budget_management_name    -- 予算管理区分名称
    --予算管理区分名称(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.budget_management_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.budget_management_structure_id
    ) AS budget_management_name
    -- , [dbo].[get_v_structure_item](lp.budget_personality_structure_id, temp.factoryId, temp.languageId) AS budget_personality_name  -- 予算性格区分名称
    --予算性格区分名称(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.budget_personality_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.budget_personality_structure_id
    ) AS budget_personality_name
    -- , [dbo].[get_v_structure_item](lp.purpose_structure_id, temp.factoryId, temp.languageId) AS purpose_name                        -- 目的区分名称
    --目的区分名称(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.purpose_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.purpose_structure_id
    ) AS purpose_name
    --, [dbo].[get_v_structure_item](lp.work_class_structure_id, temp.factoryId, temp.languageId) AS work_class_name                  -- 作業区分名称
    --作業区分名称(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.work_class_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.work_class_structure_id
    ) AS work_class_name
    --, [dbo].[get_v_structure_item](lp.treatment_structure_id, temp.factoryId, temp.languageId) AS treatment_name                    -- 処置区分名称
    --処置区分名称(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.treatment_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.treatment_structure_id
    ) AS treatment_name
    --, [dbo].[get_v_structure_item](lp.facility_structure_id, temp.factoryId, temp.languageId) AS facility_class_name                -- 設備区分名称
    --設備区分名称(翻訳)
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lp.facility_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = lp.facility_structure_id
    ) AS facility_class_name

    , lp.subject_note                                   -- 件名メモ欄

    , '1' AS output_report_location_name_got_flg                -- 機能場所名称情報取得済フラグ（帳票用）
    , '1' AS output_report_job_name_got_flg                     -- 職種・機種名称情報取得済フラグ（帳票用）

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
