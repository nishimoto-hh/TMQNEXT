WITH law AS -- 機番IDに紐付く適用法規(トランザクションテーブル)
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
        tbl.history_management_detail_id,
        tbl.applicable_laws_structure_id
    FROM
        (
            SELECT
                detail.history_management_detail_id,
                trim(
                    '|'
                    FROM
                        (
                            SELECT
                                cast(laws.applicable_laws_structure_id AS varchar) + '|'
                            FROM
                                hm_mc_applicable_laws laws
                            WHERE
                                detail.history_management_detail_id = laws.history_management_detail_id FOR XML PATH('')
                        )
                 ) AS applicable_laws_structure_id
            FROM
                hm_history_management_detail detail
            GROUP BY
                detail.history_management_detail_id
        ) AS tbl
    WHERE
        tbl.applicable_laws_structure_id IS NOT NULL
)
SELECT
    hmachine.history_management_detail_id AS machine_history_management_detail_id,   -- 変更管理詳細ID
    hmachine.location_structure_id,                                                  -- 場所階層
    machine.location_structure_id AS old_location_structure_id,                      -- 場所階層(トランザクションテーブル)
    hmachine.machine_no,                                                             -- 機器番号
    hmachine.machine_name,                                                           -- 機器名称
    hmachine.equipment_level_structure_id,                                           -- 機器レベル
    hmachine.installation_location,                                                  -- 設置場所
    hmachine.number_of_installation,                                                 -- 設置台数
    hmachine.date_of_installation,                                                   -- 設置年月
    hmachine.importance_structure_id,                                                -- 重要度
    hmachine.conservation_structure_id AS inspection_site_conservation_structure_id, -- 保全方式
    hm_law.applicable_laws_structure_id,                                             -- 適用法規
    hmachine.machine_note,                                                           -- 機番メモ
    hmachine.job_structure_id,                                                       -- 職種機種
    machine.job_structure_id AS old_job_structure_id,                                -- 職種機種(トランザクションテーブル)
    hequipment.use_segment_structure_id,                                             -- 使用区分
    hequipment.circulation_target_flg,                                               -- 循環対象
    hequipment.fixed_asset_no,                                                       -- 固定資産番号
    hequipment.manufacturer_structure_id,                                            -- メーカー
    hequipment.manufacturer_type,                                                    -- メーカー型式
    hequipment.model_no,                                                             -- 型式コード
    hequipment.serial_no,                                                            -- 製造番号
    hequipment.date_of_manufacture,                                                  -- 製造年月
    hequipment.delivery_date,                                                        -- 納期
    hequipment.maintainance_kind_manage,                                             -- 点検種別毎管理
    hequipment.equipment_note,                                                       -- 機器メモ
    history.application_reason,                                                      -- 申請理由
    history.rejection_reason,                                                        -- 否認理由
    division_ex.extension_data AS application_division_code,                         -- 申請区分(拡張項目)
    ---------- 以下は値の変更があった項目を取得 ----------
    CASE
        WHEN division_ex.extension_data = '20' THEN trim(
            '|'
            FROM
                (
                    CASE
                        WHEN(hmachine.machine_no IS NOT NULL AND hmachine.machine_no <> '') AND ( machine.machine_no IS NULL OR machine.machine_no = '' ) THEN 'MachineNo_10|' -- 値が追加された場合
                        WHEN(hmachine.machine_no IS NULL OR hmachine.machine_no = '' ) AND ( machine.machine_no IS NOT NULL AND machine.machine_no <> '' ) THEN 'MachineNo_30|' -- 値が削除された場合
                        WHEN hmachine.machine_no <> machine.machine_no THEN 'MachineNo_20|' -- 値が変更された場合
                        ELSE '' -- 機器番号
                    END + CASE
                        WHEN(hmachine.machine_name IS NOT NULL AND hmachine.machine_name <> '' ) AND ( machine.machine_name IS NULL OR machine.machine_name = '' ) THEN 'MachineName_10|' -- 値が追加された場合
                        WHEN(hmachine.machine_name IS NULL OR hmachine.machine_name = '' ) AND ( machine.machine_name IS NOT NULL AND machine.machine_name <> '' ) THEN 'MachineName_30|' -- 値が削除された場合
                        WHEN hmachine.machine_name <> machine.machine_name THEN 'MachineName_20|' -- 値が変更された場合
                        ELSE '' -- 機器名称
                    END + CASE
                       WHEN hmachine.equipment_level_structure_id IS NOT NULL AND machine.equipment_level_structure_id IS NULL THEN 'EquipmentLevel_10|' -- 値が追加された場合
                       WHEN hmachine.equipment_level_structure_id IS NULL AND machine.equipment_level_structure_id IS NOT NULL THEN 'EquipmentLevel_30|' -- 値が削除された場合
                       WHEN hmachine.equipment_level_structure_id <> machine.equipment_level_structure_id THEN 'EquipmentLevel_20|' -- 値が変更された場合
                       ELSE '' -- 機器レベル
                    END + CASE
                       WHEN hmachine.importance_structure_id IS NOT NULL AND ( machine.importance_structure_id IS NULL OR machine.importance_structure_id = '' ) THEN 'Importance_10|' -- 値が追加された場合
                       WHEN hmachine.importance_structure_id IS NULL AND machine.importance_structure_id IS NOT NULL THEN 'Importance_30|' -- 値が削除された場合
                       WHEN hmachine.importance_structure_id <> machine.importance_structure_id THEN 'Importance_20|' -- 値が変更された場合
                       ELSE '' -- 重要度
                    END + CASE
                       WHEN hmachine.conservation_structure_id IS NOT NULL AND machine.conservation_structure_id IS NULL THEN 'Conservation_10|' -- 値が追加された場合
                       WHEN hmachine.conservation_structure_id IS NULL AND machine.conservation_structure_id IS NOT NULL THEN 'Conservation_30|' -- 値が削除された場合
                       WHEN hmachine.conservation_structure_id <> machine.conservation_structure_id THEN 'Conservation_20|' -- 値が変更された場合
                       ELSE '' -- 保全方式
                    END + CASE
                       WHEN(hmachine.installation_location IS NOT NULL AND hmachine.installation_location <> '' ) AND ( machine.installation_location IS NULL OR machine.installation_location = '' ) THEN 'InstallationLocation_10|' -- 値が追加された場合
                       WHEN(hmachine.installation_location IS NULL OR hmachine.installation_location = '' ) AND ( machine.installation_location IS NOT NULL AND machine.installation_location <> '' ) THEN 'InstallationLocation_30|' -- 値が削除された場合
                       WHEN hmachine.installation_location <> machine.installation_location THEN 'InstallationLocation_20|' -- 値が変更された場合
                       ELSE '' -- 設置場所
                    END + CASE
                       WHEN hmachine.number_of_installation IS NOT NULL AND machine.number_of_installation IS NULL THEN 'NumberOfInstallation_10|' -- 値が追加された場合
                       WHEN hmachine.number_of_installation IS NULL AND machine.number_of_installation IS NOT NULL THEN 'NumberOfInstallation_30|' -- 値が削除された場合
                       WHEN hmachine.number_of_installation <> machine.number_of_installation THEN 'NumberOfInstallation_20|' -- 値が変更された場合
                       ELSE '' -- 設置台数
                    END + CASE
                       WHEN(hmachine.date_of_installation IS NOT NULL AND hmachine.date_of_installation <> '' ) AND ( machine.date_of_installation IS NULL OR machine.date_of_installation = '' ) THEN 'DateOfInstallation_10|'  -- 値が追加された場合
                       WHEN(hmachine.date_of_installation IS NULL OR hmachine.date_of_installation = '' ) AND ( machine.date_of_installation IS NOT NULL AND machine.date_of_installation <> '' ) THEN 'DateOfInstallation_30|' -- 値が削除された場合
                       WHEN hmachine.date_of_installation <> machine.date_of_installation THEN 'DateOfInstallation_20|' -- 値が変更された場合
                       ELSE '' -- 設置日
                    END + CASE
                       WHEN hm_law.applicable_laws_structure_id IS NOT NULL AND law.applicable_laws_structure_id IS NULL THEN 'ApplicableLaws_10|' -- 値が追加された場合
                       WHEN hm_law.applicable_laws_structure_id IS NULL AND law.applicable_laws_structure_id IS NOT NULL THEN 'ApplicableLaws_30|' -- 値が削除された場合
                       WHEN hm_law.applicable_laws_structure_id <> law.applicable_laws_structure_id THEN 'ApplicableLaws_20|' -- 値が変更された場合
                      ELSE '' -- 適用法規
                   END + CASE
                       WHEN(hmachine.machine_note IS NOT NULL AND hmachine.machine_note <> '' ) AND ( machine.machine_note IS NULL OR machine.machine_note = '' ) THEN 'MachineNote_10|' -- 値が追加された場合
                       WHEN(hmachine.machine_note IS NULL OR hmachine.machine_note = '' ) AND ( machine.machine_note IS NOT NULL AND machine.machine_note <> '' ) THEN 'MachineNote_30|' -- 値が削除された場合
                       WHEN hmachine.machine_note <> machine.machine_note THEN 'MachineNote_20|' -- 値が変更された場合
                       ELSE '' -- 機番メモ
                   END + CASE
                       WHEN hequipment.manufacturer_structure_id IS NOT NULL AND equipment.manufacturer_structure_id IS NULL THEN 'Manufacturer_10|' -- 値が追加された場合
                       WHEN hequipment.manufacturer_structure_id IS NULL AND equipment.manufacturer_structure_id IS NOT NULL THEN 'Manufacturer_30|' -- 値が削除された場合
                       WHEN hequipment.manufacturer_structure_id <> equipment.manufacturer_structure_id THEN 'Manufacturer_20|' -- 値が変更された場合
                       ELSE '' -- メーカー
                   END + CASE
                       WHEN(hequipment.manufacturer_type IS NOT NULL AND hequipment.manufacturer_type <> '' ) AND ( equipment.manufacturer_type IS NULL OR equipment.manufacturer_type = '' ) THEN 'ManufacturerType_10|' -- 値が追加された場合
                       WHEN(hequipment.manufacturer_type IS NULL OR hequipment.manufacturer_type = '' ) AND ( equipment.manufacturer_type IS NOT NULL AND equipment.manufacturer_type <> '' ) THEN 'ManufacturerType_30|' -- 値が削除された場合
                       WHEN hequipment.manufacturer_type <> equipment.manufacturer_type THEN 'ManufacturerType_20|' -- 値が変更された場合
                       ELSE '' -- メーカー型式
                   END + CASE
                       WHEN(hequipment.model_no IS NOT NULL AND hequipment.model_no <> '' ) AND ( equipment.model_no IS NULL OR equipment.model_no = '' ) THEN 'ModelNo_10|' -- 値が追加された場合
                       WHEN(hequipment.model_no IS NULL OR hequipment.model_no = '' ) AND ( equipment.model_no IS NOT NULL AND equipment.model_no <> '' ) THEN 'ModelNo_30|' -- 値が削除された場合
                       WHEN hequipment.model_no <> equipment.model_no THEN 'ModelNo_20|' -- 値が変更された場合
                       ELSE '' -- 型式コード
                   END + CASE
                       WHEN(hequipment.serial_no IS NOT NULL AND hequipment.serial_no <> '' ) AND ( equipment.serial_no IS NULL OR equipment.serial_no = '' ) THEN 'SerialNo_10|' -- 値が追加された場合
                       WHEN(hequipment.serial_no IS NULL OR hequipment.serial_no = '' ) AND ( equipment.serial_no IS NOT NULL AND equipment.serial_no <> '' ) THEN 'SerialNo_30|' -- 値が削除された場合
                       WHEN hequipment.serial_no <> equipment.serial_no THEN 'SerialNo_20|' -- 値が変更された場合
                       ELSE '' -- 製造番号
                  END + CASE
                       WHEN(hequipment.date_of_manufacture IS NOT NULL ) AND ( equipment.date_of_manufacture IS NULL OR equipment.date_of_manufacture = '' ) THEN 'DateOfManufacture_10|' -- 値が追加された場合
                       WHEN(hequipment.date_of_manufacture IS NULL OR hequipment.date_of_manufacture = '' ) AND ( equipment.date_of_manufacture IS NOT NULL ) THEN 'DateOfManufacture_30|' -- 値が削除された場合
                       WHEN hequipment.date_of_manufacture <> equipment.date_of_manufacture THEN 'DateOfManufacture_20|' -- 値が変更された場合
                       ELSE '' -- 製造日
                  END + CASE
                       WHEN(hequipment.delivery_date IS NOT NULL AND hequipment.delivery_date <> '' ) AND ( equipment.delivery_date IS NULL OR equipment.delivery_date = '' ) THEN 'DeliveryDate_10|' -- 値が追加された場合
                       WHEN(hequipment.delivery_date IS NULL OR hequipment.delivery_date = '' ) AND ( equipment.delivery_date IS NOT NULL AND equipment.delivery_date <> '' ) THEN 'DeliveryDate_30|' -- 値が削除された場合
                       WHEN hequipment.delivery_date <> equipment.delivery_date THEN 'DeliveryDate_20|' -- 値が変更された場合
                       ELSE '' -- 納期
                  END + CASE
                       WHEN hequipment.use_segment_structure_id IS NOT NULL AND equipment.use_segment_structure_id IS NULL THEN 'UseSegment_10|' -- 値が追加された場合
                       WHEN hequipment.use_segment_structure_id IS NULL AND equipment.use_segment_structure_id IS NOT NULL THEN 'UseSegment_30|' -- 値が削除された場合
                       WHEN hequipment.use_segment_structure_id <> equipment.use_segment_structure_id THEN 'UseSegment_20|' -- 値が変更された場合
                       ELSE '' -- 使用区分
                  END + CASE
                       WHEN(hequipment.fixed_asset_no IS NOT NULL AND hequipment.fixed_asset_no <> '' ) AND ( equipment.fixed_asset_no IS NULL OR equipment.fixed_asset_no = '' ) THEN 'FixedAssetNo_10|' -- 値が追加された場合
                       WHEN(hequipment.fixed_asset_no IS NULL OR hequipment.fixed_asset_no = '' ) AND ( equipment.fixed_asset_no IS NOT NULL AND equipment.fixed_asset_no <> '' ) THEN 'FixedAssetNo_30|' -- 値が削除された場合
                       WHEN hequipment.fixed_asset_no <> equipment.fixed_asset_no THEN 'FixedAssetNo_20|' -- 値が変更された場合
                       ELSE '' -- 固定資産番号
                  END + CASE
                       WHEN(hequipment.equipment_note IS NOT NULL AND hequipment.equipment_note <> '' ) AND ( equipment.equipment_note IS NULL OR equipment.equipment_note = '' ) THEN 'EquipmentNote_10|' -- 値が追加された場合
                       WHEN(hequipment.equipment_note IS NULL OR hequipment.equipment_note = '' ) AND ( equipment.equipment_note IS NOT NULL AND equipment.equipment_note <> '' ) THEN 'EquipmentNote_30|' -- 値が削除された場合
                       WHEN hequipment.equipment_note <> equipment.equipment_note THEN 'EquipmentNote_20|' -- 値が変更された場合
                       ELSE '' -- 機器メモ
                  END + CASE
                       WHEN hequipment.circulation_target_flg IS NOT NULL AND equipment.circulation_target_flg IS NULL THEN 'CirculationTargetFlg_10|' -- 値が追加された場合
                       WHEN hequipment.circulation_target_flg IS NULL AND equipment.circulation_target_flg IS NOT NULL THEN 'CirculationTargetFlg_30|' -- 値が削除された場合
                       WHEN hequipment.circulation_target_flg <> equipment.circulation_target_flg THEN 'CirculationTargetFlg_20|' -- 値が変更された場合
                       ELSE '' -- 循環対象
                  END + CASE
                       WHEN hequipment.maintainance_kind_manage IS NOT NULL AND equipment.maintainance_kind_manage IS NULL THEN 'MaintainanceKindManage_10|' -- 値が追加された場合
                       WHEN hequipment.maintainance_kind_manage IS NULL AND equipment.maintainance_kind_manage IS NOT NULL THEN 'MaintainanceKindManage_30|' -- 値が削除された場合
                       WHEN hequipment.maintainance_kind_manage <> equipment.maintainance_kind_manage THEN 'MaintainanceKindManage_20|' -- 値が変更された場合
                       ELSE '' -- 点検種別毎管理
                  END
                )
        )
        ELSE ''
    END AS value_changed
