WITH law AS
-- 機番IDに紐付く適用法規(トランザクションテーブル)
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
hm_law AS
-- 変更管理詳細IDに紐付く適用法規(変更管理テーブル)
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
                                cast(laws.applicable_laws_structure_id AS varchar) + '|'
                            FROM
                                hm_mc_applicable_laws laws
                            WHERE
                                history.history_management_id = laws.history_management_id FOR XML PATH('')
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
factory_approval_user AS(
    -- 工場の承認ユーザID    
    SELECT
        ms.structure_id,
        ex.extension_data AS ex_data
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 4
    WHERE
        ms.structure_group_id = 1000
    AND ms.structure_layer_no = 1
),
target AS(
    SELECT
        history.key_id AS machine_id,
        COALESCE(hequipment.equipment_id, tequipment.equipment_id) AS equipment_id,
        COALESCE(hmachine.machine_no, tmachine.machine_no) AS machine_no,
        COALESCE(hmachine.machine_name, tmachine.machine_name) AS machine_name,
        COALESCE(hmachine.equipment_level_structure_id, tmachine.equipment_level_structure_id) AS equipment_level_structure_id,
        COALESCE(hmachine.location_structure_id, tmachine.location_structure_id) AS location_structure_id,
        CASE
         WHEN division_ex.extension_data = '20' AND hmachine.machine_id IS NOT NULL
       　 THEN hmachine.importance_structure_id
         ELSE COALESCE(hmachine.importance_structure_id, tmachine.importance_structure_id)
        END AS importance_structure_id,
        CASE
         WHEN division_ex.extension_data = '20' AND hmachine.machine_id IS NOT NULL
       　 THEN hmachine.conservation_structure_id
         ELSE COALESCE(hmachine.conservation_structure_id, tmachine.conservation_structure_id)
        END AS conservation_structure_id,
        CASE
         WHEN division_ex.extension_data = '20' AND hmachine.machine_id IS NOT NULL
       　 THEN hmachine.installation_location
         ELSE COALESCE(hmachine.installation_location, tmachine.installation_location)
        END AS installation_location,
        CASE
         WHEN division_ex.extension_data = '20' AND hmachine.machine_id IS NOT NULL
       　 THEN hmachine.number_of_installation
         ELSE COALESCE(hmachine.number_of_installation, tmachine.number_of_installation)
        END AS number_of_installation,
        CASE
         WHEN division_ex.extension_data = '20' AND hmachine.machine_id IS NOT NULL
       　 THEN hmachine.date_of_installation
         ELSE COALESCE(hmachine.date_of_installation, tmachine.date_of_installation)
        END AS date_of_installation,
        CASE
         WHEN division_ex.extension_data = '20' AND hmachine.machine_id IS NOT NULL
       　 THEN hmachine.machine_note
         ELSE COALESCE(hmachine.machine_note, tmachine.machine_note)
        END AS machine_note,
        CASE
         WHEN division_ex.extension_data = '20' AND hmachine.machine_id IS NOT NULL
       　 THEN hm_law.applicable_laws_structure_id
         ELSE COALESCE(hm_law.applicable_laws_structure_id, law.applicable_laws_structure_id)
        END AS applicable_laws_structure_id,
        COALESCE(hmachine.job_structure_id, tmachine.job_structure_id) AS job_structure_id,
        COALESCE(hequipment.manufacturer_structure_id, tequipment.manufacturer_structure_id) AS manufacturer_structure_id,
        CASE
         WHEN division_ex.extension_data = '20' AND hequipment.equipment_id IS NOT NULL
       　 THEN hequipment.manufacturer_type
         ELSE COALESCE(hequipment.manufacturer_type, tequipment.manufacturer_type)
        END AS manufacturer_type,
        CASE
         WHEN division_ex.extension_data = '20' AND hequipment.equipment_id IS NOT NULL
       　 THEN hequipment.model_no
         ELSE COALESCE(hequipment.model_no, tequipment.model_no)
        END AS model_no,
        CASE
         WHEN division_ex.extension_data = '20' AND hequipment.equipment_id IS NOT NULL
       　 THEN hequipment.serial_no
         ELSE COALESCE(hequipment.serial_no, tequipment.serial_no)
        END AS serial_no,
        CASE
         WHEN division_ex.extension_data = '20' AND hequipment.equipment_id IS NOT NULL
       　 THEN hequipment.date_of_manufacture
         ELSE COALESCE(hequipment.date_of_manufacture, tequipment.date_of_manufacture)
        END AS date_of_manufacture,
        CASE
         WHEN division_ex.extension_data = '20' AND hequipment.equipment_id IS NOT NULL
       　 THEN hequipment.delivery_date
         ELSE COALESCE(hequipment.delivery_date, tequipment.delivery_date)
        END AS delivery_date,
        CASE
         WHEN division_ex.extension_data = '20' AND hequipment.equipment_id IS NOT NULL
       　 THEN hequipment.use_segment_structure_id
         ELSE COALESCE(hequipment.use_segment_structure_id, tequipment.use_segment_structure_id)
        END AS use_segment_structure_id,
        COALESCE(
        REPLACE((
            SELECT
                dbo.get_file_download_info_row(att_temp.file_name, att_temp.attachment_id, att_temp.function_type_id, att_temp.key_id, att_temp.extension_data)
            FROM
                #temp_attachment AS att_temp
            WHERE
                hmachine.machine_id = att_temp.key_id
            AND att_temp.function_type_id = 1600
            ORDER BY
                document_no FOR xml path('')
        ), ' ', ''),
        REPLACE((
            SELECT
                dbo.get_file_download_info_row(att_temp.file_name, att_temp.attachment_id, att_temp.function_type_id, att_temp.key_id, att_temp.extension_data)
            FROM
                #temp_attachment AS att_temp
            WHERE
                tmachine.machine_id = att_temp.key_id
            AND att_temp.function_type_id = 1600
            ORDER BY
                document_no FOR xml path('')
        ), ' ', '') 
        ) AS file_link_machine,
        COALESCE(
        REPLACE((
            SELECT
                dbo.get_file_download_info_row(att_temp.file_name, att_temp.attachment_id, att_temp.function_type_id, att_temp.key_id, att_temp.extension_data)
            FROM
                #temp_attachment AS att_temp
            WHERE
                hequipment.equipment_id = att_temp.key_id
            AND att_temp.function_type_id = 1610
            ORDER BY
                document_no FOR xml path('')
        ), ' ', ''),
        REPLACE((
            SELECT
                dbo.get_file_download_info_row(att_temp.file_name, att_temp.attachment_id, att_temp.function_type_id, att_temp.key_id, att_temp.extension_data)
            FROM
                #temp_attachment AS att_temp
            WHERE
                tequipment.equipment_id = att_temp.key_id
            AND att_temp.function_type_id = 1610
            ORDER BY
                document_no FOR xml path('')
        ), ' ', '') 
        ) AS file_link_equipment,
        CASE
         WHEN division_ex.extension_data = '20' AND hequipment.equipment_id IS NOT NULL
       　 THEN hequipment.fixed_asset_no
         ELSE COALESCE(hequipment.fixed_asset_no, tequipment.fixed_asset_no)
        END AS fixed_asset_no,
        CASE
         WHEN division_ex.extension_data = '20' AND hequipment.equipment_id IS NOT NULL
       　 THEN hequipment.equipment_note
         ELSE COALESCE(hequipment.equipment_note, tequipment.equipment_note)
        END AS equipment_note,
        COALESCE(hequipment.circulation_target_flg, tequipment.circulation_target_flg) AS circulation_target_flg,
        COALESCE(hequipment.maintainance_kind_manage, tequipment.maintainance_kind_manage) AS maintainance_kind_manage,
        tmachine.location_structure_id AS old_location_structure_id,
        tmachine.job_structure_id AS old_job_structure_id,
        COALESCE(hmachine.update_serialid, tmachine.update_serialid) AS mc_update_serial_id,
        COALESCE(hequipment.update_serialid, tequipment.update_serialid) AS eq_update_serial_id,
        COALESCE(hmachine.location_district_structure_id, tmachine.location_district_structure_id) AS location_district_structure_id,
        COALESCE(hmachine.location_factory_structure_id, tmachine.location_factory_structure_id) AS location_factory_structure_id,
        CASE
         WHEN division_ex.extension_data = '20' AND hmachine.machine_id IS NOT NULL
       　 THEN hmachine.location_plant_structure_id
         ELSE COALESCE(hmachine.location_plant_structure_id, tmachine.location_plant_structure_id)
        END AS location_plant_structure_id,
        CASE
         WHEN division_ex.extension_data = '20' AND hmachine.machine_id IS NOT NULL
       　 THEN hmachine.location_series_structure_id
         ELSE COALESCE(hmachine.location_series_structure_id, tmachine.location_series_structure_id)
        END AS location_series_structure_id,
        CASE
         WHEN division_ex.extension_data = '20' AND hmachine.machine_id IS NOT NULL
       　 THEN hmachine.location_stroke_structure_id
         ELSE COALESCE(hmachine.location_stroke_structure_id, tmachine.location_stroke_structure_id)
        END AS location_stroke_structure_id,
         CASE
         WHEN division_ex.extension_data = '20' AND hmachine.machine_id IS NOT NULL
       　 THEN hmachine.location_facility_structure_id
         ELSE COALESCE(hmachine.location_facility_structure_id, tmachine.location_facility_structure_id)
        END AS location_facility_structure_id,
        CASE
         WHEN division_ex.extension_data = '20' AND hmachine.machine_id IS NOT NULL
       　 THEN hmachine.job_kind_structure_id
         ELSE COALESCE(hmachine.job_kind_structure_id, tmachine.job_kind_structure_id)
        END AS job_kind_structure_id,
        CASE
         WHEN division_ex.extension_data = '20' AND hmachine.machine_id IS NOT NULL
       　 THEN hmachine.job_large_classfication_structure_id
         ELSE COALESCE(hmachine.job_large_classfication_structure_id, tmachine.job_large_classfication_structure_id)
        END AS job_large_classfication_structure_id,
        CASE
         WHEN division_ex.extension_data = '20' AND hmachine.machine_id IS NOT NULL
       　 THEN hmachine.job_middle_classfication_structure_id
         ELSE COALESCE(hmachine.job_middle_classfication_structure_id, tmachine.job_middle_classfication_structure_id)
        END AS job_middle_classfication_structure_id,
        CASE
         WHEN division_ex.extension_data = '20' AND hmachine.machine_id IS NOT NULL
       　 THEN hmachine.job_small_classfication_structure_id
         ELSE COALESCE(hmachine.job_small_classfication_structure_id, tmachine.job_small_classfication_structure_id)
        END AS job_small_classfication_structure_id,
        hmachine.hm_machine_id,
        hequipment.hm_equipment_id,
        CASE
            WHEN hcomponent.cnt > 0 THEN 1
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
        approval_user.display_name AS approval_user_name,
        history.application_date,
        history.approval_date,
        history.history_management_id,
        history.update_serialid,
        history.application_reason,
        history.rejection_reason,
        dbo.get_target_layer_id(hmachine.location_structure_id, 1) AS factory_id,
        division_ex.extension_data AS application_division_code,
        status_ex.extension_data AS application_status_code,
        ---------- 以下は値の変更があった項目(申請区分が「変更申請：20」のデータ)を取得 ----------
        CASE
            WHEN hcomponent.cnt > 0 THEN 'Component_20|'
            ELSE ''
        END +
        -- 機器別管理基準変更有無
        CASE
            WHEN hmachine.hm_machine_id IS NOT NULL
        AND division_ex.extension_data = '20' THEN trim(
                '|'
                FROM
                    (-- ①ファンクションを使用し、変更前後の値を比較する
                        -- ②変更前後の値に差異がある場合は引数に渡した[項目名 + 背景色設定値]を返す(変更が無い場合は空文字が返ってくる)
                        -- ③変更のあった項目をパイプ[|]区切りで連結させて、変更のあった項目 とする
                        -- ※項目名(MachineNoやMachineName)はJavaScriptの背景色設定処理で使用するので統一させる
                        dbo.compare_newVal_with_oldVal(hmachine.machine_no, tmachine.machine_no, 'MachineNo') +                                                                     -- 機器番号
                        dbo.compare_newVal_with_oldVal(hmachine.machine_name, tmachine.machine_name, 'MachineName') +                                                               -- 機器名称
                        dbo.compare_newId_with_oldId(hmachine.equipment_level_structure_id, tmachine.equipment_level_structure_id, 'EquipmentLevel') +                              -- 機器レベル
                        dbo.compare_newId_with_oldId(hmachine.importance_structure_id, tmachine.importance_structure_id, 'Importance') +                                            -- 重要度
                        dbo.compare_newId_with_oldId(hmachine.conservation_structure_id, tmachine.conservation_structure_id, 'Conservation') +                                      -- 保全方式
                        dbo.compare_newVal_with_oldVal(hmachine.installation_location, tmachine.installation_location, 'InstallationLocation') +                                    -- 設置場所
                        dbo.compare_newId_with_oldId(hmachine.number_of_installation, tmachine.number_of_installation, 'NumberOfInstallation') +                                    -- 設置台数
                        dbo.compare_newVal_with_oldVal(hmachine.date_of_installation, tmachine.date_of_installation, 'DateOfInstallation') +                                        -- 設置日
                        dbo.compare_newVal_with_oldVal(hm_law.applicable_laws_structure_id, law.applicable_laws_structure_id, 'ApplicableLaws') +                                   -- 適用法規
                        dbo.compare_newVal_with_oldVal(hmachine.machine_note, tmachine.machine_note, 'MachineNote') +                                                               -- 機番メモ
                        dbo.compare_newId_with_oldId(hequipment.manufacturer_structure_id, tequipment.manufacturer_structure_id, 'Manufacturer') +                                  -- メーカー
                        dbo.compare_newVal_with_oldVal(hequipment.manufacturer_type, tequipment.manufacturer_type, 'ManufacturerType') +                                            -- メーカー型式
                        dbo.compare_newVal_with_oldVal(hequipment.model_no, tequipment.model_no, 'ModelNo') +                                                                       -- 型式コード
                        dbo.compare_newVal_with_oldVal(hequipment.serial_no, tequipment.serial_no, 'SerialNo') +                                                                    -- 製造番号
                        dbo.compare_newVal_with_oldVal(hequipment.date_of_manufacture, tequipment.date_of_manufacture, 'DateOfManufacture') +                                       -- 製造日
                        dbo.compare_newVal_with_oldVal(hequipment.delivery_date, tequipment.delivery_date, 'DeliveryDate') +                                                        -- 納期
                        dbo.compare_newId_with_oldId(hequipment.use_segment_structure_id, tequipment.use_segment_structure_id, 'UseSegment') +                                      -- 使用区分
                        dbo.compare_newVal_with_oldVal(hequipment.fixed_asset_no, tequipment.fixed_asset_no, 'FixedAssetNo') +                                                      -- 固定資産番号
                        dbo.compare_newVal_with_oldVal(hequipment.equipment_note, tequipment.equipment_note, 'EquipmentNote') +                                                     -- 機器メモ
                        dbo.compare_newId_with_oldId(hequipment.circulation_target_flg, tequipment.circulation_target_flg, 'CirculationTargetFlg') +                                -- 循環対象
                        dbo.compare_newId_with_oldId(hequipment.maintainance_kind_manage, tequipment.maintainance_kind_manage, 'MaintainanceKindManage_20') +                       -- 点検種別毎管理
                        dbo.compare_newId_with_oldId(hmachine.location_district_structure_id, tmachine.location_district_structure_id, 'District') +                                -- 地区
                        dbo.compare_newId_with_oldId(hmachine.location_factory_structure_id, tmachine.location_factory_structure_id, 'Factory') +                                   -- 工場
                        dbo.compare_newId_with_oldId(hmachine.location_plant_structure_id, tmachine.location_plant_structure_id, 'Plant') +                                         -- プラント
                        dbo.compare_newId_with_oldId(hmachine.location_series_structure_id, tmachine.location_series_structure_id, 'Series') +                                      -- 系列
                        dbo.compare_newId_with_oldId(hmachine.location_stroke_structure_id, tmachine.location_stroke_structure_id, 'Stroke') +                                      -- 工程
                        dbo.compare_newId_with_oldId(hmachine.location_facility_structure_id, tmachine.location_facility_structure_id, 'Facility') +                                -- 設備
                        dbo.compare_newId_with_oldId(hmachine.job_kind_structure_id, tmachine.job_kind_structure_id, 'Job') +                                                       -- 職種
                        dbo.compare_newId_with_oldId(hmachine.job_large_classfication_structure_id, tmachine.job_large_classfication_structure_id, 'Large') +                       -- 機種大分類
                        dbo.compare_newId_with_oldId(hmachine.job_middle_classfication_structure_id, tmachine.job_middle_classfication_structure_id, 'Middle') +                    -- 機種中分類
                        dbo.compare_newId_with_oldId(hmachine.job_small_classfication_structure_id, tmachine.job_small_classfication_structure_id, 'Small')                         -- 機種小分類
                    )
            )
            ELSE ''
        END AS value_changed
    FROM
        hm_history_management history
        -- 変更管理
        LEFT JOIN
            mc_machine tmachine
        -- 機番情報(トランザクション)
    ON  history.key_id = tmachine.machine_id
    LEFT JOIN
        mc_equipment tequipment
    -- 機器情報(トランザクション)
ON  tmachine.machine_id = tequipment.machine_id
LEFT JOIN
    law
-- 適用法規(トランザクション)
ON  tmachine.machine_id = law.machine_id
LEFT JOIN
    hm_mc_machine hmachine
-- 機番情報変更管理
ON  history.history_management_id = hmachine.history_management_id
LEFT JOIN
    hm_mc_equipment hequipment
-- 機器情報変更管理
ON  history.history_management_id = hequipment.history_management_id
LEFT JOIN
    hm_law
-- 適用法規変更管理
ON  history.history_management_id = hm_law.history_management_id
LEFT JOIN
    (
        SELECT
            component.history_management_id,
            COUNT(component.history_management_id) AS cnt
        FROM
            hm_mc_management_standards_component component
        GROUP BY
            component.history_management_id
    ) hcomponent
ON  history.history_management_id = hcomponent.history_management_id
LEFT JOIN
    ms_structure status_ms
-- 構成マスタ(申請状況)
ON  history.application_status_id = status_ms.structure_id
LEFT JOIN
    ms_item_extension status_ex
-- アイテムマスタ拡張(申請状況)
ON  status_ms.structure_item_id = status_ex.item_id
AND status_ex.sequence_no = 1
LEFT JOIN
    ms_structure division_ms
--構成マスタ(申請区分)
ON  history.application_division_id = division_ms.structure_id
LEFT JOIN
    ms_item_extension division_ex
-- アイテムマスタ拡張(申請区分)
ON  division_ms.structure_item_id = division_ex.item_id
AND division_ex.sequence_no = 1
LEFT JOIN
    factory_approval_user fau
-- 工場の承認ユーザー
ON  history.factory_id = fau.structure_id
LEFT JOIN
    ms_user approval_user
ON  fau.ex_data = CAST(approval_user.user_id AS nvarchar)
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
       -- 詳細画面 または 申請内容修正の場合、変更管理IDを指定
       AND history.history_management_id = @HistoryManagementId
    @IsDetail*/
)