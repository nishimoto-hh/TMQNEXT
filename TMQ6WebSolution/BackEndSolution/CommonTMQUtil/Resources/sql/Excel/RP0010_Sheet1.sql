DECLARE @TranslationIdCirculationTargetTrue int;
DECLARE @TranslationIdCirculationTargetFalse int;
SET @TranslationIdCirculationTargetTrue = 111160071;    -- 対象
SET @TranslationIdCirculationTargetFalse = 111270034;   -- 非対象

SELECT
    mc.machine_no                                       -- 機器番号
    , mc.machine_name                                   -- 機器名称
    , mc.job_structure_id                               -- 職種機種階層id ※共通処理にて使用
    , '' AS job_name                                    -- 職種 ※共通処理にて設定                
    , mc.location_structure_id                          -- 機種階層id ※共通処理にて使用
    , '' AS factory_name                                -- 工場 ※共通処理にて設定
    , '' AS plant_name                                  -- プラント ※共通処理にて設定
    , '' AS series_name                                 -- 系列 ※共通処理にて設定
    , '' AS stroke_name                                 -- 工程 ※共通処理にて設定
    , '' AS facility_name                               -- 設備 ※共通処理にて設定
--    , mc.importance_structure_id                      -- 重要度
--    , vimp.translation_text AS importance_name          -- 重要度名称
    , [dbo].[get_v_structure_item](mc.importance_structure_id, temp.factoryId, temp.languageId) AS importance_name            -- 重要度名称
--    , mc.conservation_structure_id                    -- 保全方式
--    , vcon.translation_text AS conservation_name        -- 保全方式名称
    , [dbo].[get_v_structure_item](mc.conservation_structure_id, temp.factoryId, temp.languageId) AS conservation_name        -- 保全方式名称