FROM
    hm_history_management history -- 変更管理
    LEFT JOIN
        hm_history_management_detail detail -- 変更管理詳細
    ON  history.history_management_id = detail.history_management_detail_id
    LEFT JOIN
        hm_mc_machine hmachine -- 機番情報変更管理
    ON  detail.history_management_detail_id = hmachine.history_management_detail_id
    LEFT JOIN
        hm_mc_equipment hequipment -- 機器情報変更管理
    ON  detail.history_management_detail_id = hequipment.history_management_detail_id
    LEFT JOIN
        hm_law -- 適用法規変更管理
    ON  detail.history_management_detail_id = hm_law.history_management_detail_id
    LEFT JOIN
        mc_machine machine -- 機番情報
    ON  hmachine.machine_id = machine.machine_id
    LEFT JOIN
        law -- 適用法規(トランザクション)
    ON  machine.machine_id = law.machine_id
    LEFT JOIN
        mc_equipment equipment -- 機器情報
    ON  hequipment.equipment_id = equipment.equipment_id
    LEFT JOIN
        ms_structure division_ms --構成マスタ(申請区分)
    ON  history.application_division_id = division_ms.structure_id
    LEFT JOIN
        ms_item_extension division_ex -- アイテムマスタ拡張(申請区分)
    ON  division_ms.structure_item_id = division_ex.item_id
    AND division_ex.sequence_no = 1

WHERE
    history.history_management_id = @HistoryManagementId