SELECT
     target.machine_id
    ,target.machine_no
    ,target.inspection_site_structure_id
    ,target.inspection_content_structure_id
    ,FORMAT(target.budget_amount, '#,###') AS budget_amount_format
    ,target.schedule_type_structure_id
    ,target.cycle_year AS cycle_year_format
    ,target.cycle_month AS cycle_month_format
    ,target.cycle_day AS cycle_day_format
    ,target.start_date
    ,target.maintainance_kind_structure_id
    ,target.management_standards_content_id
    ,target.update_serialid_content
    ,target.machine_name
    ,target.management_standards_component_id
    -- 翻訳
    -- 保全部位
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.inspection_site_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.inspection_site_structure_id
    ) AS inspection_site_name
    -- 保全項目
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.inspection_content_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.inspection_content_structure_id
    ) AS inspection_content_name
    -- スケジュール管理
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.schedule_type_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.schedule_type_structure_id
    ) AS schedule_type_name
    -- 点検種別
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.maintainance_kind_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.maintainance_kind_structure_id
    ) AS maintainance_kind_name
FROM
    target