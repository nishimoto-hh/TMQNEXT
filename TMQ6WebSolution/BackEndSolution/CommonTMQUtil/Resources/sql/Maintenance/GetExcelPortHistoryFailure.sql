SELECT
    hf.history_failure_id
    , su.summary_id
    , su.location_structure_id                  --保全活動件名の場所階層
    , su.location_district_structure_id AS district_id
    , su.location_factory_structure_id AS factory_id
    , su.location_plant_structure_id AS plant_id
    , su.location_series_structure_id AS series_id
    , su.location_stroke_structure_id AS stroke_id
    , su.location_facility_structure_id AS facility_id
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
                    st_f.structure_id = su.location_district_structure_id
                AND st_f.factory_id IN(0, su.factory_id)
            )
        AND tra.structure_id = su.location_district_structure_id
    ) AS district_name
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
                    st_f.structure_id = su.location_factory_structure_id
                AND st_f.factory_id IN(0, su.factory_id)
            )
        AND tra.structure_id = su.location_factory_structure_id
    ) AS factory_name
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
                    st_f.structure_id = su.location_plant_structure_id
                AND st_f.factory_id IN(0, su.factory_id)
            )
        AND tra.structure_id = su.location_plant_structure_id
    ) AS plant_name
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
                    st_f.structure_id = su.location_series_structure_id
                AND st_f.factory_id IN(0, su.factory_id)
            )
        AND tra.structure_id = su.location_series_structure_id
    ) AS series_name
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
                    st_f.structure_id = su.location_stroke_structure_id
                AND st_f.factory_id IN(0, su.factory_id)
            )
        AND tra.structure_id = su.location_stroke_structure_id
    ) AS stroke_name
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
                    st_f.structure_id = su.location_facility_structure_id
                AND st_f.factory_id IN(0, su.factory_id)
            )
        AND tra.structure_id = su.location_facility_structure_id
    ) AS facility_name
    , su.job_structure_id AS job_id                               --保全活動件名の職種機種
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
                    st_f.structure_id = su.job_structure_id
                AND st_f.factory_id IN(0, su.factory_id)
            )
        AND tra.structure_id = su.job_structure_id
    ) AS job_name
    , re.request_no
    , su.subject
    , su.plan_implementation_content
    , su.subject_note
    , ma.machine_id
    , eq.equipment_id
    , ma.machine_no + ' ' + ma.machine_name AS machine
    , ma.location_structure_id AS machine_location_structure_id --機器の場所階層
    , ma.location_district_structure_id AS machine_district_id
    , ma.location_factory_structure_id AS machine_factory_id
    , ma.location_plant_structure_id AS machine_plant_id
    , ma.location_series_structure_id AS machine_series_id
    , ma.location_stroke_structure_id AS machine_stroke_id
    , ma.location_facility_structure_id AS machine_facility_id
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
                    st_f.structure_id = ma.location_district_structure_id
                AND st_f.factory_id IN(0, ma.location_factory_structure_id)
            )
        AND tra.structure_id = ma.location_district_structure_id
    ) AS machine_district_name
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
                    st_f.structure_id = ma.location_factory_structure_id
                AND st_f.factory_id IN(0, ma.location_factory_structure_id)
            )
        AND tra.structure_id = ma.location_factory_structure_id
    ) AS machine_factory_name
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
                    st_f.structure_id = ma.location_plant_structure_id
                AND st_f.factory_id IN(0, ma.location_factory_structure_id)
            )
        AND tra.structure_id = ma.location_plant_structure_id
    ) AS machine_plant_name
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
                    st_f.structure_id = ma.location_series_structure_id
                AND st_f.factory_id IN(0, ma.location_factory_structure_id)
            )
        AND tra.structure_id = ma.location_series_structure_id
    ) AS machine_series_name
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
                    st_f.structure_id = ma.location_stroke_structure_id
                AND st_f.factory_id IN(0, ma.location_factory_structure_id)
            )
        AND tra.structure_id = ma.location_stroke_structure_id
    ) AS machine_stroke_name
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
                    st_f.structure_id = ma.location_facility_structure_id
                AND st_f.factory_id IN(0, ma.location_factory_structure_id)
            )
        AND tra.structure_id = ma.location_facility_structure_id
    ) AS machine_facility_name
    , ma.job_structure_id AS machine_job_structure_id --機器の職種機種
    , ma.job_kind_structure_id AS machine_job_id
    , ma.job_large_classfication_structure_id AS machine_large_classfication_id
    , ma.job_middle_classfication_structure_id AS machine_middle_classfication_id
    , ma.job_small_classfication_structure_id AS machine_small_classfication_id
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
                   st_f.structure_id = ma.job_kind_structure_id
                   AND st_f.factory_id IN (0, ma.location_factory_structure_id)
           ) 
           AND tra.structure_id = ma.job_kind_structure_id
    ) AS machine_job_name
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
                   st_f.structure_id = ma.job_large_classfication_structure_id
                   AND st_f.factory_id IN (0, ma.location_factory_structure_id)
           ) 
           AND tra.structure_id = ma.job_large_classfication_structure_id
    ) AS machine_large_classfication_name
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
                   st_f.structure_id = ma.job_middle_classfication_structure_id
                   AND st_f.factory_id IN (0, ma.location_factory_structure_id)
           ) 
           AND tra.structure_id = ma.job_middle_classfication_structure_id
    ) AS machine_middle_classfication_name
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
                   st_f.structure_id = ma.job_small_classfication_structure_id
                   AND st_f.factory_id IN (0, ma.location_factory_structure_id)
           ) 
           AND tra.structure_id = ma.job_small_classfication_structure_id
    ) AS machine_small_classfication_name
    , ma.equipment_level_structure_id
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = ma.equipment_level_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = ma.equipment_level_structure_id
    ) AS equipment_level_name
    , ma.conservation_structure_id
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = ma.conservation_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = ma.conservation_structure_id
    ) AS conservation_name
    , ma.importance_structure_id
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = ma.importance_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = ma.importance_structure_id
    ) AS importance_name
    , hf.used_days_machine
    , hf.maintenance_site
    , hf.maintenance_content
    , hf.follow_flg
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = ff.structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = ff.structure_id
    ) AS follow_flg_name
    , hf.follow_plan_date
    , hf.follow_content
    , hf.follow_completion_date
    , hf.phenomenon_structure_id
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hf.phenomenon_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = hf.phenomenon_structure_id
    ) AS phenomenon_name
    , hf.phenomenon_note
    , hf.failure_cause_structure_id
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hf.failure_cause_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = hf.failure_cause_structure_id
    ) AS failure_cause_name
    , hf.failure_cause_note
    , hf.failure_cause_personality_structure_id
    , hf.failure_cause_personality_note
    , hf.treatment_measure_structure_id
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hf.treatment_measure_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = hf.treatment_measure_structure_id
    ) AS treatment_measure_name
    , hf.treatment_measure_note
    , CASE WHEN get_factory.extension_data = '1' THEN NULL ELSE hf.failure_status END AS failure_status --一般工場の場合の故障状況
    , CASE WHEN get_factory.extension_data = '1' THEN hf.failure_status ELSE NULL END AS failure_status_individual --個別工場の場合の故障状況
    , CASE WHEN get_factory.extension_data = '1' THEN NULL ELSE hf.failure_cause_addition_note END AS failure_cause_addition_note --一般工場の場合の故障原因補足
    , CASE WHEN get_factory.extension_data = '1' THEN hf.failure_cause_addition_note ELSE NULL END AS failure_cause_addition_note_individual --個別工場の場合の故障原因
    , CASE WHEN get_factory.extension_data = '1' THEN NULL ELSE hf.previous_situation END AS previous_situation --一般工場の場合の故障前の保全実施状況
    , CASE WHEN get_factory.extension_data = '1' THEN hf.previous_situation ELSE NULL END AS previous_situation_individual --個別工場の場合の故障前の保全実施状況
    , CASE WHEN get_factory.extension_data = '1' THEN NULL ELSE hf.recovery_action END AS recovery_action --一般工場の場合の復旧処置
    , CASE WHEN get_factory.extension_data = '1' THEN hf.recovery_action ELSE NULL END AS recovery_action_individual --個別工場の場合の復旧処置
    , CASE WHEN get_factory.extension_data = '1' THEN NULL ELSE hf.improvement_measure END AS improvement_measure --一般工場の場合の改善対策
    , CASE WHEN get_factory.extension_data = '1' THEN hf.improvement_measure ELSE NULL END AS improvement_measure_individual --個別工場の場合の改善対策
    , hf.system_feed_back
    , CASE WHEN get_factory.extension_data = '1' THEN NULL ELSE hf.lesson END AS lesson --一般工場の場合の教訓
    , CASE WHEN get_factory.extension_data = '1' THEN hf.lesson ELSE NULL END AS lesson_individual --個別工場の場合の教訓
    , CASE WHEN get_factory.extension_data = '1' THEN NULL ELSE hf.failure_note END AS failure_note --一般工場の場合の特記(メモ)
    , CASE WHEN get_factory.extension_data = '1' THEN hf.failure_note ELSE NULL END AS failure_note_individual --個別工場の場合の特記(メモ)
    , hf.failure_analysis_structure_id
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hf.failure_analysis_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = hf.failure_analysis_structure_id
    ) AS failure_analysis_name
    , hf.failure_personality_factor_structure_id
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hf.failure_personality_factor_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = hf.failure_personality_factor_structure_id
    ) AS failure_personality_factor_name
    , hf.failure_personality_class_structure_id
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hf.failure_personality_class_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = hf.failure_personality_class_structure_id
    ) AS failure_personality_class_name
    , hf.treatment_status_structure_id
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hf.treatment_status_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = hf.treatment_status_structure_id
    ) AS treatment_status_name
    , hf.necessity_measure_structure_id
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hf.necessity_measure_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = hf.necessity_measure_structure_id
    ) AS necessity_measure_name
    , hf.measure_plan_date
    , hf.measure_class1_structure_id
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hf.measure_class1_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = hf.measure_class1_structure_id
    ) AS measure_class1_name
    , hf.measure_class2_structure_id
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hf.measure_class2_structure_id 
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = hf.measure_class2_structure_id
    ) AS measure_class2_name 
    , hf.work_record
