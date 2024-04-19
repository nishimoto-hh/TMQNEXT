WITH CirculationTargetTrue AS(-- 「対象」の翻訳を取得
    SELECT
        tra.translation_text
    FROM
        ms_translation tra
    WHERE
        tra.location_structure_id = 0
    AND tra.translation_id = 111160071
    AND tra.language_id = (SELECT DISTINCT languageId FROM #temp)
)
, CirculationTargetFalse AS (-- 「非対象」の翻訳を取得
    SELECT
        tra.translation_text
    FROM
        ms_translation tra
    WHERE
        tra.location_structure_id = 0
    AND tra.translation_id = 111270034
    AND tra.language_id = (SELECT DISTINCT languageId FROM #temp)
)

SELECT
      mc.machine_no                                     -- 機器番号
    , mc.machine_name                                   -- 機器名称
    , mc.job_structure_id                               -- 職種機種階層id ※共通処理にて使用
    , mc.location_structure_id                          -- 機種階層id ※共通処理にて使用
    --職種(翻訳)
    ,(
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
                AND st_f.factory_id IN(0, mc.location_factory_structure_id)
            )
        AND tra.structure_id = mc.job_kind_structure_id
    ) AS job_name
    --工場(翻訳)
    ,(
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
                AND st_f.factory_id IN(0, mc.location_factory_structure_id)
            )
        AND tra.structure_id = mc.location_factory_structure_id
    ) AS factory_name
    --プラント(翻訳)
    ,(
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
                AND st_f.factory_id IN(0, mc.location_factory_structure_id)
            )
        AND tra.structure_id = mc.location_plant_structure_id
    ) AS plant_name
    --系列(翻訳)
    ,(
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
                AND st_f.factory_id IN(0, mc.location_factory_structure_id)
            )
        AND tra.structure_id = mc.location_series_structure_id
    ) AS series_name
    --工程(翻訳)
    ,(
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
                AND st_f.factory_id IN(0, mc.location_factory_structure_id)
            )
        AND tra.structure_id = mc.location_stroke_structure_id
    ) AS stroke_name
    --設備(翻訳)
    ,(
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
                AND st_f.factory_id IN(0, mc.location_factory_structure_id)
            )
        AND tra.structure_id = mc.location_facility_structure_id
    ) AS facility_name
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
                    st_f.structure_id = mc.importance_structure_id
                    AND st_f.factory_id IN (0, mc.location_factory_structure_id)
            )
            AND tra.structure_id = mc.importance_structure_id
      ) AS importance_name                              -- 重要度名称
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
                    st_f.structure_id = mc.conservation_structure_id
                    AND st_f.factory_id IN (0, mc.location_factory_structure_id)
            )
            AND tra.structure_id = mc.conservation_structure_id
      ) AS conservation_name                            -- 保全方式名称
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
      ) AS equipment_level_name                         -- 機器レベル名称
    , mc.installation_location                          -- 設置場所
    , mc.number_of_installation                         -- 設置台数
    , CASE
        WHEN mc.date_of_installation IS NOT NULL THEN FORMAT(mc.date_of_installation, 'yyyy/MM') 
        ELSE ''
    END AS date_of_installation                         -- 設置年月
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
                    st_f.structure_id = eq.use_segment_structure_id
                    AND st_f.factory_id IN (0, mc.location_factory_structure_id)
            )
            AND tra.structure_id = eq.use_segment_structure_id
      ) AS use_segment_name                        -- 使用区分名称
    , CASE
        WHEN eq.circulation_target_flg = 1 THEN CirculationTargetTrue.translation_text -- 対象
        WHEN eq.circulation_target_flg = 0 THEN CirculationTargetFalse.translation_text -- 非対象
        ELSE ''
    END AS circulation_target                           -- 循環対象
    , eq.fixed_asset_no                                 -- 固定資産番号
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
      ) AS applicable_laws_name1                        -- 適用法規１
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
      ) AS applicable_laws_name2                        -- 適用法規２
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
      ) AS applicable_laws_name3                        -- 適用法規３
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
      ) AS applicable_laws_name4                        -- 適用法規４
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
      ) AS applicable_laws_name5                        -- 適用法規５
    , mc.machine_note                                   -- 機番メモ
        -- 機種大分類(翻訳)
    ,(
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
                AND st_f.factory_id IN(0, mc.location_factory_structure_id)
            )
        AND tra.structure_id = mc.job_large_classfication_structure_id
    ) AS large_classfication_name
    -- 機種中分類
    ,(
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
                AND st_f.factory_id IN(0, mc.location_factory_structure_id)
            )
        AND tra.structure_id = mc.job_middle_classfication_structure_id
    ) AS middle_classfication_name
    -- 機種小分類
    ,(
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
                AND st_f.factory_id IN(0, mc.location_factory_structure_id)
            )
        AND tra.structure_id = mc.job_small_classfication_structure_id
    ) AS small_classfication_name
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
                    st_f.structure_id = eq.manufacturer_structure_id
                    AND st_f.factory_id IN (0, mc.location_factory_structure_id)
            )
            AND tra.structure_id = eq.manufacturer_structure_id
      ) AS manufacturer_name                            -- メーカー名称
    , eq.manufacturer_type                              -- メーカー型式
    , eq.model_no                                       -- 型式コード
    , eq.serial_no                                      -- 製造番号
    , CASE
        WHEN eq.date_of_manufacture IS NOT NULL THEN FORMAT(eq.date_of_manufacture, 'yyyy/MM')
        ELSE ''
    END AS date_of_manufacture                          -- 製造年月
    , eq.equipment_note                                 -- 機器メモ
    , '1' AS output_report_location_name_got_flg                -- 機能場所名称情報取得済フラグ（帳票用）
    , '1' AS output_report_job_name_got_flg                     -- 職種・機種名称情報取得済フラグ（帳票用）

FROM
    mc_machine mc
    INNER JOIN #temp temp
        ON mc.machine_id = temp.Key1
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
                    , DENSE_RANK() OVER( PARTITION BY machine_id  ORDER BY applicable_laws_id ) AS rnk
                FROM
                    mc_applicable_laws
                ) app
            WHERE
                app.rnk = 1
            ) app1                      -- 適用法規１
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
                    , DENSE_RANK() OVER( PARTITION BY machine_id  ORDER BY applicable_laws_id ) AS rnk
                FROM
                    mc_applicable_laws
                ) app
            WHERE
                app.rnk = 2
            ) app2                      -- 適用法規２
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
                    , DENSE_RANK() OVER( PARTITION BY machine_id  ORDER BY applicable_laws_id ) AS rnk
                FROM
                    mc_applicable_laws
                ) app
            WHERE
                app.rnk = 3
            ) app3                      -- 適用法規３
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
                    , DENSE_RANK() OVER( PARTITION BY machine_id  ORDER BY applicable_laws_id ) AS rnk
                FROM
                    mc_applicable_laws
                ) app
            WHERE
                app.rnk = 4
            ) app4                      -- 適用法規４
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
                    , DENSE_RANK() OVER( PARTITION BY machine_id  ORDER BY applicable_laws_id ) AS rnk
                FROM
                    mc_applicable_laws
                ) app
            WHERE
                app.rnk = 5
            ) app5                      -- 適用法規５
        ON mc.machine_id = app5.machine_id
    CROSS JOIN
       CirculationTargetTrue --「対象」の翻訳
    CROSS JOIN
       CirculationTargetFalse -- 「非対象」の翻訳
ORDER BY
    mc.machine_no                                       -- 機器番号
