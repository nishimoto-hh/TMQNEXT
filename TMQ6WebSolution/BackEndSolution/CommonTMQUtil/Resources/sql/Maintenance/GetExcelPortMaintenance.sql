SELECT
    su.summary_id
    , su.activity_division
    , ( 
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
                    st_f.structure_id = ad.structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = ad.structure_id
    ) AS activity_division_name
    , su.location_structure_id
    , su.job_structure_id
    , re.request_no
    , su.subject
    , su.plan_implementation_content
    , su.subject_note
    , su.mq_class_structure_id
    , ( 
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
                    st_f.structure_id = su.mq_class_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = su.mq_class_structure_id
    ) AS mq_class_name
    , su.repair_cost_class_structure_id
    , ( 
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
                    st_f.structure_id = su.repair_cost_class_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = su.repair_cost_class_structure_id
    ) AS repair_cost_class_name
    , su.budget_management_structure_id
    , ( 
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
                    st_f.structure_id = su.budget_management_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = su.budget_management_structure_id
    ) AS budget_management_name
    , su.budget_personality_structure_id
    , ( 
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
                    st_f.structure_id = su.budget_personality_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = su.budget_personality_structure_id
    ) AS budget_personality_name
    , su.sudden_division_structure_id
    , ( 
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
                    st_f.structure_id = su.sudden_division_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = su.sudden_division_structure_id
    ) AS sudden_division_name
    , su.stop_system_structure_id
    , ( 
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
                    st_f.structure_id = su.stop_system_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = su.stop_system_structure_id
    ) AS stop_system_name
    , su.stop_time
    , su.maintenance_count
    , su.change_management_structure_id
    , ( 
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
                    st_f.structure_id = su.change_management_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = su.change_management_structure_id
    ) AS change_management_name
    , su.env_safety_management_structure_id
    , ( 
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
                    st_f.structure_id = su.env_safety_management_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = su.env_safety_management_structure_id
    ) AS env_safety_management_name
    , re.request_content
    , re.issue_date
    , re.urgency_structure_id
    , ( 
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
                    st_f.structure_id = re.urgency_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = re.urgency_structure_id
    ) AS urgency_name
    , re.discovery_methods_structure_id
    , ( 
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
                    st_f.structure_id = re.discovery_methods_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = re.discovery_methods_structure_id
    ) AS discovery_methods_name
    , re.desired_start_date
    , re.desired_end_date
    , re.request_department_clerk_id
    , ( 
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
                    st_f.structure_id = re.request_department_clerk_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = re.request_department_clerk_id
    ) AS request_department_clerk_name
    , re.request_personnel_id
    , COALESCE( 
        u_personnel.display_name
        , re.request_personnel_name
    ) AS request_personnel_name
    , re.request_personnel_tel
    , re.request_department_chief_id
    , COALESCE( 
        u_chief.display_name
        , re.request_department_chief_name
    ) AS request_department_chief_name
    , re.request_department_manager_id
    , COALESCE( 
        u_manager.display_name
        , re.request_department_manager_name
    ) AS request_department_manager_name
    , re.request_department_foreman_id
    , COALESCE( 
        u_foreman.display_name
        , re.request_department_foreman_name
    ) AS request_department_foreman_name
    , re.maintenance_department_clerk_id
    , ( 
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
                    st_f.structure_id = re.maintenance_department_clerk_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = re.maintenance_department_clerk_id
    ) AS maintenance_department_clerk_name
    , re.request_reason
    , re.examination_result
    , re.construction_division_structure_id
    , ( 
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
                    st_f.structure_id = re.construction_division_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = re.construction_division_structure_id
    ) AS construction_division_name
    , pl.subject AS plan_subject
    , pl.occurrence_date
    , pl.expected_construction_date
    , pl.expected_completion_date
    , pl.total_budget_cost
    , pl.plan_man_hour
    , pl.responsibility_structure_id
    , ( 
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
                    st_f.structure_id = pl.responsibility_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = pl.responsibility_structure_id
    ) AS responsibility_name
    , pl.failure_effect
    , su.construction_date
    , su.completion_date
    , hi.maintenance_season_structure_id
    , ( 
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
                    st_f.structure_id = hi.maintenance_season_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = hi.maintenance_season_structure_id
    ) AS maintenance_season_name
    , hi.call_count
    , hi.construction_company
    , hi.construction_personnel_id
    , COALESCE( 
        u_construction.display_name
        , hi.construction_personnel_name
    ) AS construction_personnel_name
    , hi.actual_result_structure_id
    , ( 
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
                    st_f.structure_id = hi.actual_result_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = hi.actual_result_structure_id
    ) AS actual_result_name
    , hi.loss_absence
    , hi.loss_absence_type_count
    , hi.maintenance_opinion
    , hi.working_time_self
    , hi.working_time_company
    , hi.total_working_time
    , hi.cost_note
    , hi.expenditure
    , hi.manufacturing_personnel_id
    , COALESCE( 
        u_manufacturing.display_name
        , hi.manufacturing_personnel_name
    ) AS manufacturing_personnel_name
    , hi.work_failure_division_structure_id
    , ( 
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
                    st_f.structure_id = hi.work_failure_division_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = hi.work_failure_division_structure_id
    ) AS work_failure_division_name
    , hi.occurrence_time
    , su.completion_time
    , hi.stop_count
    , hi.discovery_personnel
    , hi.effect_production_structure_id
    , ( 
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
                    st_f.structure_id = hi.effect_production_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = hi.effect_production_structure_id
    ) AS effect_production_name
    , hi.effect_quality_structure_id
    , ( 
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
                    st_f.structure_id = hi.effect_quality_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = hi.effect_quality_structure_id
    ) AS effect_quality_name
    , hi.failure_site
    , hi.parts_existence_flg
    , hi.working_time_research
    , hi.working_time_procure
    , hi.working_time_repair
    , hi.working_time_test 
FROM
    ( 
        SELECT
            *
            , dbo.get_target_layer_id(location_structure_id, 1) AS factory_id 
        FROM
            ma_summary
    ) su 
    LEFT JOIN ma_request re 
        ON su.summary_id = re.summary_id 
    LEFT JOIN ma_plan pl 
        ON su.summary_id = pl.summary_id 
    LEFT JOIN ma_history hi 
        ON su.summary_id = hi.summary_id 
    LEFT JOIN activity_division ad 
        ON CAST(su.activity_division AS nvarchar) = ad.extension_data 
    LEFT JOIN ms_user u_personnel 
        ON re.request_personnel_id = u_personnel.user_id 
    LEFT JOIN ms_user u_chief 
        ON re.request_department_chief_id = u_chief.user_id 
    LEFT JOIN ms_user u_manager 
        ON re.request_department_manager_id = u_manager.user_id 
    LEFT JOIN ms_user u_foreman 
        ON re.request_department_foreman_id = u_foreman.user_id 
    LEFT JOIN ms_user u_construction 
        ON hi.construction_personnel_id = u_construction.user_id 
    LEFT JOIN ms_user u_manufacturing 
        ON hi.manufacturing_personnel_id = u_manufacturing.user_id 
WHERE
    EXISTS ( 
        SELECT
            * 
        FROM
            #temp_structure_selected temp 
        WHERE
            temp.structure_group_id = 1000 
            AND temp.structure_id = su.location_structure_id
    ) 
    AND EXISTS ( 
        SELECT
            * 
        FROM
            #temp_structure_selected temp 
        WHERE
            temp.structure_group_id = 1010 
            AND temp.structure_id = su.job_structure_id
    )
