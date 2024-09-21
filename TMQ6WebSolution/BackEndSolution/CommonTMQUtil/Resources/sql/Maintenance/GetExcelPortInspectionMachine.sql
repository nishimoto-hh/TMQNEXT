SELECT DISTINCT
    hm.history_machine_id
    , his.history_inspection_site_id
    , hic.history_inspection_content_id
    , su.summary_id
    , su.summary_id AS old_summary_id
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
    , ma.machine_id AS machine_id_before
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
    , his.inspection_site_structure_id
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
                    st_f.structure_id = his.inspection_site_structure_id
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = his.inspection_site_structure_id
    ) AS inspection_site_name
    , hic.inspection_content_structure_id
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
                    st_f.structure_id = hic.inspection_content_structure_id
                    AND st_f.factory_id IN (0, su.factory_id)
            ) 
            AND tra.structure_id = hic.inspection_content_structure_id
    ) AS inspection_content_name
    , hic.follow_flg
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
    , hic.follow_plan_date
    , hic.follow_content
    , hic.follow_completion_date
    , hm.used_days_machine
    , hic.work_record
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
    INNER JOIN ma_history_machine hm 
        ON hi.history_id = hm.history_id 
    LEFT JOIN ma_history_inspection_site his 
        ON hm.history_machine_id = his.history_machine_id 
    LEFT JOIN ma_history_inspection_content hic 
        ON his.history_inspection_site_id = hic.history_inspection_site_id 
    LEFT JOIN mc_machine ma 
        ON hm.machine_id = ma.machine_id 
    LEFT JOIN mc_equipment eq 
        ON hm.equipment_id = eq.equipment_id 
    LEFT JOIN #follow_flg ff 
        ON CAST(hic.follow_flg AS nvarchar) = ff.extension_data 
WHERE
    su.activity_division = 1                    --点検情報
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

    -- 発行日(From)
    /*@IssueDateFrom
        AND re.issue_date >= @IssueDateFrom
    @IssueDateFrom*/

    -- 発行日(To)
    /*@IssueDateTo
        AND re.issue_date <= @IssueDateTo
    @IssueDateTo*/