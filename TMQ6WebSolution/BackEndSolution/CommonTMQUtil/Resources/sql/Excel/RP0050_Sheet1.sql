DECLARE @w_key1 bigint       -- 機器ID
DECLARE @w_key2 nvarchar(2)  -- 言語ID
DECLARE @w_key3 int          -- 本務工場
DECLARE @w_idx  int          -- ソート用インデックス
DECLARE @TranslationIdCirculationTargetTrue int;
DECLARE @TranslationIdCirculationTargetFalse int;
DECLARE @TranslationIdExists int;
DECLARE @TranslationIdNotExists int;
SET @TranslationIdCirculationTargetTrue = 111160071;    -- 対象
SET @TranslationIdCirculationTargetFalse = 111270034;   -- 非対象
SET @TranslationIdExists = 111010021;                   -- あり
SET @TranslationIdNotExists = 111210005;                -- なし

CREATE TABLE #temp_rep(
	job_structure_id                int,            -- 職種機種階層ID
	location_structure_id           int,            -- 機能場所階層ID
    job_structure_id2               int,            -- 職種機種階層ID(機器)
	location_structure_id2          int,            -- 機能場所階層ID(機器)
	subject                         nvarchar(800),  -- 件名
	phenomenon_name                 nvarchar(800),  -- 現象
	call_count_name                 nvarchar(800),  -- 呼出
	stop_count_name                 nvarchar(800),  -- 系停止
	stop_time                       decimal(13, 3), -- 停止時間
	work_purpose_name               nvarchar(800),  -- 目的区分
	sudden_division_name            nvarchar(800),  -- 作業区分
	mq_class_name                   nvarchar(800),  -- MQ分類
	occurrence_date                 date,           -- 発生日
    expected_construction_date      date,           -- 着工予定日
    completion_date                 date,           -- 完了年月日
    total_budget_cost               decimal(13, 3), -- 予算費用
    expenditure                     decimal(13, 3), -- 実績費用
    loss_absence　　                int,            -- 休損量
    maintenance_season_name         nvarchar(800),  -- 時期
    construction_personnel_name     nvarchar(800),  -- 担当者
	-- total_working_time              decimal(13, 3), -- 作業時間
	-- working_time_self               decimal(13, 3), -- 自社
	total_working_time              nvarchar(800), -- 作業時間
	working_time_self               nvarchar(800), -- 自社
    construction_company            nvarchar(800),  -- 施工会社
    working_time_company            decimal(13, 3), -- 施工会社工数
	plan_implementation_content     nvarchar(800),  -- 件名概要
	subject_note                    nvarchar(800),  -- メモ
    maintenance_count               int,            -- カウント件数
    discovery_methods_name          nvarchar(800),  -- 発見方法
    actual_result_name              nvarchar(800),  -- 実績結果
    budget_management_name          nvarchar(800),  -- 予算管理区分
	budget_personality_name         nvarchar(800),  -- 予算性格区分
	machine_no                      nvarchar(800),  -- 機器番号
	machine_name                    nvarchar(800),  -- 機器名称
	-- 36～41
	equipment_level                 nvarchar(800),  -- 機器レベル
	installation_location           nvarchar(800),  -- 設置場所
    number_of_installation          int,            -- 設置台数
	date_of_installation	        nvarchar(800),  -- 設置年月
	importance_name                 nvarchar(800),  -- 重要度
    conservation_name               nvarchar(800),  -- 保全方式
    applicable_laws_name1           nvarchar(800),  -- 適用法規１
    applicable_laws_name2           nvarchar(800),  -- 適用法規２
    applicable_laws_name3           nvarchar(800),  -- 適用法規３
    applicable_laws_name4           nvarchar(800),  -- 適用法規４
    applicable_laws_name5           nvarchar(800),  -- 適用法規５
    machine_note                    nvarchar(800),  -- 機番メモ
    -- 54～56
    use_segment_name                nvarchar(800),  -- 使用区分
    circulation_target              nvarchar(800),  -- 循環対象
	fixed_asset_no                  nvarchar(200),  -- 固定資産番号
    manufacturer_name               nvarchar(800),  -- メーカー
    manufacturer_type               nvarchar(200),  -- メーカー型式
    model_no                        nvarchar(200),  -- 型式コード
    serial_no                       nvarchar(200),  -- 製造番号
    date_of_manufacture             nvarchar(800),  -- 製造年月
    equipment_note                  nvarchar(800),  -- 機器メモ
    work_item_name                  nvarchar(800),  -- 作業項目
    treatment_measure_note          nvarchar(800),  -- 作業内容・結果
	failure_cause_addition_note     nvarchar(800),  -- 故障原因
	failure_cause_personality_note  nvarchar(800),  -- 原因性格
	treatment_measure_name          nvarchar(800),  -- 処置対策
    follow_flg                      nvarchar(800),  -- フォロー要否
    follow_completion_date          nvarchar(800),  -- フォロー年月
    follow_content                  nvarchar(800),  -- フォロー内容
	maintenance_site                nvarchar(800),  -- 保全部位
	inspection_content_name         nvarchar(800),  -- 作業項目(保全項目)
	maintenance_content             nvarchar(800),  -- 作業項目(保全履歴内容)
    -- 共通処理用
    factory_name2                   nvarchar(800), -- 工場名(機器)
    plant_name2                     nvarchar(800), -- プラント名(機器)
    series_name2                    nvarchar(800), -- 系列名(機器)
    stroke_name2                    nvarchar(800), -- 工程名(機器)
    facility_name2                  nvarchar(800), -- 設備名(機器)
    job_name2                       nvarchar(800), -- 職種名(機器)
    large_classfication_name2       nvarchar(800),  -- 大分類(機器)
    middle_classfication_name2      nvarchar(800),  -- 中分類(機器)
    small_classfication_name2       nvarchar(800),  -- 小分類(機器)
	seq                             int,            -- 一時テーブル連番
    idx                             int             -- ソート用インデックス
)

