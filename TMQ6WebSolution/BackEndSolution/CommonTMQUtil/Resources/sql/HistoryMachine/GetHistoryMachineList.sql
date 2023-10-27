SELECT
    target.machine_no,                   -- 機器番号
    target.machine_name,                 -- 機器名称
    target.location_structure_id,        -- 場所階層
    target.installation_location,        -- 設置場所
    target.number_of_installation,       -- 設置台数
    target.date_of_installation,         -- 設置年月
    target.machine_note,                 -- 機番メモ
    target.applicable_laws_structure_id, -- 適用法規
    target.job_structure_id,             -- 職種機種
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
    target.value_changed,                -- 値に変更のあった項目
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