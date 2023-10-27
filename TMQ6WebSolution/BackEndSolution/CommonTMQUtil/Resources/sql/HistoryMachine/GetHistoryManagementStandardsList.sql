
SELECT
    target.machine_id,                   -- 機番ID
    target.machine_no,                   -- 機器番号
    target.machine_name,                 -- 機器名称
    target.location_structure_id,        -- 場所階層(変更管理テーブル)
    target.installation_location,        -- 設置場所
    target.number_of_installation,       -- 設置台数
    target.date_of_installation,         -- 設置年月
    target.machine_note,                 -- 機番メモ
    target.applicable_laws_structure_id, -- 適用法規
    target.job_structure_id,             -- 職種機種(変更管理テーブル)
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
    'shinsei_conduct' AS conduct_name,   -- 申請機能
    target.application_user_name,        -- 申請者
    target.approval_user_name,           -- 承認者
    target.application_date,             -- 申請日
    target.approval_date,                -- 承認日
    target.application_division_code,    -- 申請区分(拡張項目)
    target.value_changed,                -- 値に変更のあった項目
    target.history_management_id,        -- 変更管理ID
    target.history_management_detail_id, -- 変更管理詳細ID
    target.machine_history_management_detail_id, -- 変更管理詳細ID(機番情報変更管理テーブル)
    target.update_serialid,              -- 更新シリアルID(変更管理テーブル)
    target.old_location_structure_id,    -- 場所階層(トランザクションテーブル)
    target.old_job_structure_id,         -- 職種機種(トランザクションテーブル)
    target.data_type,                    -- 表示しているデータの種類(0:トランザクションデータ,1:変更管理データ)
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
                    structure_factory AS st_f
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
                    structure_factory AS st_f
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
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.conservation_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.conservation_structure_id
    ) AS inspection_site_conservation_name, -- 保全方式
    trim(
        ','
        FROM
            (
                SELECT
                    tra.translation_text + ','
                FROM
                    hm_history_management_detail detail
                    LEFT JOIN
                        hm_mc_applicable_laws laws
                    ON  detail.history_management_detail_id = laws.history_management_detail_id
                    LEFT JOIN
                        v_structure_item_all tra
                    ON  laws.applicable_laws_structure_id = tra.structure_id
                WHERE
                    tra.language_id = @LanguageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            structure_factory AS st_f
                        WHERE
                            st_f.structure_id = laws.applicable_laws_structure_id
                        AND st_f.factory_id IN(0, target.factory_id)
                    )
                AND tra.structure_id = laws.applicable_laws_structure_id
                AND target.history_management_detail_id = laws.history_management_detail_id FOR XML PATH('')
            )
    ) AS applicable_laws_name, -- 適用法規
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
                    structure_factory AS st_f
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
                    structure_factory AS st_f
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
                    structure_factory AS st_f
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
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.application_division_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.application_division_id
    ) AS application_division_name -- 申請区分
FROM
    target