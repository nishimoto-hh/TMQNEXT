WITH structure_factory AS( -- 翻訳の取得に使用
    SELECT
        structure_id,
        location_structure_id AS factory_id
    FROM
        v_structure_item_all
    WHERE
        structure_group_id IN(1030, 1150, 1160, 1170, 1200, 1210, 2090, 2100)
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
),
target AS(
    SELECT
        COALESCE(hmachine.machine_id, cmachine.machine_id) AS machine_id,
        COALESCE(hmachine.machine_no, cmachine.machine_no) AS machine_no,
        COALESCE(hmachine.machine_name, cmachine.machine_name) AS machine_name,
        COALESCE(hmachine.equipment_level_structure_id, cmachine.equipment_level_structure_id) AS equipment_level_structure_id,
        COALESCE(hmachine.location_structure_id, cmachine.location_structure_id) AS location_structure_id,
        COALESCE(hmachine.importance_structure_id, cmachine.importance_structure_id) AS importance_structure_id,
        COALESCE(hmachine.conservation_structure_id, cmachine.conservation_structure_id) AS conservation_structure_id,
        COALESCE(hmachine.installation_location, cmachine.installation_location) AS installation_location,
        COALESCE(hmachine.number_of_installation, cmachine.number_of_installation) AS number_of_installation,
        COALESCE(hmachine.date_of_installation, cmachine.date_of_installation) AS date_of_installation,
        COALESCE(hmachine.machine_note, cmachine.machine_note) AS machine_note,
        COALESCE(hm_law.applicable_laws_structure_id, claw.applicable_laws_structure_id) AS applicable_laws_structure_id,
        COALESCE(hmachine.job_structure_id, cmachine.job_structure_id) AS job_structure_id,
        COALESCE(hequipment.manufacturer_structure_id, cequipment.manufacturer_structure_id) AS manufacturer_structure_id,
        COALESCE(hequipment.manufacturer_type, cequipment.manufacturer_type) AS manufacturer_type,
        COALESCE(hequipment.model_no, cequipment.model_no) AS model_no,
        COALESCE(hequipment.serial_no, cequipment.serial_no) AS serial_no,
        COALESCE(hequipment.date_of_manufacture, cequipment.date_of_manufacture) AS date_of_manufacture,
        COALESCE(hequipment.delivery_date, cequipment.delivery_date) AS delivery_date,
        COALESCE(hequipment.use_segment_structure_id, cequipment.use_segment_structure_id) AS use_segment_structure_id,
        COALESCE(dbo.get_file_download_info(1610, hequipment.equipment_id), dbo.get_file_download_info(1610, cequipment.equipment_id)) AS file_link_equipment,
        COALESCE(dbo.get_file_download_info(1600, hmachine.machine_id), dbo.get_file_download_info(1600, cmachine.machine_id)) AS file_link_machine,
        COALESCE(hequipment.fixed_asset_no, cequipment.fixed_asset_no) AS fixed_asset_no,
        COALESCE(hequipment.equipment_note, cequipment.equipment_note) AS equipment_note,
        COALESCE(hequipment.circulation_target_flg, cequipment.circulation_target_flg) AS circulation_target_flg,
        COALESCE(hequipment.maintainance_kind_manage, cequipment.maintainance_kind_manage) AS maintainance_kind_manage,
        COALESCE(machine.location_structure_id, cmachine.location_structure_id) AS old_location_structure_id,
        COALESCE(machine.job_structure_id, cmachine.job_structure_id) AS old_job_structure_id,
        CASE
            WHEN hcomponent.hm_management_standards_component_id IS NOT NULL THEN 1
            ELSE 0
        END AS is_changed_component,
        CASE
            WHEN hmachine.machine_id IS NOT NULL THEN 1
            ELSE 0
        END AS data_type,
        history.application_status_id,
        history.application_division_id,
        history.application_conduct_id,
        history.application_user_name,
        history.approval_user_name,
        history.application_date,
        history.approval_date,
        history.history_management_id,
        history.update_serialid,
        history.application_reason,
        history.rejection_reason,
        detail.history_management_detail_id,
        dbo.get_target_layer_id(hmachine.location_structure_id, 1) AS factory_id,
        division_ex.extension_data AS application_division_code,
        status_ex.extension_data AS application_status_code,
        ---------- 以下は値の変更があった項目(申請区分が「変更申請：20」のデータ)を取得 ----------
        CASE
            WHEN hmachine.history_management_detail_id IS NULL THEN 'Component_20' -- 機器別管理基準変更有無
            WHEN division_ex.extension_data = '20' THEN trim(
                '|'
                FROM
                    (
                       -- ①ファンクションを使用し、変更前後の値を比較する
                       -- ②変更前後の値に差異がある場合は引数に渡した[項目名 + 背景色設定値]を返す(変更が無い場合は空文字が返ってくる)
                       -- ③変更のあった項目をパイプ[|]区切りで連結させて、変更のあった項目 とする
                       -- ※項目名(MachineNoやMachineName)はJavaScriptの背景色設定処理で使用するので統一させる
                       dbo.compare_newVal_with_oldVal(hmachine.machine_no, machine.machine_no, 'MachineNo') +                                              -- 機器番号
                       dbo.compare_newVal_with_oldVal(hmachine.machine_name, machine.machine_name, 'MachineName') +                                        -- 機器名称
                       dbo.compare_newId_with_oldId(hmachine.equipment_level_structure_id, machine.equipment_level_structure_id, 'EquipmentLevel') +       -- 機器レベル
                       dbo.compare_newId_with_oldId(hmachine.importance_structure_id, machine.importance_structure_id, 'Importance') +                     -- 重要度
                       dbo.compare_newId_with_oldId(hmachine.conservation_structure_id, machine.conservation_structure_id, 'Conservation') +               -- 保全方式
                       dbo.compare_newVal_with_oldVal(hmachine.installation_location, machine.installation_location, 'InstallationLocation') +             -- 設置場所
                       dbo.compare_newId_with_oldId(hmachine.number_of_installation, machine.number_of_installation, 'NumberOfInstallation') +             -- 設置台数
                       dbo.compare_newVal_with_oldVal(hmachine.date_of_installation, machine.date_of_installation, 'DateOfInstallation') +                 -- 設置日
                       dbo.compare_newVal_with_oldVal(hm_law.applicable_laws_structure_id, law.applicable_laws_structure_id, 'ApplicableLaws') +           -- 適用法規
                       dbo.compare_newVal_with_oldVal(hmachine.machine_note, machine.machine_note, 'MachineNote') +                                        -- 機番メモ
                       dbo.compare_newId_with_oldId(hequipment.manufacturer_structure_id, equipment.manufacturer_structure_id, 'Manufacturer') +           -- メーカー
                       dbo.compare_newVal_with_oldVal(hequipment.manufacturer_type, equipment.manufacturer_type, 'ManufacturerType') +                     -- メーカー型式
                       dbo.compare_newVal_with_oldVal(hequipment.model_no, equipment.model_no, 'ModelNo') +                                                -- 型式コード
                       dbo.compare_newVal_with_oldVal(hequipment.serial_no, equipment.serial_no, 'SerialNo') +                                             -- 製造番号
                       dbo.compare_newVal_with_oldVal(hequipment.date_of_manufacture, equipment.date_of_manufacture, 'DateOfManufacture') +                -- 製造日
                       dbo.compare_newVal_with_oldVal(hequipment.delivery_date, equipment.delivery_date, 'DeliveryDate') +                                 -- 納期
                       dbo.compare_newId_with_oldId(hequipment.use_segment_structure_id, equipment.use_segment_structure_id, 'UseSegment') +               -- 使用区分
                       dbo.compare_newVal_with_oldVal(hequipment.fixed_asset_no, equipment.fixed_asset_no, 'FixedAssetNo') +                               -- 固定資産番号
                       dbo.compare_newVal_with_oldVal(hequipment.equipment_note, equipment.equipment_note, 'EquipmentNote') +                              -- 機器メモ
                       dbo.compare_newId_with_oldId(hequipment.circulation_target_flg, equipment.circulation_target_flg, 'CirculationTargetFlg') +         -- 循環対象
                       dbo.compare_newId_with_oldId(hequipment.maintainance_kind_manage, equipment.maintainance_kind_manage, 'MaintainanceKindManage_20')  -- 点検種別毎管理
                    )
            )
            ELSE ''
        END AS value_changed
    FROM
        hm_history_management history -- 変更管理
        LEFT JOIN
            hm_history_management_detail detail -- 変更管理詳細
        ON  history.history_management_id = detail.history_management_id
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
            hm_mc_management_standards_component hcomponent -- 機器別管理基準部位変更管理
        ON  detail.history_management_detail_id = hcomponent.history_management_detail_id
        LEFT JOIN
            mc_machine machine -- 機番情報(トランザクション)
        ON  hmachine.machine_id = machine.machine_id
        LEFT JOIN
            mc_equipment equipment -- 機器情報(トランザクション)
        ON  machine.machine_id = equipment.machine_id
        LEFT JOIN
            law -- 適用法規(トランザクション)
        ON  machine.machine_id = law.machine_id
        LEFT JOIN
            mc_machine cmachine -- 機番情報(トランザクション)
        ON  hcomponent.machine_id = cmachine.machine_id
        LEFT JOIN
            mc_equipment cequipment -- 機器情報(トランザクション)
        ON  cmachine.machine_id = cequipment.machine_id
        LEFT JOIN
            law AS claw -- 適用法規(トランザクション)
        ON  cmachine.machine_id = claw.machine_id
        LEFT JOIN
            ms_structure status_ms -- 構成マスタ(申請状況)
        ON  history.application_status_id = status_ms.structure_id
        LEFT JOIN
            ms_item_extension status_ex -- アイテムマスタ拡張(申請状況)
        ON  status_ms.structure_item_id = status_ex.item_id
        AND status_ex.sequence_no = 1
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
        -- 「1：機器台帳」のデータのみ
        AND history.application_conduct_id = 1

        /*@DispOnlyMySubject
        -- 自分の件名のみ表示
        AND history.application_user_id = @UserId
        @DispOnlyMySubject*/

        /*@IsDetail
        -- 詳細画面の場合、変更管理IDを指定
        AND history.history_management_id = @HistoryManagementId
        @IsDetail*/
)