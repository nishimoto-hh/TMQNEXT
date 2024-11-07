WITH CirculationTargetTrue AS ( 
    -- 「対象」の翻訳を取得
    SELECT
        tra.translation_text 
    FROM
        ms_translation tra 
    WHERE
        tra.location_structure_id = 0 
        AND tra.translation_id = 111160071 
        AND tra.language_id = (SELECT TOP 1 languageId FROM #temp)
) 
, CirculationTargetFalse AS ( 
    -- 「非対象」の翻訳を取得
    SELECT
        tra.translation_text 
    FROM
        ms_translation tra 
    WHERE
        tra.location_structure_id = 0 
        AND tra.translation_id = 111270034 
        AND tra.language_id = (SELECT TOP 1 languageId FROM #temp)
) 
, TransExists AS ( 
    -- 「あり」の翻訳を取得
    SELECT
        tra.translation_text 
    FROM
        ms_translation tra 
    WHERE
        tra.location_structure_id = 0 
        AND tra.translation_id = 111010021 
        AND tra.language_id = (SELECT TOP 1 languageId FROM #temp)
) 
, TransNotExists AS ( 
    -- 「なし」の翻訳を取得
    SELECT
        tra.translation_text 
    FROM
        ms_translation tra 
    WHERE
        tra.location_structure_id = 0 
        AND tra.translation_id = 111210005 
        AND tra.language_id = (SELECT TOP 1 languageId FROM #temp)
)
-- 機器別保全保全履歴一覧 情報取得
SELECT
    * 
FROM
    ( 
        SELECT
            sm.job_structure_id
            , sm.location_structure_id
            , mc.job_structure_id AS job_structure_id2
            , mc.location_structure_id AS location_structure_id2
            --工場(翻訳)
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
                            st_f.structure_id = sm.location_factory_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = sm.location_factory_structure_id
            ) AS factory_name                   
            --プラント(翻訳)
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
                            st_f.structure_id = sm.location_plant_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = sm.location_plant_structure_id
            ) AS plant_name                     
            --系列(翻訳)
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
                            st_f.structure_id = sm.location_series_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = sm.location_series_structure_id
            ) AS series_name                    
            --工程(翻訳)
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
                            st_f.structure_id = sm.location_stroke_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = sm.location_stroke_structure_id
            ) AS stroke_name                    
            --設備(翻訳)
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
                            st_f.structure_id = sm.location_facility_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = sm.location_facility_structure_id
            ) AS facility_name                  
            --職種(翻訳)
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
                            st_f.structure_id = sm.job_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = sm.job_structure_id
            ) AS job_name
            , sm.subject                        -- 件名
            , '' AS phenomenon_name             -- 現象
            , CASE 
                WHEN ISNULL(call_count, 0) >= 1 
                    THEN TransExists.translation_text -- あり
                ELSE TransNotExists.translation_text -- なし
                END AS call_count_name
            , CASE 
                WHEN ISNULL(stop_count, 0) >= 1 
                    THEN TransExists.translation_text -- あり
                ELSE TransNotExists.translation_text -- なし
                END AS stop_count_name
            , stop_time
            , '' AS work_purpose_name           -- 目的区分
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
                            st_f.structure_id = sudden_division_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = sudden_division_structure_id
            ) AS sudden_division_name           -- 突発区分
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
                            st_f.structure_id = mq_class_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mq_class_structure_id
            ) AS mq_class_name                  -- MQ分類
            , occurrence_date
            , expected_construction_date
            , completion_date
            , total_budget_cost
            , expenditure
            , CAST(loss_absence AS INT) AS loss_absence
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
                            st_f.structure_id = hs.maintenance_season_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = hs.maintenance_season_structure_id
            ) AS maintenance_season_name        -- 時期
            , ( 
                SELECT
                    TOP 1 display_name 
                FROM
                    ms_user 
                WHERE
                    user_id = construction_personnel_id
            ) AS construction_personnel_name    -- 担当者
            , hs.total_working_time             -- 作業時間
            --FORMAT(hs.total_working_time, '0.##') AS total_working_time,  --作業時間(Hr)(表示用)
            , working_time_self                 
            --FORMAT(working_time_self, '0.##') AS working_time_self,  --自係(Hr)(表示用)
            , hs.construction_company
            , working_time_company
            , sm.plan_implementation_content    -- 作業内容結果
            , subject_note
            , maintenance_count
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
                            st_f.structure_id = discovery_methods_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = discovery_methods_structure_id
            ) AS discovery_methods_name         -- 発見方法
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
                            st_f.structure_id = actual_result_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = actual_result_structure_id
            ) AS actual_result_name             -- 実績結果
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
                            st_f.structure_id = sm.budget_management_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = sm.budget_management_structure_id
            ) AS budget_management_name         -- 予算管理区分
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
                            st_f.structure_id = budget_personality_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = budget_personality_structure_id
            ) AS budget_personality_name        -- 予算性格区分
            , machine_no
            , machine_name
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
                            st_f.structure_id = mc.equipment_level_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.equipment_level_structure_id
            ) AS equipment_level                -- 機器レベル
            , installation_location
            , number_of_installation
            , FORMAT(date_of_installation, 'yyyy/MM') AS date_of_installation
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
                            st_f.structure_id = importance_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = importance_structure_id
            ) AS importance_name                -- 重要度
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
                            st_f.structure_id = conservation_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = conservation_structure_id
            ) AS conservation_name              -- 保全方式
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
                            st_f.structure_id = app1.applicable_laws_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = app1.applicable_laws_structure_id
            ) AS applicable_laws_name1          -- 適用法規１
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
                            st_f.structure_id = app2.applicable_laws_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = app2.applicable_laws_structure_id
            ) AS applicable_laws_name2          -- 適用法規２
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
                            st_f.structure_id = app3.applicable_laws_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = app3.applicable_laws_structure_id
            ) AS applicable_laws_name3          -- 適用法規３
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
                            st_f.structure_id = app4.applicable_laws_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = app4.applicable_laws_structure_id
            ) AS applicable_laws_name4          -- 適用法規４
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
                            st_f.structure_id = app5.applicable_laws_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = app5.applicable_laws_structure_id
            ) AS applicable_laws_name5          -- 適用法規５
            , mc.machine_note                   -- 機番メモ
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
                            st_f.structure_id = use_segment_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = use_segment_structure_id
            ) AS use_segment_name               -- 使用区分
            , ( 
                CASE 
                    WHEN ISNULL(circulation_target_flg, 0) = 0 
                        THEN CirculationTargetFalse.translation_text 
                    ELSE CirculationTargetTrue.translation_text -- 対象
                    END
            ) AS circulation_target             -- 循環対象
            , fixed_asset_no                    -- 固定資産番号
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
                            st_f.structure_id = manufacturer_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = manufacturer_structure_id
            ) AS manufacturer_name              -- メーカー
            , manufacturer_type                 -- メーカー型式
            , model_no                          -- 型式コード
            , serial_no                         -- 製造番号
            , FORMAT(date_of_manufacture, 'yyyy/MM') AS date_of_manufacture -- 製造年月
            , equipment_note                    -- 機器メモ
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
                            st_f.structure_id = inspection_content_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = inspection_content_structure_id
            ) AS work_item_name                 -- 作業項目
            , '' AS treatment_measure_note      -- 作業内容・結果
            , '' AS failure_cause_addition_note -- 故障原因
            , '' AS failure_cause_personality_note -- 原因性格
            , '' AS treatment_measure_name      -- 処置対策
            , CASE ISNULL(hic.follow_flg, 0) 
                WHEN 1 THEN '○' 
                ELSE '' 
                END AS follow_flg               -- フォロー要否
            , FORMAT(follow_completion_date, 'yyyy/MM') AS follow_completion_date -- フォロー年月
            , follow_content                    -- フォロー内容
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
                            st_f.structure_id = his.inspection_site_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = his.inspection_site_structure_id
            ) AS maintenance_site               -- 作業部位
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
                            st_f.structure_id = hic.inspection_content_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = hic.inspection_content_structure_id
            ) AS inspection_content_name        -- 作業項目(保全項目)
            , '' AS maintenance_content         -- 作業項目(保全履歴内容)
            --工場(翻訳)
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
                            st_f.structure_id = mc.location_factory_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.location_factory_structure_id
            ) AS factory_name2                  
            --プラント(翻訳)
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
                            st_f.structure_id = mc.location_plant_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.location_plant_structure_id
            ) AS plant_name2                    
            --系列(翻訳)
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
                            st_f.structure_id = mc.location_series_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.location_series_structure_id
            ) AS series_name2                   
            --工程(翻訳)
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
                            st_f.structure_id = mc.location_stroke_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.location_stroke_structure_id
            ) AS stroke_name2                   
            --設備(翻訳)
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
                            st_f.structure_id = mc.location_facility_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.location_facility_structure_id
            ) AS facility_name2                 
            --職種(翻訳)
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
                            st_f.structure_id = mc.job_kind_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.job_kind_structure_id
            ) AS job_name2                      
            -- 機種大分類(翻訳)
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
                            st_f.structure_id = mc.job_large_classfication_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.job_large_classfication_structure_id
            ) AS large_classfication_name2      
            -- 機種中分類
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
                            st_f.structure_id = mc.job_middle_classfication_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.job_middle_classfication_structure_id
            ) AS middle_classfication_name2     
            -- 機種小分類
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
                            st_f.structure_id = mc.job_small_classfication_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.job_small_classfication_structure_id
            ) AS small_classfication_name2
            , '1' AS output_report_location_name_got_flg -- 機能場所名称情報取得済フラグ（帳票用）
            , '1' AS output_report_job_name_got_flg -- 職種・機種名称情報取得済フラグ（帳票用）
            , mc.machine_id 
        FROM
            ma_summary sm 
            LEFT JOIN ma_plan mp 
                ON mp.summary_id = sm.summary_id 
            LEFT JOIN ma_request req 
                ON req.summary_id = sm.summary_id 
            LEFT JOIN ma_history hs 
                ON sm.summary_id = hs.summary_id 
            LEFT JOIN ma_history_machine hsm 
                ON hs.history_id = hsm.history_id 
            INNER JOIN #temp temp 
                ON hsm.machine_id = temp.key1 
            LEFT JOIN ma_history_inspection_site his 
                ON hsm.history_machine_id = his.history_machine_id 
            LEFT JOIN ma_history_inspection_content hic 
                ON his.history_inspection_site_id = hic.history_inspection_site_id 
            LEFT JOIN mc_machine mc 
                ON hsm.machine_id = mc.machine_id 
            LEFT JOIN mc_equipment eq 
                ON mc.machine_id = eq.machine_id 
            LEFT JOIN ( 
                SELECT
                    app.machine_id
                    , app.applicable_laws_structure_id
                    , app.rnk 
                FROM
                    ( 
                        SELECT
                            machine_id
                            , applicable_laws_id
                            , applicable_laws_structure_id
                            , DENSE_RANK() OVER ( 
                                PARTITION BY
                                    machine_id 
                                ORDER BY
                                    applicable_laws_id
                            ) AS rnk 
                        FROM
                            mc_applicable_laws
                    ) app 
                WHERE
                    app.rnk = 1
            ) app1                              -- 適用法規１
                ON mc.machine_id = app1.machine_id 
            LEFT JOIN ( 
                SELECT
                    app.machine_id
                    , app.applicable_laws_structure_id
                    , app.rnk 
                FROM
                    ( 
                        SELECT
                            machine_id
                            , applicable_laws_id
                            , applicable_laws_structure_id
                            , DENSE_RANK() OVER ( 
                                PARTITION BY
                                    machine_id 
                                ORDER BY
                                    applicable_laws_id
                            ) AS rnk 
                        FROM
                            mc_applicable_laws
                    ) app 
                WHERE
                    app.rnk = 2
            ) app2                              -- 適用法規２
                ON mc.machine_id = app2.machine_id 
            LEFT JOIN ( 
                SELECT
                    app.machine_id
                    , app.applicable_laws_structure_id
                    , app.rnk 
                FROM
                    ( 
                        SELECT
                            machine_id
                            , applicable_laws_id
                            , applicable_laws_structure_id
                            , DENSE_RANK() OVER ( 
                                PARTITION BY
                                    machine_id 
                                ORDER BY
                                    applicable_laws_id
                            ) AS rnk 
                        FROM
                            mc_applicable_laws
                    ) app 
                WHERE
                    app.rnk = 3
            ) app3                              -- 適用法規３
                ON mc.machine_id = app3.machine_id 
            LEFT JOIN ( 
                SELECT
                    app.machine_id
                    , app.applicable_laws_structure_id
                    , app.rnk 
                FROM
                    ( 
                        SELECT
                            machine_id
                            , applicable_laws_id
                            , applicable_laws_structure_id
                            , DENSE_RANK() OVER ( 
                                PARTITION BY
                                    machine_id 
                                ORDER BY
                                    applicable_laws_id
                            ) AS rnk 
                        FROM
                            mc_applicable_laws
                    ) app 
                WHERE
                    app.rnk = 4
            ) app4                              -- 適用法規４
                ON mc.machine_id = app4.machine_id 
            LEFT JOIN ( 
                SELECT
                    app.machine_id
                    , app.applicable_laws_structure_id
                    , app.rnk 
                FROM
                    ( 
                        SELECT
                            machine_id
                            , applicable_laws_id
                            , applicable_laws_structure_id
                            , DENSE_RANK() OVER ( 
                                PARTITION BY
                                    machine_id 
                                ORDER BY
                                    applicable_laws_id
                            ) AS rnk 
                        FROM
                            mc_applicable_laws
                    ) app 
                WHERE
                    app.rnk = 5
            ) app5                              -- 適用法規５
                ON mc.machine_id = app5.machine_id 
            CROSS JOIN CirculationTargetTrue    --「対象」の翻訳
            CROSS JOIN CirculationTargetFalse   -- 「非対象」の翻訳
            CROSS JOIN TransExists              --「あり」の翻訳
            CROSS JOIN TransNotExists           -- 「なし」の翻訳
        WHERE
            sm.activity_division = 1            -- 点検
            
            UNION ALL 
            
        SELECT
            sm.job_structure_id
            , sm.location_structure_id
            , mc.job_structure_id AS job_structure_id2
            , mc.location_structure_id AS location_structure_id2 
            --工場(翻訳)
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
                            st_f.structure_id = sm.location_factory_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = sm.location_factory_structure_id
            ) AS factory_name                  
            --プラント(翻訳)
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
                            st_f.structure_id = sm.location_plant_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = sm.location_plant_structure_id
            ) AS plant_name                     
            --系列(翻訳)
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
                            st_f.structure_id = sm.location_series_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = sm.location_series_structure_id
            ) AS series_name                    
            --工程(翻訳)
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
                            st_f.structure_id = sm.location_stroke_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = sm.location_stroke_structure_id
            ) AS stroke_name                    
            --設備(翻訳)
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
                            st_f.structure_id = sm.location_facility_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = sm.location_facility_structure_id
            ) AS facility_name                  
            --職種(翻訳)
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
                            st_f.structure_id = sm.job_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = sm.job_structure_id
            ) AS job_name
            , sm.subject                        -- 件名
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
                            st_f.structure_id = phenomenon_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = phenomenon_structure_id
            ) AS phenomenon_name                -- 現象
            , CASE 
                WHEN ISNULL(call_count, 0) >= 1 
                    THEN TransExists.translation_text -- あり
                ELSE TransNotExists.translation_text -- なし
                END AS call_count_name
            , CASE 
                WHEN ISNULL(stop_count, 0) >= 1 
                    THEN TransExists.translation_text -- あり
                ELSE TransNotExists.translation_text -- なし
                END AS stop_count_name
            , stop_time
            , '' AS work_purpose_name           -- 目的区分
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
                            st_f.structure_id = sudden_division_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = sudden_division_structure_id
            ) AS sudden_division_name           -- 突発区分
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
                            st_f.structure_id = mq_class_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mq_class_structure_id
            ) AS mq_class_name                  -- MQ分類
            , occurrence_date
            , expected_construction_date
            , completion_date
            , total_budget_cost
            , expenditure
            , CAST(loss_absence AS INT) AS loss_absence
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
                            st_f.structure_id = hs.maintenance_season_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = hs.maintenance_season_structure_id
            ) AS maintenance_season_name        -- 時期
            , ( 
                SELECT
                    TOP 1 display_name 
                FROM
                    ms_user 
                WHERE
                    user_id = construction_personnel_id
            ) AS construction_personnel_name    -- 担当者
            , hs.total_working_time             -- 作業時間
            --FORMAT(hs.total_working_time, '0.##') AS total_working_time,  --作業時間(Hr)(表示用)
            , working_time_self                 
            --FORMAT(working_time_self, '0.##') AS working_time_self,  --自係(Hr)(表示用)
            , hs.construction_company
            , working_time_company
            , sm.plan_implementation_content    -- 作業内容結果
            , subject_note
            , maintenance_count
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
                            st_f.structure_id = discovery_methods_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = discovery_methods_structure_id
            ) AS discovery_methods_name         -- 発見方法
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
                            st_f.structure_id = actual_result_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = actual_result_structure_id
            ) AS actual_result_name             -- 実績結果
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
                            st_f.structure_id = sm.budget_management_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = sm.budget_management_structure_id
            ) AS budget_management_name         -- 予算管理区分
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
                            st_f.structure_id = budget_personality_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = budget_personality_structure_id
            ) AS budget_personality_name        -- 予算性格区分
            , machine_no
            , machine_name
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
                            st_f.structure_id = mc.equipment_level_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.equipment_level_structure_id
            ) AS equipment_level                -- 機器レベル
            , installation_location
            , number_of_installation
            , FORMAT(date_of_installation, 'yyyy/MM') AS date_of_installation
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
                            st_f.structure_id = importance_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = importance_structure_id
            ) AS importance_name                -- 重要度
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
                            st_f.structure_id = conservation_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = conservation_structure_id
            ) AS conservation_name              -- 保全方式
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
                            st_f.structure_id = app1.applicable_laws_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = app1.applicable_laws_structure_id
            ) AS applicable_laws_name1          -- 適用法規１
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
                            st_f.structure_id = app2.applicable_laws_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = app2.applicable_laws_structure_id
            ) AS applicable_laws_name2          -- 適用法規２
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
                            st_f.structure_id = app3.applicable_laws_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = app3.applicable_laws_structure_id
            ) AS applicable_laws_name3          -- 適用法規３
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
                            st_f.structure_id = app4.applicable_laws_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = app4.applicable_laws_structure_id
            ) AS applicable_laws_name4          -- 適用法規４
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
                            st_f.structure_id = app5.applicable_laws_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = app5.applicable_laws_structure_id
            ) AS applicable_laws_name5          -- 適用法規５
            , mc.machine_note                   -- 機番メモ
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
                            st_f.structure_id = use_segment_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = use_segment_structure_id
            ) AS use_segment_name               -- 使用区分
            , ( 
                CASE 
                    WHEN ISNULL(circulation_target_flg, 0) = 0 
                        THEN CirculationTargetFalse.translation_text 
                    ELSE CirculationTargetTrue.translation_text -- 対象
                    END
            ) AS circulation_target             -- 循環対象
            , fixed_asset_no                    -- 固定資産番号
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
                            st_f.structure_id = manufacturer_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = manufacturer_structure_id
            ) AS manufacturer_name              -- メーカー
            , manufacturer_type                 -- メーカー型式
            , model_no                          -- 型式コード
            , serial_no                         -- 製造番号
            , FORMAT(date_of_manufacture, 'yyyy/MM') AS date_of_manufacture -- 製造年月
            , equipment_note                    -- 機器メモ
            , hf.maintenance_site AS work_item_name -- 作業項目
            , treatment_measure_note            -- 作業内容・結果
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
                            st_f.structure_id = hf.failure_cause_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = hf.failure_cause_structure_id
            ) AS failure_cause_addition_note    -- 故障原因
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
                            st_f.structure_id = hf.failure_cause_personality_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = hf.failure_cause_personality_structure_id
            ) AS failure_cause_personality_note -- 原因性格
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
                            st_f.structure_id = hf.treatment_measure_structure_id 
                            AND st_f.factory_id IN (0, sm.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = hf.treatment_measure_structure_id
            ) AS treatment_measure_name         -- 処置・対策
            , CASE ISNULL(hf.follow_flg, 0) 
                WHEN 1 THEN '○' 
                ELSE '' 
                END AS follow_flg               -- フォロー要否
            , FORMAT(hf.follow_completion_date, 'yyyy/MM') AS follow_completion_date -- フォロー年月
            , hf.follow_content                 -- フォロー内容
            , hf.maintenance_site
            , hf.maintenance_content AS inspection_content_name -- 作業項目(保全項目)
            , hf.maintenance_content            -- 作業項目(保全履歴内容)
            --工場(翻訳)
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
                            st_f.structure_id = mc.location_factory_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.location_factory_structure_id
            ) AS factory_name2                  
            --プラント(翻訳)
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
                            st_f.structure_id = mc.location_plant_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.location_plant_structure_id
            ) AS plant_name2                    
            --系列(翻訳)
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
                            st_f.structure_id = mc.location_series_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.location_series_structure_id
            ) AS series_name2                   
            --工程(翻訳)
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
                            st_f.structure_id = mc.location_stroke_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.location_stroke_structure_id
            ) AS stroke_name2                   
            --設備(翻訳)
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
                            st_f.structure_id = mc.location_facility_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.location_facility_structure_id
            ) AS facility_name2                 
            --職種(翻訳)
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
                            st_f.structure_id = mc.job_kind_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.job_kind_structure_id
            ) AS job_name2                      
            -- 機種大分類(翻訳)
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
                            st_f.structure_id = mc.job_large_classfication_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.job_large_classfication_structure_id
            ) AS large_classfication_name2      
            -- 機種中分類
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
                            st_f.structure_id = mc.job_middle_classfication_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.job_middle_classfication_structure_id
            ) AS middle_classfication_name2     
            -- 機種小分類
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
                            st_f.structure_id = mc.job_small_classfication_structure_id 
                            AND st_f.factory_id IN (0, mc.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mc.job_small_classfication_structure_id
            ) AS small_classfication_name2
            , '1' AS output_report_location_name_got_flg -- 機能場所名称情報取得済フラグ（帳票用）
            , '1' AS output_report_job_name_got_flg -- 職種・機種名称情報取得済フラグ（帳票用）
            , mc.machine_id 
        FROM
            ma_summary sm 
            LEFT JOIN ma_plan mp 
                ON mp.summary_id = sm.summary_id 
            LEFT JOIN ma_request req 
                ON req.summary_id = sm.summary_id 
            LEFT JOIN ma_history hs 
                ON sm.summary_id = hs.summary_id 
            LEFT JOIN ma_history_failure hf 
                ON hs.history_id = hf.history_id 
            INNER JOIN #temp temp 
                ON hf.machine_id = temp.key1 
            LEFT JOIN mc_machine mc 
                ON hf.machine_id = mc.machine_id 
            LEFT JOIN mc_equipment eq 
                ON mc.machine_id = eq.machine_id 
            LEFT JOIN ( 
                SELECT
                    app.machine_id
                    , app.applicable_laws_structure_id
                    , app.rnk 
                FROM
                    ( 
                        SELECT
                            machine_id
                            , applicable_laws_id
                            , applicable_laws_structure_id
                            , DENSE_RANK() OVER ( 
                                PARTITION BY
                                    machine_id 
                                ORDER BY
                                    applicable_laws_id
                            ) AS rnk 
                        FROM
                            mc_applicable_laws
                    ) app 
                WHERE
                    app.rnk = 1
            ) app1                              -- 適用法規１
                ON mc.machine_id = app1.machine_id 
            LEFT JOIN ( 
                SELECT
                    app.machine_id
                    , app.applicable_laws_structure_id
                    , app.rnk 
                FROM
                    ( 
                        SELECT
                            machine_id
                            , applicable_laws_id
                            , applicable_laws_structure_id
                            , DENSE_RANK() OVER ( 
                                PARTITION BY
                                    machine_id 
                                ORDER BY
                                    applicable_laws_id
                            ) AS rnk 
                        FROM
                            mc_applicable_laws
                    ) app 
                WHERE
                    app.rnk = 2
            ) app2                              -- 適用法規２
                ON mc.machine_id = app2.machine_id 
            LEFT JOIN ( 
                SELECT
                    app.machine_id
                    , app.applicable_laws_structure_id
                    , app.rnk 
                FROM
                    ( 
                        SELECT
                            machine_id
                            , applicable_laws_id
                            , applicable_laws_structure_id
                            , DENSE_RANK() OVER ( 
                                PARTITION BY
                                    machine_id 
                                ORDER BY
                                    applicable_laws_id
                            ) AS rnk 
                        FROM
                            mc_applicable_laws
                    ) app 
                WHERE
                    app.rnk = 3
            ) app3                              -- 適用法規３
                ON mc.machine_id = app3.machine_id 
            LEFT JOIN ( 
                SELECT
                    app.machine_id
                    , app.applicable_laws_structure_id
                    , app.rnk 
                FROM
                    ( 
                        SELECT
                            machine_id
                            , applicable_laws_id
                            , applicable_laws_structure_id
                            , DENSE_RANK() OVER ( 
                                PARTITION BY
                                    machine_id 
                                ORDER BY
                                    applicable_laws_id
                            ) AS rnk 
                        FROM
                            mc_applicable_laws
                    ) app 
                WHERE
                    app.rnk = 4
            ) app4                              -- 適用法規４
                ON mc.machine_id = app4.machine_id 
            LEFT JOIN ( 
                SELECT
                    app.machine_id
                    , app.applicable_laws_structure_id
                    , app.rnk 
                FROM
                    ( 
                        SELECT
                            machine_id
                            , applicable_laws_id
                            , applicable_laws_structure_id
                            , DENSE_RANK() OVER ( 
                                PARTITION BY
                                    machine_id 
                                ORDER BY
                                    applicable_laws_id
                            ) AS rnk 
                        FROM
                            mc_applicable_laws
                    ) app 
                WHERE
                    app.rnk = 5
            ) app5                              -- 適用法規５
                ON mc.machine_id = app5.machine_id 
            CROSS JOIN CirculationTargetTrue    --「対象」の翻訳
            CROSS JOIN CirculationTargetFalse   -- 「非対象」の翻訳
            CROSS JOIN TransExists              --「あり」の翻訳
            CROSS JOIN TransNotExists           -- 「なし」の翻訳
        WHERE
            sm.activity_division = 2            -- 故障
    ) tbl 
ORDER BY
    machine_no ASC
    , machine_id ASC
    , ISNULL(completion_date, '9999/12/31') DESC;
