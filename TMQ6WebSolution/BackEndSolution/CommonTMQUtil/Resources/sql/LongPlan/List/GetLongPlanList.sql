SELECT
     target.long_plan_id
     -- 件名
    ,target.subject
    -- 件名メモ
    /*@SubjectNote
    ,target.subject_note
    @SubjectNote*/
    -- 担当
    /*@PersonName
    ,target.person_name
    @PersonName*/

/*@UnList
    -- 場所階層
    ,target.location_structure_id
    ,target.district_id
    ,target.factory_id
    ,target.plant_id
    ,target.series_id
    ,target.stroke_id
    ,target.facility_id
    -- 職種機種
    ,target.job_structure_id
    ,target.job_id
    ,target.large_classfication_id
    ,target.middle_classfication_id
    ,target.small_classfication_id
    ,target.maintenance_season_structure_id
    -- 作業項目
    ,target.work_item_structure_id
    -- 担当
    ,target.person_id
    -- 予算管理区分
    ,target.budget_management_structure_id
    -- 予算性格区分
    ,target.budget_personality_structure_id
    -- 長計区分
    ,target.long_plan_division_structure_id
    -- 長計グループ
    ,target.long_plan_group_structure_id
    -- 目的区分
    ,target.purpose_structure_id
    -- 作業区分
    ,target.work_class_structure_id
    -- 処置区分
    ,target.treatment_structure_id
    -- 設備区分
    ,target.facility_structure_id
@UnList*/

/*@UnExcelPort
    /*@FileLinkEquip
    ,target.file_link_equip
    @FileLinkEquip*/
    /*@FileLinkSubject
    ,target.file_link_subject
    @FileLinkSubject*/

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
    /*@MaintenanceSeasonName
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
    @MaintenanceSeasonName*/
    -- 作業項目
    /*@WorkItemName
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
    @WorkItemName*/
    -- 予算管理区分
    /*@BudgetManagementName
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
    @BudgetManagementName*/
    -- 予算性格区分
    /*@BudgetPersonalityName
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
    @BudgetPersonalityName*/
    -- 目的区分
    /*@PurposeName
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
    @PurposeName*/
    -- 作業区分
    /*@WorkClassName
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
    @WorkClassName*/
    -- 処置区分
    /*@TreatmentName
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
    @TreatmentName*/
    -- 設備区分
    /*@FacilityStructureName
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
    @FacilityStructureName*/
    -- 長計区分
    /*@LongPlanDivisionName
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
    @LongPlanDivisionName*/
    -- 長計グループ
    /*@LongPlanGroupName
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
    @LongPlanGroupName*/
    -- 地区
    /*@DistrictName
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
    @DistrictName*/
    -- 工場
    /*@FactoryName
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
    @FactoryName*/
    -- プラント
    /*@PlantName
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
    @PlantName*/
    -- 系列
    /*@SeriesName
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
    @SeriesName*/
    -- 工程
    /*@StrokeName
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
    @StrokeName*/
    -- 設備
    /*@FacilityName
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
    @FacilityName*/
    -- 職種
    /*@JobName
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
    @JobName*/
    -- 機種大分類
    /*@LargeClassficationName
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
    @LargeClassficationName*/
    -- 機種中分類
    /*@MiddleClassficationName
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
    @MiddleClassficationName*/
    -- 機種小分類
    /*@SmallClassficationName
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
    @SmallClassficationName*/
FROM
    target