SELECT
     target.long_plan_id
    ,target.machine_id
    ,target.management_standards_component_id
    ,target.management_standards_content_id
    ,target.location_structure_id
    ,target.job_structure_id
    ,target.maintainance_schedule_id
    ,target.machine_no
    ,target.machine_name
    ,target.inspection_site_structure_id
    ,target.inspection_site_importance_structure_id
    ,target.inspection_site_conservation_structure_id
    ,target.inspection_content_structure_id
    ,target.cycle_year
    ,target.cycle_month
    ,target.cycle_day
    ,target.cycle_year AS cycle_year_display
    ,target.cycle_month AS cycle_month_display
    ,target.cycle_day AS cycle_day_display
    ,target.start_date
    ,target.budget_personality_structure_id
    ,target.list_group_id
    ,target.key_id
    -- 翻訳
    -- 部位
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
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.inspection_site_structure_id
                AND st_f.factory_id IN(0, target.machine_factory_id)
            )
        AND tra.structure_id = target.inspection_site_structure_id
    ) AS inspection_site_name
    -- 部位重要度
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
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.inspection_site_importance_structure_id
                AND st_f.factory_id IN(0, target.machine_factory_id)
            )
        AND tra.structure_id = target.inspection_site_importance_structure_id
    ) AS importance_name
    -- 保全方式
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
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.inspection_site_conservation_structure_id
                AND st_f.factory_id IN(0, target.machine_factory_id)
            )
        AND tra.structure_id = target.inspection_site_conservation_structure_id
    ) AS inspection_site_conservation_name
    -- 保全項目
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
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.inspection_content_structure_id
                AND st_f.factory_id IN(0, target.machine_factory_id)
            )
        AND tra.structure_id = target.inspection_content_structure_id
    ) AS inspection_content_name
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
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.budget_personality_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.budget_personality_structure_id
    ) AS budget_personality_name
FROM
    target