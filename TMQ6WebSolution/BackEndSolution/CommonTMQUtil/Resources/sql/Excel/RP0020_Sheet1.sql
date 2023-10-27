DECLARE @TranslationIdCirculationTargetTrue int;
DECLARE @TranslationIdCirculationTargetFalse int;
SET @TranslationIdCirculationTargetTrue = 111160071;    -- 対象
SET @TranslationIdCirculationTargetFalse = 111270034;   -- 非対象

SELECT
      mc.machine_no                                     -- 機器番号
    , mc.machine_name                                   -- 機器名称
    , mc.job_structure_id                               -- 職種機種階層id ※共通処理にて使用
    , '' AS job_name                                    -- 職種 ※共通処理にて設定                
    , mc.location_structure_id                          -- 機種階層id ※共通処理にて使用
    , '' AS factory_name                                -- 工場 ※共通処理にて設定
    , '' AS plant_name                                  -- プラント ※共通処理にて設定
    , '' AS series_name                                 -- 系列 ※共通処理にて設定
    , '' AS stroke_name                                 -- 工程 ※共通処理にて設定
    , '' AS facility_name                               -- 設備 ※共通処理にて設定
    , [dbo].[get_v_structure_item](mc.importance_structure_id, temp.factoryId, temp.languageId) AS importance_name            -- 重要度名称
    , [dbo].[get_v_structure_item](mc.conservation_structure_id, temp.factoryId, temp.languageId) AS conservation_name        -- 保全方式名称
    , [dbo].[get_v_structure_item](mc.equipment_level_structure_id, temp.factoryId, temp.languageId) AS equipment_level_name  -- 機器レベル名称
    , mc.installation_location                          -- 設置場所
    , mc.number_of_installation                         -- 設置台数
    , CASE
        WHEN mc.date_of_installation IS NOT NULL THEN '''' + FORMAT(mc.date_of_installation, 'yyyy/MM') 
        ELSE ''
    END AS date_of_installation                         -- 設置年月
    , [dbo].[get_v_structure_item](eq.use_segment_structure_id, temp.factoryId, temp.languageId) AS use_segment_name          -- 使用区分名称
    , CASE
        -- WHEN eq.circulation_target_flg = 1 THEN '対象'
        -- WHEN eq.circulation_target_flg = 0 THEN '非対象'
        WHEN eq.circulation_target_flg = 1 THEN [dbo].[get_rep_translation_text](temp.factoryId, @TranslationIdCirculationTargetTrue, temp.languageId)
        WHEN eq.circulation_target_flg = 0 THEN [dbo].[get_rep_translation_text](temp.factoryId, @TranslationIdCirculationTargetFalse, temp.languageId)
        ELSE ''
    END AS circulation_target                           -- 循環対象
    , eq.fixed_asset_no                                 -- 固定資産番号
    , [dbo].[get_applicable_laws](mc.machine_id, 1, temp.factoryId, temp.languageId) AS applicable_laws_name1   -- 適用法規１
    , [dbo].[get_applicable_laws](mc.machine_id, 2, temp.factoryId, temp.languageId) AS applicable_laws_name2   -- 適用法規２
    , [dbo].[get_applicable_laws](mc.machine_id, 3, temp.factoryId, temp.languageId) AS applicable_laws_name3   -- 適用法規３
    , [dbo].[get_applicable_laws](mc.machine_id, 4, temp.factoryId, temp.languageId) AS applicable_laws_name4   -- 適用法規４
    , [dbo].[get_applicable_laws](mc.machine_id, 5, temp.factoryId, temp.languageId) AS applicable_laws_name5   -- 適用法規５
    , mc.machine_note                                   -- 機番メモ
    , '' AS large_classfication_name                    -- 機種大分類 ※共通処理にて設定
    , '' AS middle_classfication_name                   -- 機種中分類 ※共通処理にて設定
    , '' AS small_classfication_name                    -- 機種小分類 ※共通処理にて設定
    , [dbo].[get_v_structure_item](eq.manufacturer_structure_id, temp.factoryId, temp.languageId) AS manufacturer_name        -- メーカー名称
    , eq.manufacturer_type                              -- メーカー型式
    , eq.model_no                                       -- 型式コード
    , eq.serial_no                                      -- 製造番号
    , CASE
        WHEN eq.date_of_manufacture IS NOT NULL THEN '''' + FORMAT(eq.date_of_manufacture, 'yyyy/MM')
        ELSE ''
    END AS date_of_manufacture                          -- 製造年月
    , eq.equipment_note                                 -- 機器メモ
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

ORDER BY
    mc.machine_no                                       -- 機器番号
