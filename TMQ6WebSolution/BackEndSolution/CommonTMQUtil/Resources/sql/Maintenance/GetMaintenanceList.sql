SELECT
    *
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                -- 工場IDまたは0で合致する最大の工場IDを取得→工場IDのレコードがなければ0となる
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.mq_class_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.mq_class_structure_id
    ) AS mq_class_name  --MQ分類(翻訳)
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
                    st_f.structure_id = target.stop_system_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.stop_system_structure_id
    ) AS stop_system_name  --系停止(翻訳)
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
                    st_f.structure_id = target.sudden_division_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.sudden_division_structure_id
    ) AS sudden_division_name  --突発区分(翻訳)
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
                    st_f.structure_id = target.budget_management_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.budget_management_structure_id
    ) AS budget_management_name  --予算管理区分(翻訳)
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
                    st_f.structure_id = target.budget_personality_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.budget_personality_structure_id
    ) AS budget_personality_name  --予算性格区分(翻訳)
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
                    st_f.structure_id = target.maintenance_season_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.maintenance_season_structure_id
    ) AS maintenance_season_name  --保全時期(翻訳)
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
                    st_f.structure_id = target.discovery_methods_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.discovery_methods_structure_id
    ) AS discovery_methods_name  --発見方法(翻訳)
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
                    st_f.structure_id = target.actual_result_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.actual_result_structure_id
    ) AS actual_result_name  --実績結果(翻訳)
    , COALESCE( 
        target.maintenance_site
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
                        st_f.structure_id = target.inspection_site_structure_id 
                        AND st_f.factory_id IN (0, target.factoryId)
                ) 
                AND tra.structure_id = target.inspection_site_structure_id
        )
    ) AS maintenance_site_name  --保全部位(翻訳)
    , COALESCE( 
        target.maintenance_content
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
                        st_f.structure_id = target.inspection_content_structure_id 
                        AND st_f.factory_id IN (0, target.factoryId)
                ) 
                AND tra.structure_id = target.inspection_content_structure_id
        )
    ) AS maintenance_content_name  --保全内容(翻訳)
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
                    st_f.structure_id = target.phenomenon_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.phenomenon_structure_id
    ) AS phenomenon_name  --現象(翻訳)
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
                    st_f.structure_id = target.failure_cause_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.failure_cause_structure_id
    ) AS failure_cause_name  --原因(翻訳)
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
                    st_f.structure_id = target.treatment_measure_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.treatment_measure_structure_id
    ) AS treatment_measure_name  --処置対策(翻訳)
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
                    st_f.structure_id = target.progress_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.progress_id
    ) AS progress_name  --進捗状況(翻訳)
FROM
    translate_target AS target 
