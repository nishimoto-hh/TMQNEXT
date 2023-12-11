with call_count_name as ( -- 系停止 の翻訳を取得
    -- 「あり」の翻訳を取得
    select
        1 as division
        , tr.translation_text as name 
    from
        ms_translation tr 
    where
        tr.translation_id = 111010021 
        and tr.language_id = @LanguageId
        and tr.location_structure_id = 0  
        
    -- 「なし」の翻訳を取得
    union all 
    select
        2 as division
        , tr.translation_text as name 
    from
        ms_translation tr 
    where
        tr.translation_id = 111210005 
        and tr.language_id = @LanguageId
        and tr.location_structure_id = 0
)
, circulation as ( -- 循環対象 の翻訳を取得
    -- 「対象」の翻訳を取得
    select
        1 as division
        , tr.translation_text as name 
    from
        ms_translation tr 
    where
        tr.translation_id = 111160071 
        and tr.language_id = @LanguageId
        and tr.location_structure_id = 0  
        
    -- 「非対象」の翻訳を取得
    union all 
    select
        2 as division
        , tr.translation_text as name 
    from
        ms_translation tr 
    where
        tr.translation_id = 111270034 
        and tr.language_id = @LanguageId
        and tr.location_structure_id = 0
)
, follow as ( -- フォロー有無
    -- 「○」の翻訳を取得
    select
        tr.translation_text as name 
    from
        ms_translation tr 
    where
        tr.translation_id = 111310013 
        and tr.language_id = @LanguageId
        and tr.location_structure_id = 0 
)
select
    su.subject                                  -- 件名
    , su.stop_time                              -- 系停止時間
    , pl.occurrence_date                        -- 発生日
    , pl.expected_construction_date             -- 着工予定日
    , su.completion_date                        -- 完了年月日
    , pl.total_budget_cost                      -- 予算費用
    , hi.expenditure                            -- 実績費用
    , hi.cost_note                              -- 費用メモ
    , hi.construction_personnel_name            -- 担当者1
    , hi.total_working_time                     -- 作業時間
    , hi.working_time_self                      -- 自社
    , hi.construction_company                   -- 施工会社
    , hi.working_time_company                   -- 施工会社工数
    , su.plan_implementation_content            -- 作業計画内容
    , su.subject_note                           -- メモ
    , su.maintenance_count                      -- カウント件数
    , su.summary_id                             -- 保全管理NO
    , machine.machine_no                        -- 機器番号
    , machine.machine_name                      -- 機器名称
    , machine.installation_location             -- 設置場所
    , machine.number_of_installation            -- 設置台数
    , machine.date_of_installation              -- 設置年月
    , machine.machine_note                      -- 機番メモ
    , equipment.fixed_asset_no                  -- 固定資産番号
    , equipment.manufacturer_type               -- メーカー型式
    , equipment.model_no                        -- 型式コード
    , equipment.serial_no                       -- 製造番号
    , equipment.date_of_manufacture             -- 製造年月
    , equipment.equipment_note                  -- 機器メモ
    , '' as work_division                       -- 作業目的(空のデータを出力)
    , '' as construction_personnel_name2        -- 担当者2(空のデータを出力)
    , '' as working_item                        -- 作業項目(空のデータを出力)
    , '' as working_content_result              -- 作業内容・結果(空のデータを出力)
    , '' as machine_change                      -- 機器交換(空のデータを出力)
    , '' as deterioration_inspection            -- 劣化検査(空のデータを出力)
    , '' as machine_working_time                -- 作業時間(機器) (空のデータを出力)
    , '' as company_time                        -- 自社時間(機器) (空のデータを出力)
    , '' as machine_construction_company        -- 施工会社(機器) (空のデータを出力)
    , '' as position_large_name                 -- 部位大分類(空のデータを出力)
    , '' as position_middle_name                -- 部位中分類(空のデータを出力)
    , '' as position_small_name                 -- 部位小分類(空のデータを出力)
    , '' as machine_expenditure                 -- 実績金額(空のデータを出力)
    , '' as materialc_cost                      -- 材料費(機器) (空のデータを出力)
    , '' as labor_costs                         -- 労務費(機器) (空のデータを出力)
    , '' as incidentals                         -- 雑費(機器) (空のデータを出力)
    , '' as common_cost                         -- 共通費(機器) (空のデータを出力)
    , '' as history_content                     -- 保全履歴内容(空のデータを出力)
    ---------- 以下は翻訳を取得 ----------
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = su.location_factory_structure_id 
                    and st_f.factory_id in (0, su.location_factory_structure_id)
            ) 
            and tra.structure_id = su.location_factory_structure_id
    ) as factory_name                           -- 工場
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = su.location_plant_structure_id 
                    and st_f.factory_id in (0, su.location_factory_structure_id)
            ) 
            and tra.structure_id = su.location_plant_structure_id
    ) as plant_name                             -- プラント
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = su.location_series_structure_id 
                    and st_f.factory_id in (0, su.location_factory_structure_id)
            ) 
            and tra.structure_id = su.location_series_structure_id
    ) as series_name                            -- 系列(工程)
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = su.location_stroke_structure_id 
                    and st_f.factory_id in (0, su.location_factory_structure_id)
            ) 
            and tra.structure_id = su.location_stroke_structure_id
    ) as stroke_name                            -- 工程(系列)
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = su.location_facility_structure_id 
                    and st_f.factory_id in (0, su.location_factory_structure_id)
            ) 
            and tra.structure_id = su.location_facility_structure_id
    ) as facility_name                          -- 設備
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = su.job_structure_id 
                    and st_f.factory_id in (0, su.location_factory_structure_id)
            ) 
            and tra.structure_id = su.job_structure_id
    ) as job_name                               -- 職種
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = failure.phenomenon_structure_id 
                    and st_f.factory_id in (0, su.location_factory_structure_id)
            ) 
            and tra.structure_id = failure.phenomenon_structure_id
    ) as phenomenon_name                        -- 現象
    ,case 
        when isnull(hi.call_count, 0) >= 1 then
            (select name from call_count_name where division = 1) 
        else
            (select name from call_count_name where division = 2)
        end as call_count                      -- 呼出
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = su.stop_system_structure_id 
                    and st_f.factory_id in (0, su.location_factory_structure_id)
            ) 
            and tra.structure_id = su.stop_system_structure_id
    ) as stop_system_name                       -- 系停止
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = su.sudden_division_structure_id 
                    and st_f.factory_id in (0, su.location_factory_structure_id)
            ) 
            and tra.structure_id = su.sudden_division_structure_id
    ) as sudden_name                            -- 突発区分
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = su.mq_class_structure_id 
                    and st_f.factory_id in (0, su.location_factory_structure_id)
            ) 
            and tra.structure_id = su.mq_class_structure_id
    ) as mq_name                                -- MQ分類
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = hi.maintenance_season_structure_id 
                    and st_f.factory_id in (0, su.location_factory_structure_id)
            ) 
            and tra.structure_id = hi.maintenance_season_structure_id
    ) as maintenance_season_name                -- 保全時期
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = re.discovery_methods_structure_id 
                    and st_f.factory_id in (0, su.location_factory_structure_id)
            ) 
            and tra.structure_id = re.discovery_methods_structure_id
    ) as discovery_methods_name                 -- 発見方法
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = hi.actual_result_structure_id 
                    and st_f.factory_id in (0, su.location_factory_structure_id)
            ) 
            and tra.structure_id = hi.actual_result_structure_id
    ) as actual_result_name                     -- 実績結果
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = re.maintenance_department_clerk_id 
                    and st_f.factory_id in (0, su.location_factory_structure_id)
            ) 
            and tra.structure_id = re.maintenance_department_clerk_id
    ) as maintenance_department_name                 -- 担当部門
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = machine.location_factory_structure_id 
                    and st_f.factory_id in (0, machine.location_factory_structure_id)
            ) 
            and tra.structure_id = machine.location_factory_structure_id
    ) as machine_factory_name                   -- 工場(機器)
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = machine.location_plant_structure_id 
                    and st_f.factory_id in (0, machine.location_factory_structure_id)
            ) 
            and tra.structure_id = machine.location_plant_structure_id
    ) as machine_plant_name                     -- 工程(機器)
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = machine.location_series_structure_id 
                    and st_f.factory_id in (0, machine.location_factory_structure_id)
            ) 
            and tra.structure_id = machine.location_series_structure_id
    ) as machine_series_name                    -- 系列(機器)
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = machine.location_stroke_structure_id 
                    and st_f.factory_id in (0, machine.location_factory_structure_id)
            ) 
            and tra.structure_id = machine.location_stroke_structure_id
    ) as machine_stroke_name                    -- 設備(機器)
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = machine.location_facility_structure_id 
                    and st_f.factory_id in (0, machine.location_factory_structure_id)
            ) 
            and tra.structure_id = machine.location_facility_structure_id
    ) as machine_facility_name                  -- 場所5(機器)
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = machine.job_kind_structure_id 
                    and st_f.factory_id in (0, machine.location_factory_structure_id)
            ) 
            and tra.structure_id = machine.job_kind_structure_id
    ) as machine_job_name                       -- 職種(機器)
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = machine.equipment_level_structure_id 
                    and st_f.factory_id in (0, machine.location_factory_structure_id)
            ) 
            and tra.structure_id = machine.equipment_level_structure_id
    ) as equipment_level_name                   -- 機器レベル
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = machine.importance_structure_id 
                    and st_f.factory_id in (0, machine.location_factory_structure_id)
            ) 
            and tra.structure_id = machine.importance_structure_id
    ) as importance_name                        -- 重要度
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = machine.conservation_structure_id 
                    and st_f.factory_id in (0, machine.location_factory_structure_id)
            ) 
            and tra.structure_id = machine.conservation_structure_id
    ) as conservation_name                      -- 保全方式
    , [dbo].[get_applicable_laws]( 
        machine.machine_id
        , 1
        , machine.location_factory_structure_id
        , @LanguageId
    ) as applicable_laws_name1
    ,                                           -- 適用法規１
    [dbo].[get_applicable_laws]( 
        machine.machine_id
        , 2
        , machine.location_factory_structure_id
        , @LanguageId
    ) as applicable_laws_name2
    ,                                           -- 適用法規２
    [dbo].[get_applicable_laws]( 
        machine.machine_id
        , 3
        , machine.location_factory_structure_id
        , @LanguageId
    ) as applicable_laws_name3
    ,                                           -- 適用法規３
    [dbo].[get_applicable_laws]( 
        machine.machine_id
        , 4
        , machine.location_factory_structure_id
        , @LanguageId
    ) as applicable_laws_name4
    ,                                           -- 適用法規４
    [dbo].[get_applicable_laws]( 
        machine.machine_id
        , 5
        , machine.location_factory_structure_id
        , @LanguageId
    ) as applicable_laws_name5                  -- 適用法規５
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = machine.job_large_classfication_structure_id 
                    and st_f.factory_id in (0, machine.location_factory_structure_id)
            ) 
            and tra.structure_id = machine.job_large_classfication_structure_id
    ) as job_large_name                         -- 機種大分類
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = machine.job_middle_classfication_structure_id 
                    and st_f.factory_id in (0, machine.location_factory_structure_id)
            ) 
            and tra.structure_id = machine.job_middle_classfication_structure_id
    ) as job_middle_name                        -- 機種中分類
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = machine.job_small_classfication_structure_id 
                    and st_f.factory_id in (0, machine.location_factory_structure_id)
            ) 
            and tra.structure_id = machine.job_small_classfication_structure_id
    ) as job_small_name                         -- 機種小分類
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = equipment.use_segment_structure_id 
                    and st_f.factory_id in (0, machine.location_factory_structure_id)
            ) 
            and tra.structure_id = equipment.use_segment_structure_id
    ) as use_segment_name                       -- 使用区分
    ,case 
        when equipment.circulation_target_flg = 1 then
            (select name from circulation where division = 1) 
        else
            (select name from circulation where division = 2)
        end as circulation_target_flg                      -- 循環対象
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = equipment.manufacturer_structure_id 
                    and st_f.factory_id in (0, machine.location_factory_structure_id)
            ) 
            and tra.structure_id = equipment.manufacturer_structure_id
    ) as manufacture_name                       -- メーカー
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = failure.failure_cause_structure_id 
                    and st_f.factory_id in (0, su.location_factory_structure_id)
            ) 
            and tra.structure_id = failure.failure_cause_structure_id
    ) as failure_cause_name                     -- 故障原因
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = failure.failure_cause_personality_structure_id 
                    and st_f.factory_id in (0, su.location_factory_structure_id)
            ) 
            and tra.structure_id = failure.failure_cause_personality_structure_id
    ) as failure_cause_personality_name         -- 原因性格
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId 
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = failure.treatment_measure_structure_id 
                    and st_f.factory_id in (0, su.location_factory_structure_id)
            ) 
            and tra.structure_id = failure.treatment_measure_structure_id
    ) as treatment_measure_name                 -- 処置・対策
    ---------- 点検か故障で取得先が異なる項目 ----------
    , case
        when
        (case 
            when failure.history_failure_id is null -- 故障情報のデータが存在しない(=点検のデータ)場合
                then content.follow_flg 
            else                                    -- 故障情報のデータが存在する(=故障のデータ)場合
            failure.follow_flg 
        end) = 1
            then (select name from follow)
        else ''
        end as follow_flg                       -- フォロー要否
    , case 
        when failure.history_failure_id is null -- 故障情報のデータが存在しない(=点検のデータ)場合
            then content.follow_plan_date 
        else                                    -- 故障情報のデータが存在する(=故障のデータ)場合
        failure.follow_plan_date 
        end as follow_plan_date                 -- フォロー年月
    , case 
        when failure.history_failure_id is null -- 故障情報のデータが存在しない(=点検のデータ)場合
            then content.follow_content 
        else                                    -- 故障情報のデータが存在する(=故障のデータ)場合
        failure.follow_content 
        end as follow_content                   -- フォロー内容
    , case 
        when failure.history_failure_id is null -- 故障情報のデータが存在しない(=点検のデータ)場合
            then ( 
            select
                tra.translation_text 
            from
                v_structure_item_all as tra 
            where
                tra.language_id = @LanguageId 
                and tra.location_structure_id = ( 
                    select
                        max(st_f.factory_id) 
                    from
                        #temp_structure_factory as st_f 
                    where
                        st_f.structure_id = site.inspection_site_structure_id 
                        and st_f.factory_id in (0, su.location_factory_structure_id)
                ) 
                and tra.structure_id = site.inspection_site_structure_id
        ) 
        else                                    -- 故障情報のデータが存在する(=故障のデータ)場合
        failure.maintenance_site 
        end as maintenance_site                 -- 保全部位
    , case 
        when failure.history_failure_id is null -- 故障情報のデータが存在しない(=点検のデータ)場合
            then ( 
            select
                tra.translation_text 
            from
                v_structure_item_all as tra 
            where
                tra.language_id = @LanguageId 
                and tra.location_structure_id = ( 
                    select
                        max(st_f.factory_id) 
                    from
                        #temp_structure_factory as st_f 
                    where
                        st_f.structure_id = content.inspection_content_structure_id 
                        and st_f.factory_id in (0, su.location_factory_structure_id)
                ) 
                and tra.structure_id = content.inspection_content_structure_id
        ) 
        else                                    -- 故障情報のデータが存在する(=故障のデータ)場合
        failure.maintenance_content 
        end as maintenance_content              -- 保全項目
