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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
            )
        AND tra.structure_id = lp.location_facility_structure_id
    ) AS facility_name

    , '' AS long_plan_group                             -- 長計グループ
    , '' AS subject_group_no                            -- 件名グループ内No
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
            )
        AND tra.structure_id = lp.job_small_classfication_structure_id
    ) AS small_classfication_name
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
            )
        AND tra.structure_id = lp.work_item_structure_id
    ) AS work_item_name
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
            )
        AND tra.structure_id = lp.budget_management_structure_id
    ) AS budget_management_name
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
            )
        AND tra.structure_id = lp.budget_personality_structure_id
    ) AS budget_personality_name
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
            )
        AND tra.structure_id = lp.purpose_structure_id
    ) AS purpose_name
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
            )
        AND tra.structure_id = lp.work_class_structure_id
    ) AS work_class_name
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
            )
        AND tra.structure_id = lp.treatment_structure_id
    ) AS treatment_name
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
            )
        AND tra.structure_id = lp.facility_structure_id
    ) AS facility_class_name




    --長計区分名称(翻訳)
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
                    st_f.structure_id = lp.long_plan_division_structure_id
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
            )
        AND tra.structure_id = lp.long_plan_division_structure_id
    ) AS long_plan_division_name
    --長計グループ名称(翻訳)
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
                    st_f.structure_id = lp.long_plan_group_structure_id
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
            )
        AND tra.structure_id = lp.long_plan_group_structure_id
    ) AS long_plan_group_name
    , lp.subject_note                                           -- 件名メモ欄
    , '1' AS output_report_location_name_got_flg                -- 機能場所名称情報取得済フラグ（帳票用）
    , '1' AS output_report_job_name_got_flg                     -- 職種・機種名称情報取得済フラグ（帳票用）

FROM
    ln_long_plan lp
    INNER JOIN #temp temp
        ON lp.long_plan_id = temp.Key1
ORDER BY
    -- 画面に合わせて件名で並び替え
    -- lp.long_plan_id                                     -- 機器番号
    lp.subject, lp.long_plan_id