FROM
    ( 
        SELECT
            *
            , location_factory_structure_id AS factory_id 
        FROM
            ma_summary
    ) su 
    LEFT JOIN ma_request re 
        ON su.summary_id = re.summary_id 
    LEFT JOIN ma_plan pl 
        ON su.summary_id = pl.summary_id 
    LEFT JOIN ma_history hi 
        ON su.summary_id = hi.summary_id 
    LEFT JOIN ma_history_failure hf 
        ON hi.history_id = hf.history_id 
    LEFT JOIN mc_machine ma 
        ON hf.machine_id = ma.machine_id 
    LEFT JOIN mc_equipment eq 
        ON hf.equipment_id = eq.equipment_id 
    LEFT JOIN #follow_flg ff 
        ON CAST(hf.follow_flg AS nvarchar) = ff.extension_data 
    LEFT JOIN #get_factory get_factory
        ON get_factory.structure_id = su.factory_id
WHERE
    su.activity_division = 2                    --故障情報
    AND EXISTS ( 
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
    -- 発生日(From)
    /*@OccurrenceDateFrom
        AND pl.occurrence_date >= @OccurrenceDateFrom
    @OccurrenceDateFrom*/

    -- 発生日(To)
    /*@OccurrenceDateTo
        AND pl.occurrence_date <= @OccurrenceDateTo
    @OccurrenceDateTo*/

    -- 着工予定日(From)
    /*@ExpectedConstructionDateFrom
        AND pl.expected_construction_date >= @ExpectedConstructionDateFrom
    @ExpectedConstructionDateFrom*/

    -- 着工予定日(To)
    /*@ExpectedConstructionDateTo
        AND pl.expected_construction_date <= @ExpectedConstructionDateTo
    @ExpectedConstructionDateTo*/

    -- 完了日(From)
    /*@CompletionDateFrom
        AND su.completion_date >= @CompletionDateFrom
    @CompletionDateFrom*/

    -- 完了日(To)
    /*@CompletionDateTo
        AND su.completion_date <= @CompletionDateTo
    @CompletionDateTo*/

    -- 未完了
    /*@Incomplete
        AND su.completion_date IS NULL
    @Incomplete*/

    -- 完了
    /*@Completion
        AND su.completion_date IS NOT NULL
    @Completion*/