from
    ma_summary su                               -- 保全活動件名
    left join ma_plan pl                        -- 保全計画
        on su.summary_id = pl.summary_id 
    left join ma_history hi                     -- 保全履歴
        on su.summary_id = hi.summary_id 
    left join ma_history_machine hm             -- 保全履歴機器
        on hi.history_id = hm.history_id 
    left join ma_history_inspection_site site   -- 保全履歴機器部位
        on hm.history_machine_id = site.history_machine_id 
    left join ma_history_inspection_content content -- 保全履歴点検内容
        on site.history_inspection_site_id = content.history_inspection_site_id 
    left join ma_history_failure failure        -- 保全履歴故障情報
        on hi.history_id = failure.history_id 
    left join ma_request re                     -- 保全依頼
        on su.summary_id = re.summary_id 
    left join mc_machine machine                -- 機番情報
        on hm.machine_id = machine.machine_id 
    left join mc_equipment equipment            -- 機器情報
        on machine.machine_id = equipment.machine_id    -- 場所階層ツリーで選択されている項目を抽出条件とするため、ツリーで選択された項目の構成IDが格納されている一時テーブルと内部結合をする
    inner join #temp_location tl
        on su.location_structure_id = tl.structure_id

    -- 職種(出力対象)の並び順が格納されている一時テーブル
    inner join #temp_job_order tjo
       on su.job_structure_id = tjo.structure_id
where
    -- 画面で指定された年月の初日から末日までが取得対象
    su.completion_date between @TargetStartDate and @TargetEndDate
order by
    tjo.sort             -- 職種
    , su.completion_date -- 完了日