
SELECT
    target.machine_id,                   -- 機番ID
    target.equipment_id,                 -- 機器ID
    target.machine_no,                   -- 機器番号
    target.machine_name,                 -- 機器名称
    target.location_structure_id,        -- 場所階層(変更管理テーブル)
    target.location_district_structure_id AS district_id, -- 地区
    target.location_factory_structure_id AS factory_id,   -- 工場
    target.location_plant_structure_id AS plant_id,       -- プラント
    target.location_series_structure_id AS series_id,     -- 系列
    target.location_stroke_structure_id AS stroke_id,     -- 工程
    target.location_facility_structure_id AS facility_id, --設備
    target.installation_location,        -- 設置場所
    target.number_of_installation,       -- 設置台数
    target.date_of_installation,         -- 設置年月
    target.machine_note,                 -- 機番メモ
    target.applicable_laws_structure_id, -- 適用法規
    target.job_structure_id,             -- 職種機種(変更管理テーブル)
    target.job_kind_structure_id AS job_id,                                  -- 職種
    target.job_large_classfication_structure_id AS large_classfication_id,   -- 機種大分類
    target.job_middle_classfication_structure_id AS middle_classfication_id, -- 機種中分類
    target.job_small_classfication_structure_id AS small_classfication_id,   -- 機種小分類
    target.manufacturer_type,            -- メーカー型式
    target.model_no,                     -- 型式コード
    target.serial_no,                    -- 製造番号
    target.date_of_manufacture,          -- 製造年月
    target.delivery_date,                -- 納期
    target.file_link_equipment,          -- 機器添付有無
    target.file_link_machine,            -- 機番添付有無
    target.fixed_asset_no,               -- 固定資産番号
    target.equipment_note,               -- 機器メモ
    target.is_changed_component,         -- 機器別管理基準変更有無
    target.application_user_name,        -- 申請者
    target.approval_user_name,           -- 承認者
    target.application_date,             -- 申請日
    target.approval_date,                -- 承認日
    target.application_division_code,    -- 申請区分(拡張項目)
    target.application_status_code,      -- 申請状況(拡張項目)
    target.value_changed,                -- 値に変更のあった項目
    target.history_management_id,        -- 変更管理ID
    target.update_serialid,              -- 更新シリアルID(変更管理テーブル)
    target.old_location_structure_id,    -- 場所階層(トランザクションテーブル)
    target.old_job_structure_id,         -- 職種機種(トランザクションテーブル)
    target.application_status_id,        -- 申請状況
    target.application_division_id,      -- 申請区分

    ---------- 以下は詳細画面で使用 ----------
    target.hm_machine_id,                -- 機番情報変更管理ID
    target.hm_equipment_id,              -- 機器情報変更管理ID
    target.equipment_level_structure_id, -- 機器レベル
    target.importance_structure_id,      -- 重要度
    target.conservation_structure_id AS inspection_site_conservation_structure_id, -- 保全方式
    target.use_segment_structure_id,     -- 使用区分
    target.circulation_target_flg,       -- 循環対象
    target.manufacturer_structure_id,    -- メーカー
    target.maintainance_kind_manage,     -- 点検種別毎管理
    target.application_reason,           -- 申請理由
    target.rejection_reason,             -- 否認理由
    target.mc_update_serial_id,          -- 更新シリアルID(機番情報)
    target.eq_update_serial_id,          -- 更新シリアルID(機器情報)

    ---------- 以下は翻訳を取得 ----------
    (
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
                    st_f.structure_id = target.equipment_level_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.equipment_level_structure_id
    ) AS equipment_level, -- 機器レベル
    (
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
                    st_f.structure_id = target.importance_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.importance_structure_id
    ) AS importance_name, -- 重要度
    (
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
                    st_f.structure_id = target.conservation_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.conservation_structure_id
    ) AS inspection_site_conservation_name, -- 保全方式
    CASE WHEN target.data_type = 0 THEN
    trim(
        ','
        FROM
            (
                SELECT
                    tra.translation_text + ','
                FROM
                    mc_applicable_laws laws
                    LEFT JOIN
                        v_structure_item_all tra
                    ON  laws.applicable_laws_structure_id = tra.structure_id
                WHERE
                    tra.language_id = @LanguageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = laws.applicable_laws_structure_id
                        AND st_f.factory_id IN(0, target.factory_id)
                    )
                AND tra.structure_id = laws.applicable_laws_structure_id
                AND target.machine_id = laws.machine_id FOR XML PATH('')
            )
    ) 
    ELSE
    trim(
        ','
        FROM
            (
                SELECT
                    tra.translation_text + ','
                FROM
                    hm_history_management history
                    LEFT JOIN
                        hm_mc_applicable_laws laws
                    ON  history.history_management_id = laws.history_management_id
                    LEFT JOIN
                        v_structure_item_all tra
                    ON  laws.applicable_laws_structure_id = tra.structure_id
                WHERE
                    tra.language_id = @LanguageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = laws.applicable_laws_structure_id
                        AND st_f.factory_id IN(0, target.factory_id)
                    )
                AND tra.structure_id = laws.applicable_laws_structure_id
                AND target.history_management_id = laws.history_management_id FOR XML PATH('')
            )
    ) 
    END AS applicable_laws_name, -- 適用法規 表示しているデータの種類(0:変更管理のデータがない,1:変更管理のデータがある)
    (
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
                    st_f.structure_id = target.manufacturer_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.manufacturer_structure_id
    ) AS manufacturer_name, -- メーカー
    (
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
                    st_f.structure_id = target.use_segment_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.use_segment_structure_id
    ) AS use_segment_name, -- 使用区分
    (
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
                    st_f.structure_id = target.application_status_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.application_status_id
    ) AS application_status_name, -- 申請状況
    (
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
                    st_f.structure_id = target.application_division_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.application_division_id
    ) AS application_division_name, -- 申請区分



    (
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
                    st_f.structure_id = target.location_district_structure_id
                AND st_f.factory_id IN(0, target.location_factory_structure_id)
            )
        AND tra.structure_id = target.location_district_structure_id
    ) AS district_name, -- 地区
    (
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
                    st_f.structure_id = target.location_factory_structure_id
                AND st_f.factory_id IN(0, target.location_factory_structure_id)
            )
        AND tra.structure_id = target.location_factory_structure_id
    ) AS factory_name, -- 工場
    (
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
                    st_f.structure_id = target.location_plant_structure_id
                AND st_f.factory_id IN(0, target.location_factory_structure_id)
            )
        AND tra.structure_id = target.location_plant_structure_id
    ) AS plant_name, -- プラント
    (
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
                    st_f.structure_id = target.location_series_structure_id
                AND st_f.factory_id IN(0, target.location_factory_structure_id)
            )
        AND tra.structure_id = target.location_series_structure_id
    ) AS series_name, -- 系列
    (
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
                    st_f.structure_id = target.location_stroke_structure_id
                AND st_f.factory_id IN(0, target.location_factory_structure_id)
            )
        AND tra.structure_id = target.location_stroke_structure_id
    ) AS stroke_name, -- 工程
        (
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
                    st_f.structure_id = target.location_facility_structure_id
                AND st_f.factory_id IN(0, target.location_factory_structure_id)
            )
        AND tra.structure_id = target.location_facility_structure_id
    ) AS facility_name, -- 設備
        (
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
                    st_f.structure_id = target.job_kind_structure_id
                AND st_f.factory_id IN(0, target.location_factory_structure_id)
            )
        AND tra.structure_id = target.job_kind_structure_id
    ) AS job_name, -- 職種
        (
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
                    st_f.structure_id = target.job_large_classfication_structure_id
                AND st_f.factory_id IN(0, target.location_factory_structure_id)
            )
        AND tra.structure_id = target.job_large_classfication_structure_id
    ) AS large_classfication_name, -- 機種大分類
        (
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
                    st_f.structure_id = target.job_middle_classfication_structure_id
                AND st_f.factory_id IN(0, target.location_factory_structure_id)
            )
        AND tra.structure_id = target.job_middle_classfication_structure_id
    ) AS middle_classfication_name, -- 機種中分類
        (
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
                    st_f.structure_id = target.job_small_classfication_structure_id
                AND st_f.factory_id IN(0, target.location_factory_structure_id)
            )
        AND tra.structure_id = target.job_small_classfication_structure_id
    ) AS small_classfication_name -- 機種小分類
FROM
    target