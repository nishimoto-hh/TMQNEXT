WITH sudden_code AS ( 
    --突発計画区分コード
    SELECT
        structure_id
        , extension_data 
    FROM
        ms_structure ms 
        LEFT JOIN ms_item_extension ex 
            ON ms.structure_item_id = ex.item_id 
    WHERE
        ms.structure_group_id = 1400 
        AND ex.sequence_no = 1
) 
, header_info AS ( 
    -- 概要情報（カンマ区切り）
    SELECT
        TOP 1 CAST(CURRENT_TIMESTAMP AS DATE) AS output_date
        , trim( 
            ',' 
            FROM
                ( 
                    SELECT DISTINCT
                        ( 
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
                                        st_f.structure_id = su.job_structure_id 
                                        AND st_f.factory_id IN (0, su.location_factory_structure_id)
                                ) 
                                AND tra.structure_id = su.job_structure_id
                        ) + ',' 
                    FROM
                        ma_summary su 
                        INNER JOIN #temp temp 
                            ON su.summary_id = temp.Key1 FOR XML PATH ('')
                )
        ) AS job_name
        , trim( 
            ',' 
            FROM
                ( 
                    SELECT DISTINCT
                        ( 
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
                                        st_f.structure_id = su.location_factory_structure_id 
                                        AND st_f.factory_id IN (0, su.location_factory_structure_id)
                                ) 
                                AND tra.structure_id = su.location_factory_structure_id
                        ) + ',' 
                    FROM
                        ma_summary su 
                        INNER JOIN #temp temp 
                            ON su.summary_id = temp.Key1 FOR XML PATH ('')
                )
        ) AS factory_name
        , trim( 
            ',' 
            FROM
                ( 
                    SELECT DISTINCT
                        hi.construction_personnel_name + ',' 
                    FROM
                        ma_history hi 
                        INNER JOIN #temp temp 
                            ON hi.summary_id = temp.Key1 FOR XML PATH ('')
                )
        ) AS personnel_name
) 
SELECT
    1 AS data_type                              -- 計画作業
    , ( 
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
                    st_f.structure_id = su.location_stroke_structure_id 
                    AND st_f.factory_id IN (0, su.location_factory_structure_id)
            ) 
            AND tra.structure_id = su.location_stroke_structure_id
    ) AS stroke_name
    , ( 
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
                    st_f.structure_id = su.location_series_structure_id 
                    AND st_f.factory_id IN (0, su.location_factory_structure_id)
            ) 
            AND tra.structure_id = su.location_series_structure_id
    ) AS series_name
    , CASE 
        WHEN su.activity_division = 1 
            THEN mchm.machine_name 
        ELSE mchf.machine_name 
        END AS machine_name
    , su.subject
    , CASE 
        WHEN su.activity_division = 1 
            THEN ( 
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
                        st_f.structure_id = hic.inspection_content_structure_id 
                        AND st_f.factory_id IN (0, su.location_factory_structure_id)
                ) 
                AND tra.structure_id = hic.inspection_content_structure_id
        ) 
        ELSE hf.maintenance_content 
        END AS content
    , CASE 
        WHEN su.activity_division = 1 
            THEN ( 
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
                        st_f.structure_id = his.inspection_site_structure_id 
                        AND st_f.factory_id IN (0, su.location_factory_structure_id)
                ) 
                AND tra.structure_id = his.inspection_site_structure_id
        ) 
        ELSE hf.maintenance_site 
        END AS site
    , su.plan_implementation_content
    , pl.total_budget_cost
    , pl.plan_man_hour
    , null stop_time
    , null loss_absence_type_count
    , header_info.output_date
    , header_info.job_name
    , header_info.factory_name
    , header_info.personnel_name 
