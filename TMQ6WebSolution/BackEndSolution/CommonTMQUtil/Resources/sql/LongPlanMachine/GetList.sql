SELECT
     target.long_plan_id
    ,target.machine_id
    ,target.management_standards_component_id
    ,target.management_standards_content_id
    ,target.maintainance_schedule_id
    ,target.list_group_id
    ,target.key_id
    ,0 AS header_flg
    -- 周期(非表示)
    ,target.cycle_year
    ,target.cycle_month
    ,target.cycle_day
    -- ソートキー
    ,target.inspection_site_structure_id
    ,target.inspection_site_importance_structure_id
    ,target.inspection_site_conservation_structure_id
    ,target.inspection_content_structure_id

    --,target.location_structure_id
    --,target.job_structure_id
    --,target.budget_personality_structure_id

    -- 機器番号
    /*@MachineNo
    ,target.machine_no
    @MachineNo*/
    -- 機器名称
    /*@MachineName
    ,target.machine_name
    @MachineName*/
    -- 周期(年)
    /*@CycleYearDisplay
    ,target.cycle_year AS cycle_year_display
    @CycleYearDisplay*/
    -- 周期(月)
    /*@CycleMonthDisplay
    ,target.cycle_month AS cycle_month_display
    @CycleMonthDisplay*/
    -- 周期(日)
    /*@CycleDayDisplay
    ,target.cycle_day AS cycle_day_display
    @CycleDayDisplay*/
    -- 基準日
    /*@StartDate
    ,target.start_date
    @StartDate*/

    -- 翻訳
    -- 部位
    /*@InspectionSiteName
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
                    st_f.structure_id = target.inspection_site_structure_id
                AND st_f.factory_id IN(0, target.machine_factory_id)
            )
        AND tra.structure_id = target.inspection_site_structure_id
    ) AS inspection_site_name
    @InspectionSiteName*/
    -- 部位重要度
    /*@ImportanceName
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
                    st_f.structure_id = target.inspection_site_importance_structure_id
                AND st_f.factory_id IN(0, target.machine_factory_id)
            )
        AND tra.structure_id = target.inspection_site_importance_structure_id
    ) AS importance_name
    @ImportanceName*/
    -- 保全方式
    /*@InspectionSiteConservationName
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
                    st_f.structure_id = target.inspection_site_conservation_structure_id
                AND st_f.factory_id IN(0, target.machine_factory_id)
            )
        AND tra.structure_id = target.inspection_site_conservation_structure_id
    ) AS inspection_site_conservation_name
    @InspectionSiteConservationName*/
    -- 保全項目
    /*@InspectionContentName
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
                    st_f.structure_id = target.inspection_content_structure_id
                AND st_f.factory_id IN(0, target.machine_factory_id)
            )
        AND tra.structure_id = target.inspection_content_structure_id
    ) AS inspection_content_name
    @InspectionContentName*/
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
FROM
    target