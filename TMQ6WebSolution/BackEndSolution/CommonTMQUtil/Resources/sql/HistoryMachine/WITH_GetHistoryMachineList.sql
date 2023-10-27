WITH structure_factory AS( -- 翻訳の取得に使用
    SELECT
        structure_id,
        location_structure_id AS factory_id
    FROM
        v_structure_item_all
    WHERE
        structure_group_id IN(1030, 1150, 1170, 1200, 1210, 2090, 2100)
    AND language_id = @LanguageId
),
law AS -- 機番IDに紐付く適用法規(トランザクションテーブル)
(
    SELECT
        tbl.machine_id,
        tbl.applicable_laws_structure_id
    FROM
        (
            SELECT
                machine.machine_id,
                trim(
                    '|'
                    FROM
                        (
                            SELECT
                                cast(law.applicable_laws_structure_id AS varchar) + '|'
                            FROM
                                mc_applicable_laws law
                            WHERE
                                machine.machine_id = law.machine_id FOR XML PATH('')
                        )
                ) AS applicable_laws_structure_id
            FROM
                mc_machine machine
            GROUP BY
                machine.machine_id
        ) AS tbl
    WHERE
        tbl.applicable_laws_structure_id IS NOT NULL
),
hm_law AS -- 変更管理IDに紐付く適用法規(変更管理テーブル)
(
    SELECT
        tbl.history_management_id,
        tbl.applicable_laws_structure_id
    FROM
        (
            SELECT
                history.history_management_id,
                trim(
                    '|'
                    FROM
                        (
                            SELECT
                                cast(law.applicable_laws_structure_id AS varchar) + '|'
                            FROM
                                hm_mc_applicable_laws law
                            WHERE
                                history.history_management_id = law.history_management_id FOR XML PATH('')
                        )
                ) AS applicable_laws_structure_id
            FROM
                hm_history_management history
            GROUP BY
                history.history_management_id
        ) AS tbl
    WHERE
        tbl.applicable_laws_structure_id IS NOT NULL
),
target AS(
    SELECT
        hmachine.machine_no,
        hmachine.machine_name,
        hmachine.equipment_level_structure_id,
        hmachine.location_structure_id,
        hmachine.importance_structure_id,
        hmachine.conservation_structure_id,
        hmachine.installation_location,
        hmachine.number_of_installation,
        hmachine.date_of_installation,
        hmachine.machine_note,
        hm_law.applicable_laws_structure_id,
        hmachine.job_structure_id,
        hequipment.manufacturer_structure_id,
        hequipment.manufacturer_type,
        hequipment.model_no,
        hequipment.serial_no,
        hequipment.date_of_manufacture,
        hequipment.delivery_date,
        hequipment.use_segment_structure_id,
        dbo.get_file_download_info(1610, hequipment.equipment_id) AS file_link_equipment,
        dbo.get_file_download_info(1600, hmachine.machine_id) AS file_link_machine,
        hequipment.fixed_asset_no,
        hequipment.equipment_note,
        CASE
            WHEN hcomponent.hm_management_standards_component_id IS NOT NULL THEN 1
            ELSE 0
        END AS is_changed_component,
        history.application_status_id,
        history.application_division_id,
        history.application_conduct_id,
        history.application_user_name,
        history.approval_user_name,
        history.application_date,
        history.approval_date,
        dbo.get_target_layer_id(hmachine.location_structure_id, 1) AS factory_id,
        ---------- 以下は値の変更があった項目(申請区分が「変更申請：20」のデータ)を取得 ----------
        CASE
            WHEN division_ex.extension_data = '20' THEN trim(
                '|'
                FROM
                    (
                        CASE
                            WHEN hmachine.machine_no <> machine.machine_no THEN 'MachineNo|'                                          -- 機器番号
                            ELSE ''
                        END + CASE
                            WHEN hmachine.machine_name <> machine.machine_name THEN 'MachineName|'                                    -- 機器名称
                            ELSE ''
                        END + CASE
                            WHEN hmachine.equipment_level_structure_id <> machine.equipment_level_structure_id THEN 'EquipmentLevel|' -- 機器レベル
                            ELSE ''
                        END + CASE
                            WHEN hmachine.location_structure_id <> machine.location_structure_id THEN 'Location|'                     -- 場所階層
                            ELSE ''
                        END + CASE
                            WHEN hmachine.importance_structure_id <> machine.importance_structure_id THEN 'Importance|'               -- 重要度
                            ELSE ''
                        END + CASE
                            WHEN hmachine.conservation_structure_id <> machine.conservation_structure_id THEN 'Conservation|'         -- 保全方式
                            ELSE ''
                        END + CASE
                            WHEN hmachine.installation_location <> machine.installation_location THEN 'InstallationLocation|'         -- 設置場所
                            ELSE ''
                        END + CASE
                            WHEN hmachine.number_of_installation <> machine.number_of_installation THEN 'NumberOfInstallation|'       -- 設置台数
                            ELSE ''
                        END + CASE
                            WHEN hmachine.date_of_installation <> machine.date_of_installation THEN 'DateOfInstallation|'             -- 設置年月
                            ELSE ''
                        END + CASE
                            WHEN hm_law.applicable_laws_structure_id <> law.applicable_laws_structure_id THEN 'ApplicableLaws|'       -- 適用法規
                            ELSE ''
                        END + CASE
                            WHEN hmachine.machine_note <> machine.machine_note THEN 'MachineNote|'                                    -- 機番メモ
                            ELSE ''
                        END + CASE
                            WHEN hmachine.job_structure_id <> machine.job_structure_id THEN 'Job|'                                    -- 職種機種
                            ELSE ''
                        END + CASE
                            WHEN hequipment.manufacturer_structure_id <> equipment.manufacturer_structure_id THEN 'Manufacturer|'     -- メーカー
                            ELSE ''
                        END + CASE
                            WHEN hequipment.manufacturer_type <> equipment.manufacturer_type THEN 'ManufacturerType|'                 -- メーカー型式
                            ELSE ''
                        END + CASE
                            WHEN hequipment.model_no <> equipment.model_no THEN 'ModelNo|'                                            -- 製造番号
                            ELSE ''
                        END + CASE
                            WHEN hequipment.date_of_manufacture <> equipment.date_of_manufacture THEN 'DateOfManufacture|'            -- 製造年月
                            ELSE ''
                        END + CASE
                            WHEN hequipment.delivery_date <> equipment.delivery_date THEN 'DeliveryDate|'                             -- 納期
                            ELSE ''
                        END + CASE
                            WHEN hequipment.use_segment_structure_id <> equipment.use_segment_structure_id THEN 'UseSegment|'         -- 使用区分
                            ELSE ''
                        END + CASE
                            WHEN hequipment.fixed_asset_no <> equipment.fixed_asset_no THEN 'FixedAssetNo|'                           -- 固定資産番号
                            ELSE ''
                        END + CASE
                            WHEN hequipment.equipment_note <> equipment.equipment_note THEN 'EquipmentNote|'                          -- 機器メモ
                            ELSE ''
                        END
                    )
            )
            ELSE ''
        END AS value_changed
    FROM
        hm_history_management history -- 変更管理
        LEFT JOIN
            hm_mc_machine hmachine -- 機番情報変更管理
        ON  history.history_management_id = hmachine.history_management_id
        LEFT JOIN
            hm_mc_equipment hequipment -- 機器情報変更管理
        ON  history.history_management_id = hequipment.history_management_id
        LEFT JOIN
            hm_mc_management_standards_component hcomponent -- 機器別管理基準部位変更管理
        ON  history.history_management_id = hcomponent.history_management_id
        LEFT JOIN
            ms_structure status_ms -- 構成マスタ(申請状況)
        ON  history.application_status_id = status_ms.structure_id
        LEFT JOIN
            ms_item_extension status_ex -- アイテムマスタ拡張(申請状況)
        ON  status_ms.structure_item_id = status_ex.item_id
        AND status_ex.sequence_no = 1
        LEFT JOIN
            mc_machine machine -- 機番情報(トランザクション)
        ON  hmachine.hm_machine_id = machine.machine_id
        LEFT JOIN
            mc_equipment equipment -- 機器情報(トランザクション)
        ON  machine.machine_id = equipment.equipment_id
        LEFT JOIN
            hm_law -- 適用法規(変更管理)
        ON  history.history_management_id = hm_law.history_management_id
        LEFT JOIN
            law -- 適用法規(トランザクション)
        ON  machine.machine_id = law.machine_id
        LEFT JOIN
            ms_structure division_ms --構成マスタ(申請区分)
        ON  history.application_division_id = division_ms.structure_id
        LEFT JOIN
            ms_item_extension division_ex -- アイテムマスタ拡張(申請区分)
        ON  division_ms.structure_item_id = division_ex.item_id
        AND division_ex.sequence_no = 1
    WHERE
        -- 「申請データ作成中」「承認依頼中」「差戻中」のデータのみ
        status_ex.extension_data IN('10', '20', '30')
        /*@DispOnlyMySubject
        -- 自分の件名のみ表示
        AND (history.application_user_id = @UserId OR history.approval_user_id = @UserId)
        @DispOnlyMySubject*/
)