-- カーソル定義
DECLARE cur_key CURSOR FOR
	select Key1, languageId, factoryId from #temp

-- カーソルオープン
OPEN cur_key;

-- １レコード取得
FETCH NEXT FROM cur_key INTO @w_key1, @w_key2, @w_key3;

SET @w_idx = 0;

-- データの行数分ループ処理を実行する
WHILE @@FETCH_STATUS = 0
BEGIN

    SET @w_idx = @w_idx + 1;

	-- 機器別保全保全履歴一覧 情報取得
    -- 複数行に対応させるため、一旦一時テーブルに保存
    INSERT INTO #temp_rep
    SELECT *, row_number() over(order by ISNULL(completion_date,'9999/12/31') desc) as row_no, @w_idx
    FROM
    (SELECT
        sm.job_structure_id,
	    sm.location_structure_id,
        mc.job_structure_id AS job_structure_id2,
        mc.location_structure_id AS location_structure_id2,
	    sm.subject,    -- 件名
	   	'' AS phenomenon_name,                    -- 現象
		-- CASE WHEN ISNULL(call_count, 0) >= 1 THEN '有り' ELSE 'なし' END AS call_count_name,
	    -- CASE WHEN ISNULL(stop_count, 0) >= 1 THEN '有り' ELSE 'なし' END AS stop_count_name, 
		CASE WHEN ISNULL(call_count, 0) >= 1 THEN 
            [dbo].[get_rep_translation_text](@w_key3, @TranslationIdExists, @w_key2)
        ELSE 
            [dbo].[get_rep_translation_text](@w_key3, @TranslationIdNotExists, @w_key2)
        END AS call_count_name,
	    CASE WHEN ISNULL(maintenance_count, 0) >= 1 THEN 
            [dbo].[get_rep_translation_text](@w_key3, @TranslationIdExists, @w_key2)
        ELSE 
            [dbo].[get_rep_translation_text](@w_key3, @TranslationIdNotExists, @w_key2)
        END AS stop_count_name, 
	    stop_time,
        '' AS work_purpose_name,                     -- 目的区分
	    [dbo].[get_v_structure_item](sudden_division_structure_id, @w_key3, @w_key2) AS sudden_division_name,   -- 突発区分
	    [dbo].[get_v_structure_item](mq_class_structure_id, @w_key3, @w_key2) AS mq_class_name,                 -- MQ分類
	    occurrence_date,
        expected_construction_date,
        completion_date,
        total_budget_cost,
        expenditure,
        loss_absence,
        [dbo].[get_v_structure_item](hs.maintenance_season_structure_id, @w_key3, @w_key2) AS maintenance_season_name,          -- 時期
        (SELECT TOP 1 display_name FROM ms_user WHERE user_id = construction_personnel_id) AS construction_personnel_name,      -- 担当者
    	-- hs.total_working_time,           -- 作業時間
        FORMAT(hs.total_working_time, '0.##') AS total_working_time,  --作業時間(Hr)(表示用)
        -- working_time_self,
        FORMAT(working_time_self, '0.##') AS working_time_self,  --自係(Hr)(表示用)
        hs.construction_company,
        working_time_company,
	    sm.plan_implementation_content,  -- 作業内容結果
	   	subject_note,
        maintenance_count,
        [dbo].[get_v_structure_item](discovery_methods_structure_id, @w_key3, @w_key2) AS discovery_methods_name,    -- 発見方法
        [dbo].[get_v_structure_item](actual_result_structure_id, @w_key3, @w_key2) AS actual_result_name,            -- 実績結果
        [dbo].[get_v_structure_item](budget_management_structure_id, @w_key3, @w_key2) AS budget_management_name,    -- 予算管理区分
		[dbo].[get_v_structure_item](budget_personality_structure_id, @w_key3, @w_key2) AS budget_personality_name,  -- 予算性格区分
		machine_no,
        machine_name,
        [dbo].[get_v_structure_item](mc.equipment_level_structure_id, @w_key3, @w_key2) AS equipment_level,          -- 機器レベル
        installation_location,
        number_of_installation,
        FORMAT(date_of_installation, 'yyyy/MM') AS date_of_installation,
        [dbo].[get_v_structure_item](importance_structure_id, @w_key3, @w_key2) AS importance_name,            -- 重要度
        [dbo].[get_v_structure_item](conservation_structure_id, @w_key3, @w_key2) AS conservation_name,        -- 保全方式
        [dbo].[get_applicable_laws](mc.machine_id, 1, @w_key3, @w_key2) AS applicable_laws_name1,   -- 適用法規１
        [dbo].[get_applicable_laws](mc.machine_id, 2, @w_key3, @w_key2) AS applicable_laws_name2,   -- 適用法規２
        [dbo].[get_applicable_laws](mc.machine_id, 3, @w_key3, @w_key2) AS applicable_laws_name3,   -- 適用法規３
        [dbo].[get_applicable_laws](mc.machine_id, 4, @w_key3, @w_key2) AS applicable_laws_name4,   -- 適用法規４
        [dbo].[get_applicable_laws](mc.machine_id, 5, @w_key3, @w_key2) AS applicable_laws_name5,   -- 適用法規５
        mc.machine_note,                                   -- 機番メモ
        [dbo].[get_v_structure_item](use_segment_structure_id, @w_key3, @w_key2) AS use_segment_name,        -- 使用区分
        --(CASE WHEN ISNULL(circulation_target_flg, 0) = 0 THEN '非対象' ELSE '対象' END) AS circulation_target,                                           -- 循環対象
        (
            CASE WHEN ISNULL(circulation_target_flg, 0) = 0 THEN 
                [dbo].[get_rep_translation_text](@w_key3, @TranslationIdCirculationTargetFalse, @w_key2)
            ELSE 
                [dbo].[get_rep_translation_text](@w_key3, @TranslationIdCirculationTargetTrue, @w_key2)
            END
        ) AS circulation_target,                                           -- 循環対象
        fixed_asset_no,                  -- 固定資産番号
		[dbo].[get_v_structure_item](manufacturer_structure_id, @w_key3, @w_key2) AS manufacturer_name,      -- メーカー
		manufacturer_type,               -- メーカー型式
        model_no,                        -- 型式コード
        serial_no,                       -- 製造番号
        FORMAT(date_of_manufacture, 'yyyy/MM') AS date_of_manufacture,                                     -- 製造年月
        equipment_note,                  -- 機器メモ
        [dbo].[get_v_structure_item](inspection_content_structure_id, @w_key3, @w_key2) AS work_item_name, -- 作業項目
        '' AS treatment_measure_note,         -- 作業内容・結果
        '' as failure_cause_addition_note,    -- 故障原因
        '' as failure_cause_personality_note, -- 原因性格
	    '' as treatment_measure_name,         -- 処置対策
        CASE ISNULL(follow_flg, 0)
            WHEN 1 THEN '○'
            ELSE ''
        END AS follow_flg,                                                    -- フォロー要否
        FORMAT(follow_completion_date, 'yyyy/MM') AS follow_completion_date,  -- フォロー年月
        follow_content,                                                       -- フォロー内容
	    [dbo].[get_v_structure_item](his.inspection_site_structure_id, @w_key3, @w_key2) AS maintenance_site ,           -- 作業部位
        [dbo].[get_v_structure_item](hic.inspection_content_structure_id, @w_key3, @w_key2) AS inspection_content_name,  -- 作業項目(保全項目)
        '' AS maintenance_content,        -- 作業項目(保全履歴内容)
        '' AS factory_name2,              -- 工場名(機器)
        '' AS plant_name2,                -- プラント名(機器)
        '' AS series_name2,               -- 系列名(機器)
        '' AS stroke_name2,               -- 工程名(機器)
        '' AS facility_name2,             -- 設備名(機器)
        '' AS job_name2,                  -- 職種名(機器)
        '' AS large_classfication_name2,  -- 大分類(機器)
        '' AS middle_classfication_name2, -- 中分類(機器)
        '' AS small_classfication_name2   -- 小分類(機器)
    FROM
        ma_summary sm
        LEFT JOIN ma_plan mp
	    ON mp.summary_id = sm.summary_id
	    LEFT JOIN ma_request req
	    ON req.summary_id = sm.summary_id,
        ma_history hs,
	    ma_history_machine hsm
	    LEFT JOIN mc_machine mc
	    ON hsm.machine_id = mc.machine_id
	    LEFT JOIN mc_equipment eq
	    ON mc.machine_id = eq.machine_id,
	    ma_history_inspection_site his,
	    ma_history_inspection_content hic
        WHERE
            sm.summary_id = hs.summary_id
        and hs.history_id = hsm.history_id
        and hsm.history_machine_id = his.history_machine_id
        and his.history_inspection_site_id = hic.history_inspection_site_id
        and sm.activity_division = 1 -- 点検
        and hsm.machine_id = @w_key1

        UNION ALL

        SELECT 
            sm.job_structure_id,
	        sm.location_structure_id,
            mc.job_structure_id AS job_structure_id2,
            mc.location_structure_id AS location_structure_id2,
            sm.subject,    -- 件名
	        [dbo].[get_v_structure_item](phenomenon_structure_id, @w_key3, @w_key2) AS phenomenon_name,             -- 現象
	        -- CASE WHEN ISNULL(call_count, 0) >= 1 THEN '有り' ELSE 'なし' END AS call_count_name,
	        -- CASE WHEN ISNULL(stop_count, 0) >= 1 THEN '有り' ELSE 'なし' END AS stop_count_name, 
	        CASE WHEN ISNULL(call_count, 0) >= 1 THEN 
                [dbo].[get_rep_translation_text](@w_key3, @TranslationIdExists, @w_key2)
            ELSE 
                [dbo].[get_rep_translation_text](@w_key3, @TranslationIdNotExists, @w_key2)
            END AS call_count_name,
	        CASE WHEN ISNULL(stop_count, 0) >= 1 THEN 
                [dbo].[get_rep_translation_text](@w_key3, @TranslationIdExists, @w_key2)
            ELSE 
                [dbo].[get_rep_translation_text](@w_key3, @TranslationIdNotExists, @w_key2)
            END AS stop_count_name, 
	        stop_time,
	        '' AS work_purpose_name,                    -- 目的区分
	        [dbo].[get_v_structure_item](sudden_division_structure_id, @w_key3, @w_key2) AS sudden_division_name,   -- 突発区分
	        [dbo].[get_v_structure_item](mq_class_structure_id, @w_key3, @w_key2) AS mq_class_name,                 -- MQ分類
	        occurrence_date,
            expected_construction_date,
            completion_date,
            total_budget_cost,
            expenditure,
            loss_absence,
            [dbo].[get_v_structure_item](hs.maintenance_season_structure_id, @w_key3, @w_key2) AS maintenance_season_name,      -- 時期
            (SELECT TOP 1 display_name FROM ms_user WHERE user_id = construction_personnel_id) AS construction_personnel_name,  -- 担当者
    	    -- hs.total_working_time,           -- 作業時間
            FORMAT(hs.total_working_time, '0.##') AS total_working_time,  --作業時間(Hr)(表示用)
            -- working_time_self,
            FORMAT(working_time_self, '0.##') AS working_time_self,  --自係(Hr)(表示用)
            hs.construction_company,
            working_time_company,
	        sm.plan_implementation_content, -- 作業内容結果
	   	    subject_note,
            maintenance_count,
            [dbo].[get_v_structure_item](discovery_methods_structure_id, @w_key3, @w_key2) AS discovery_methods_name,    -- 発見方法
            [dbo].[get_v_structure_item](actual_result_structure_id, @w_key3, @w_key2) AS actual_result_name,            -- 実績結果
            [dbo].[get_v_structure_item](budget_management_structure_id, @w_key3, @w_key2) AS budget_management_name,    -- 予算管理区分
			[dbo].[get_v_structure_item](budget_personality_structure_id, @w_key3, @w_key2) AS budget_personality_name,  -- 予算性格区分
			machine_no,
            machine_name,
            [dbo].[get_v_structure_item](mc.equipment_level_structure_id, @w_key3, @w_key2) AS equipment_level,          -- 機器レベル
	        installation_location,
            number_of_installation,
            FORMAT(date_of_installation, 'yyyy/MM') AS date_of_installation,
            [dbo].[get_v_structure_item](importance_structure_id, @w_key3, @w_key2) AS importance_name,            -- 重要度
            [dbo].[get_v_structure_item](conservation_structure_id, @w_key3, @w_key2) AS conservation_name,        -- 保全方式
            [dbo].[get_applicable_laws](mc.machine_id, 1, @w_key3, @w_key2) AS applicable_laws_name1,   -- 適用法規１
            [dbo].[get_applicable_laws](mc.machine_id, 2, @w_key3, @w_key2) AS applicable_laws_name2,   -- 適用法規２
            [dbo].[get_applicable_laws](mc.machine_id, 3, @w_key3, @w_key2) AS applicable_laws_name3,   -- 適用法規３
            [dbo].[get_applicable_laws](mc.machine_id, 4, @w_key3, @w_key2) AS applicable_laws_name4,   -- 適用法規４
            [dbo].[get_applicable_laws](mc.machine_id, 5, @w_key3, @w_key2) AS applicable_laws_name5,   -- 適用法規５
            mc.machine_note,                                   -- 機番メモ
            [dbo].[get_v_structure_item](use_segment_structure_id, @w_key3, @w_key2) AS use_segment_name,        -- 使用区分
			-- (CASE WHEN ISNULL(circulation_target_flg, 0) = 0 THEN '非対象' ELSE '対象' END) AS circulation_target,                                           -- 循環対象
            (
                CASE WHEN ISNULL(circulation_target_flg, 0) = 0 THEN 
                    [dbo].[get_rep_translation_text](@w_key3, @TranslationIdCirculationTargetFalse, @w_key2)
                ELSE 
                    [dbo].[get_rep_translation_text](@w_key3, @TranslationIdCirculationTargetTrue, @w_key2)
                END
            ) AS circulation_target,                                           -- 循環対象
            fixed_asset_no,                  -- 固定資産番号
			[dbo].[get_v_structure_item](manufacturer_structure_id, @w_key3, @w_key2) AS manufacturer_name,      -- メーカー
			manufacturer_type,               -- メーカー型式
            model_no,                        -- 型式コード
            serial_no,                       -- 製造番号
            FORMAT(date_of_manufacture, 'yyyy/MM') AS date_of_manufacture,                                     -- 製造年月
            equipment_note,                  -- 機器メモ
            [dbo].[get_v_structure_item](inspection_content_structure_id, @w_key3, @w_key2) AS work_item_name, -- 作業項目
            treatment_measure_note,　　　-- 作業内容・結果
	        [dbo].[get_v_structure_item](hf.failure_cause_structure_id, @w_key3, @w_key2) AS failure_cause_addition_note,                 -- 故障原因
            [dbo].[get_v_structure_item](hf.failure_cause_personality_structure_id, @w_key3, @w_key2) AS failure_cause_personality_note,  -- 原因性格
	        [dbo].[get_v_structure_item](hf.treatment_measure_structure_id, @w_key3, @w_key2) AS treatment_measure_name,                  -- 処置・対策
            CASE ISNULL(hf.follow_flg, 0)
                WHEN 1 THEN '○'
                ELSE ''
            END AS follow_flg,                                                       -- フォロー要否
            FORMAT(hf.follow_completion_date, 'yyyy/MM') AS follow_completion_date,  -- フォロー年月
            hf.follow_content,                                                       -- フォロー内容
            hf.maintenance_site,
	        [dbo].[get_v_structure_item](hic.inspection_content_structure_id, @w_key3, @w_key2) AS inspection_content_name, -- 作業項目(保全項目)
	        hf.maintenance_content,           -- 作業項目(保全履歴内容)
            '' AS factory_name2,              -- 工場名(機器)
            '' AS plant_name2,                -- プラント名(機器)
            '' AS series_name2,               -- 系列名(機器)
            '' AS stroke_name2,               -- 工程名(機器)
            '' AS facility_name2,             -- 設備名(機器)
            '' AS job_name2,                  -- 職種名(機器)
            '' AS large_classfication_name2,  -- 大分類(機器)
            '' AS middle_classfication_name2, -- 中分類(機器)
            '' AS small_classfication_name2   -- 小分類(機器)
        FROM
            ma_summary sm
        LEFT JOIN ma_plan mp
	    ON mp.summary_id = sm.summary_id
	    LEFT JOIN ma_request req
	    ON req.summary_id = sm.summary_id,
        ma_history hs
	    LEFT JOIN (select min(history_machine_id) AS history_machine_id, history_id, machine_id from ma_history_machine group by history_id, machine_id) hsm
	    ON hs.history_id = hsm.history_id
	    LEFT JOIN ma_history_inspection_site his
	    ON hsm.history_machine_id = his.history_machine_id
	    LEFT JOIN ma_history_inspection_content hic
	    ON his.history_inspection_site_id = hic.history_inspection_site_id
	    LEFT JOIN mc_machine mc
	    ON hsm.machine_id = mc.machine_id
	    LEFT JOIN mc_equipment eq
	    ON mc.machine_id = eq.machine_id,
	    ma_history_failure hf
        WHERE
            sm.summary_id = hs.summary_id
        and hs.history_id = hf.history_id
        and sm.activity_division = 2 -- 故障
        and hf.machine_id = @w_key1
    )tbl
    order by ISNULL(completion_date,'9999/12/31') desc

	-- 次のレコードへ
	FETCH NEXT FROM cur_key INTO @w_key1, @w_key2, @w_key3;
END

-- カーソルクローズ
CLOSE cur_key;
DEALLOCATE cur_key;

-- 帳票データを返却
SELECT * FROM #temp_rep ORDER BY idx, seq

-- 一時テーブル削除
DROP TABLE #temp_rep

