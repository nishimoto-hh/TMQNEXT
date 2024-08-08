-- 機器一覧の検索結果を取得する
SELECT
    target.machine_id                                    -- 機番ID
    , target.machine_name                                -- 機器名称
    , target.machine_no                                  -- 機器番号
    , target.location_factory_structure_id AS factory_id -- 工場ID
    , target.maintainance_kind_manage                    -- 点検種別毎管理
    ---------------------------------- 以下は翻訳を取得 ----------------------------------
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.location_district_structure_id 
                    AND st_f.factory_id IN (0, target.location_factory_structure_id)
            ) 
            AND tra.structure_id = target.location_district_structure_id
    ) AS district_name -- 地区
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.location_factory_structure_id 
                    AND st_f.factory_id IN (0, target.location_factory_structure_id)
            ) 
            AND tra.structure_id = target.location_factory_structure_id
    ) AS factory_name -- 工場
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.location_plant_structure_id 
                    AND st_f.factory_id IN (0, target.location_factory_structure_id)
            ) 
            AND tra.structure_id = target.location_plant_structure_id
    ) AS plant_name -- プラント
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.location_series_structure_id 
                    AND st_f.factory_id IN (0, target.location_factory_structure_id)
            ) 
            AND tra.structure_id = target.location_series_structure_id
    ) AS series_name -- 系列
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.location_stroke_structure_id 
                    AND st_f.factory_id IN (0, target.location_factory_structure_id)
            ) 
            AND tra.structure_id = target.location_stroke_structure_id
    ) AS stroke_name -- 工程
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.location_facility_structure_id 
                    AND st_f.factory_id IN (0, target.location_factory_structure_id)
            ) 
            AND tra.structure_id = target.location_facility_structure_id
    ) AS facility_name -- 設備
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.job_kind_structure_id 
                    AND st_f.factory_id IN (0, target.location_factory_structure_id)
            ) 
            AND tra.structure_id = target.job_kind_structure_id
    ) AS job_name -- 職種
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.job_large_classfication_structure_id 
                    AND st_f.factory_id IN (0, target.location_factory_structure_id)
            ) 
            AND tra.structure_id = target.job_large_classfication_structure_id
    ) AS large_classfication_name -- 機種大分類
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.job_middle_classfication_structure_id 
                    AND st_f.factory_id IN (0, target.location_factory_structure_id)
            ) 
            AND tra.structure_id = target.job_middle_classfication_structure_id
    ) AS middle_classfication_name -- 機種中分類
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.job_small_classfication_structure_id 
                    AND st_f.factory_id IN (0, target.location_factory_structure_id)
            ) 
            AND tra.structure_id = target.job_small_classfication_structure_id
    ) AS small_classfication_name -- 機種小分類
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.importance_structure_id 
                    AND st_f.factory_id IN (0, target.location_factory_structure_id)
            ) 
            AND tra.structure_id = target.importance_structure_id
    ) AS importance_name -- 重要度
    , is_management_standards_exists -- 機器別管理基準有無
FROM
    target