--    , mc.equipment_level_structure_id                 -- 機器レベル
--    , vequ.translation_text AS equipment_level_name     -- 機器レベル名称
    , [dbo].[get_v_structure_item](mc.equipment_level_structure_id, temp.factoryId, temp.languageId) AS equipment_level_name  -- 機器レベル名称
    , mc.installation_location                          -- 設置場所
    , mc.number_of_installation                         -- 設置台数
    , CASE
        WHEN mc.date_of_installation IS NOT NULL THEN '''' + FORMAT(mc.date_of_installation, 'yyyy/MM') 
        ELSE ''
    END AS date_of_installation                         -- 設置年月
--    , eq.use_segment_structure_id                     -- 使用区分
--    , vuse.translation_text AS use_segment_name         -- 使用区分名称
    , [dbo].[get_v_structure_item](eq.use_segment_structure_id, temp.factoryId, temp.languageId) AS use_segment_name            -- 使用区分名称
--    , eq.circulation_target_flg                       -- 循環対象フラグ
    , CASE
--        WHEN eq.circulation_target_flg = 1 THEN '対象'
--        WHEN eq.circulation_target_flg = 0 THEN '非対象'
        WHEN eq.circulation_target_flg = 1 THEN [dbo].[get_rep_translation_text](temp.factoryId, @TranslationIdCirculationTargetTrue, temp.languageId)
        WHEN eq.circulation_target_flg = 0 THEN [dbo].[get_rep_translation_text](temp.factoryId, @TranslationIdCirculationTargetFalse, temp.languageId)
        ELSE ''
    END AS circulation_target                           -- 循環対象
    , eq.fixed_asset_no                                 -- 固定資産番号
--    , vapp1.translation_text AS applicable_laws_name1   -- 適用法規１
--    , vapp2.translation_text AS applicable_laws_name2   -- 適用法規２
--    , vapp3.translation_text AS applicable_laws_name3   -- 適用法規３
--    , vapp4.translation_text AS applicable_laws_name4   -- 適用法規４
--    , vapp5.translation_text AS applicable_laws_name5   -- 適用法規５
    , [dbo].[get_v_structure_item](app1.applicable_laws_structure_id, temp.factoryId, temp.languageId) AS applicable_laws_name1   -- 適用法規１
    , [dbo].[get_v_structure_item](app2.applicable_laws_structure_id, temp.factoryId, temp.languageId) AS applicable_laws_name2   -- 適用法規２
    , [dbo].[get_v_structure_item](app3.applicable_laws_structure_id, temp.factoryId, temp.languageId) AS applicable_laws_name3   -- 適用法規３
    , [dbo].[get_v_structure_item](app4.applicable_laws_structure_id, temp.factoryId, temp.languageId) AS applicable_laws_name4   -- 適用法規４
    , [dbo].[get_v_structure_item](app5.applicable_laws_structure_id, temp.factoryId, temp.languageId) AS applicable_laws_name5   -- 適用法規５
    , mc.machine_note                                   -- 機番メモ
    , '' AS large_classfication_name                    -- 機種大分類 ※共通処理にて設定
    , '' AS middle_classfication_name                   -- 機種中分類 ※共通処理にて設定
    , '' AS small_classfication_name                    -- 機種小分類 ※共通処理にて設定
--    , eq.manufacturer_structure_id                    -- メーカー
--    , vman.translation_text AS manufacturer_name        -- メーカー名称
    , [dbo].[get_v_structure_item](eq.manufacturer_structure_id, temp.factoryId, temp.languageId) AS manufacturer_name            -- メーカー名称
    , eq.manufacturer_type                              -- メーカー型式
    , eq.model_no                                       -- 型式コード
    , eq.serial_no                                      -- 製造番号
    , CASE
        WHEN eq.date_of_manufacture IS NOT NULL THEN '''' + FORMAT(eq.date_of_manufacture, 'yyyy/MM')
        ELSE ''
    END AS date_of_manufacture                          -- 製造年月
    , eq.equipment_note                                 -- 機器メモ
    , '' AS iso_division                                -- ISO区分 ※該当項目なし
    , '' AS performance                                 -- 性能 ※該当項目なし
    , mc.machine_id                                     -- 機番id ※画面にて取得(帳票はなし)
    , eq.equipment_id                                   -- 機器id ※画面にて取得(帳票はなし)
    , eq.delivery_date                                  -- 納期 ※画面にて取得(帳票はなし)
    , eq.maintainance_kind_manage                       -- 点検種別毎管理 ※画面にて取得(帳票はなし)

FROM
    mc_machine mc
    INNER JOIN #temp temp
        ON mc.machine_id = temp.Key1
    LEFT JOIN mc_equipment eq 
        ON mc.machine_id = eq.machine_id
--    LEFT JOIN v_structure_item vimp     -- 重要度
--        ON mc.importance_structure_id = vimp.structure_id
--    LEFT JOIN v_structure_item vcon     -- 保全方式
--        ON mc.conservation_structure_id = vcon.structure_id
--    LEFT JOIN v_structure_item vequ     -- 機器レベル
--        ON mc.equipment_level_structure_id = vequ.structure_id
--    LEFT JOIN v_structure_item vuse     -- 使用区分
--        ON mc.equipment_level_structure_id = vuse.structure_id
--    LEFT JOIN v_structure_item vman     -- メーカー
--        ON mc.equipment_level_structure_id = vman.structure_id
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
--    LEFT JOIN v_structure_item vapp1    -- 適用法規１
--        ON app1.applicable_laws_structure_id = vapp1.structure_id
--    LEFT JOIN v_structure_item vapp2    -- 適用法規２
--        ON app2.applicable_laws_structure_id = vapp2.structure_id
--    LEFT JOIN v_structure_item vapp3    -- 適用法規３
--        ON app3.applicable_laws_structure_id = vapp3.structure_id
--    LEFT JOIN v_structure_item vapp4    -- 適用法規４
--        ON app4.applicable_laws_structure_id = vapp4.structure_id
--    LEFT JOIN v_structure_item vapp5    -- 適用法規５
--        ON app5.applicable_laws_structure_id = vapp5.structure_id
ORDER BY
    mc.machine_no                                       -- 機器番号