FROM
    ma_summary su 
    INNER JOIN #temp temp 
        ON su.summary_id = temp.Key1 
    LEFT JOIN ma_plan pl 
        ON su.summary_id = pl.summary_id 
    LEFT JOIN ma_history hi 
        ON su.summary_id = hi.summary_id 
    LEFT JOIN ma_history_failure hf 
        ON hi.history_id = hf.history_id 
    LEFT JOIN mc_machine mchf 
        ON hf.machine_id = mchf.machine_id 
    LEFT JOIN ma_history_machine hm 
        ON hi.history_id = hm.history_id 
    LEFT JOIN mc_machine mchm 
        ON hm.machine_id = mchm.machine_id 
    LEFT JOIN ma_history_inspection_site his 
        ON hm.history_machine_id = his.history_machine_id 
    LEFT JOIN ma_history_inspection_content hic 
        ON his.history_inspection_site_id = hic.history_inspection_site_id 
    LEFT JOIN sudden_code 
        ON su.sudden_division_structure_id = sudden_code.structure_id 
    CROSS JOIN header_info 
WHERE
    sudden_code.extension_data = '10'           -- 計画
    UNION ALL 
SELECT
    2 AS data_type                              -- 非計画作業
    , ( 
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
                    st_f.structure_id = su.location_stroke_structure_id 
                    AND st_f.factory_id IN (0, su.location_factory_structure_id)
            ) 
            AND tra.structure_id = su.location_stroke_structure_id
    ) AS stroke_name
    , ( 
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
                    st_f.structure_id = su.location_series_structure_id 
                    AND st_f.factory_id IN (0, su.location_factory_structure_id)
            ) 
            AND tra.structure_id = su.location_series_structure_id
    ) AS series_name
    , CASE 
        WHEN su.activity_division = 1 
            THEN mchm.machine_name 
        ELSE mchf.machine_name 
        END AS machine_name
    , su.subject
    , CASE 
        WHEN su.activity_division = 1 
            THEN ( 
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
                        st_f.structure_id = hic.inspection_content_structure_id 
                        AND st_f.factory_id IN (0, su.location_factory_structure_id)
                ) 
                AND tra.structure_id = hic.inspection_content_structure_id
        ) 
        ELSE hf.maintenance_content 
        END AS content
    , CASE 
        WHEN su.activity_division = 1 
            THEN ( 
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
                        st_f.structure_id = his.inspection_site_structure_id 
                        AND st_f.factory_id IN (0, su.location_factory_structure_id)
                ) 
                AND tra.structure_id = his.inspection_site_structure_id
        ) 
        ELSE hf.maintenance_site 
        END AS site
    , su.plan_implementation_content
    , pl.total_budget_cost
    , pl.plan_man_hour
    , su.stop_time
    , hi.loss_absence_type_count
    , header_info.output_date
    , header_info.job_name
    , header_info.factory_name
    , header_info.personnel_name 
FROM
    ma_summary su 
    INNER JOIN #temp temp 
        ON su.summary_id = temp.Key1 
    LEFT JOIN ma_plan pl 
        ON su.summary_id = pl.summary_id 
    LEFT JOIN ma_history hi 
        ON su.summary_id = hi.summary_id 
    LEFT JOIN ma_history_failure hf 
        ON hi.history_id = hf.history_id 
    LEFT JOIN mc_machine mchf 
        ON hf.machine_id = mchf.machine_id 
    LEFT JOIN ma_history_machine hm 
        ON hi.history_id = hm.history_id 
    LEFT JOIN mc_machine mchm 
        ON hm.machine_id = mchm.machine_id 
    LEFT JOIN ma_history_inspection_site his 
        ON hm.history_machine_id = his.history_machine_id 
    LEFT JOIN ma_history_inspection_content hic 
        ON his.history_inspection_site_id = hic.history_inspection_site_id 
    LEFT JOIN sudden_code 
        ON su.sudden_division_structure_id = sudden_code.structure_id 
    CROSS JOIN header_info 
WHERE
    sudden_code.extension_data IN ('20', '30')  --計画外、突発、
    OR su.sudden_division_structure_id IS NULL  --突発区分が未設定