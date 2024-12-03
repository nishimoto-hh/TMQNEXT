SELECT
     target.long_plan_id
    ,target.subject
    ,target.subject_note
    ,target.location_structure_id
    ,target.district_id
    ,target.factory_id
    ,target.plant_id
    ,target.series_id
    ,target.stroke_id
    ,target.facility_id
    ,target.job_structure_id
    ,target.job_id
    ,target.large_classfication_id
    ,target.middle_classfication_id
    ,target.small_classfication_id
    ,target.maintenance_season_structure_id
    ,target.person_id
    ,target.person_name
    ,target.work_item_structure_id
    ,target.budget_management_structure_id
    ,target.budget_personality_structure_id
    ,target.long_plan_division_structure_id
    ,target.long_plan_group_structure_id

/*@UnExcelPort
    ,target.file_link_equip
    ,target.file_link_subject
@UnExcelPort*/
    ,target.purpose_structure_id
    ,target.work_class_structure_id
    ,target.treatment_structure_id
    ,target.facility_structure_id
/*@UnExcelPort
    ,target.update_serialid
    ,target.long_plan_id_dt
    ,target.mc_man_st_con_update_datetime
    ,target.sche_detail_update_datetime
    ,target.attachment_update_datetime
    ,target.key_id
    ,target.preparation_flg
@UnExcelPort*/
    -- 翻訳
    -- 保全時期
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.maintenance_season_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.maintenance_season_structure_id
    ) AS maintenance_season_name
    -- 作業項目
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.work_item_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.work_item_structure_id
    ) AS work_item_name
    -- 予算管理区分
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.budget_management_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.budget_management_structure_id
    ) AS budget_management_name
    -- 予算性格区分
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.budget_personality_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.budget_personality_structure_id
    ) AS budget_personality_name
    -- 目的区分
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.purpose_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.purpose_structure_id
    ) AS purpose_name
    -- 作業区分
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.work_class_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.work_class_structure_id
    ) AS work_class_name
    -- 処置区分
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.treatment_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.treatment_structure_id
    ) AS treatment_name
    -- 設備区分
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.facility_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.facility_structure_id
    ) AS facility_structure_name
    -- 長計区分
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.long_plan_division_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.long_plan_division_structure_id
    ) AS long_plan_division_name
    -- 長計グループ
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.long_plan_group_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.long_plan_group_structure_id
    ) AS long_plan_group_name
    -- 地区
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.district_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.district_id
    ) AS district_name
    -- 工場
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.factory_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.factory_id
    ) AS factory_name
    -- プラント
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.plant_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.plant_id
    ) AS plant_name
    -- 系列
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.series_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.series_id
    ) AS series_name
    -- 工程
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.stroke_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.stroke_id
    ) AS stroke_name
    -- 設備
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.facility_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.facility_id
    ) AS facility_name
    -- 職種
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.job_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.job_id
    ) AS job_name
    -- 機種大分類
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.large_classfication_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.large_classfication_id
    ) AS large_classfication_name
    -- 機種中分類
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.middle_classfication_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.middle_classfication_id
    ) AS middle_classfication_name
    -- 機種小分類
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.small_classfication_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.small_classfication_id
    ) AS small_classfication_name
FROM
    target