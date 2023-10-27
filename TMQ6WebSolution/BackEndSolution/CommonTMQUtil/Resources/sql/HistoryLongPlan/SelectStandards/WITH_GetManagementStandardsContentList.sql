WITH ms_con AS(
    SELECT
        con.management_standards_component_id,
        con.inspection_content_structure_id,
        con.budget_amount,
        con.schedule_type_structure_id,
        con.maintainance_kind_structure_id,
        con.management_standards_content_id,
        con.update_serialid
    FROM
        mc_management_standards_content AS con
    WHERE
        con.long_plan_id IS NULL
        AND NOT EXISTS ( 
            SELECT
                * 
            FROM
                hm_history_management history       -- 変更管理
                LEFT JOIN hm_mc_management_standards_content hmmsc -- 機器別管理基準内容変更管理
                    ON history.key_id = hmmsc.long_plan_id 
                    AND history.history_management_id = hmmsc.history_management_id 
                LEFT JOIN ms_structure status_ms    -- 構成マスタ(申請状況)
                    ON history.application_status_id = status_ms.structure_id 
                LEFT JOIN ms_item_extension status_ex -- アイテムマスタ拡張(申請状況)
                    ON status_ms.structure_item_id = status_ex.item_id 
                    AND status_ex.sequence_no = 1 
            WHERE
                hmmsc.management_standards_content_id = con.management_standards_content_id 
                AND hmmsc.long_plan_id IS NOT NULL 
                -- 「申請データ作成中」「承認依頼中」「差戻中」のデータのみ
                AND status_ex.extension_data IN ('10', '20', '30') 
                -- 「2：長期計画」のデータのみ
                AND history.application_conduct_id = 2
        )
),
ms_com AS(
    SELECT
        com.management_standards_component_id,
        com.machine_id,
        com.inspection_site_structure_id
    FROM
        mc_management_standards_component AS com
    WHERE
        com.is_management_standard_conponent = 1
),
-- 保全スケジュールを機器別管理基準内容IDごとに取得(同じ値なら最大の開始日のレコード)
schedule_start_date AS(
    SELECT
        sc.management_standards_content_id,
        sc.cycle_year,
        sc.cycle_month,
        sc.cycle_day,
        sc.start_date,
        sc.update_datetime
    FROM
        mc_maintainance_schedule AS sc
    WHERE
        NOT EXISTS(
            SELECT
                *
            FROM
                mc_maintainance_schedule AS sub
            WHERE
                sc.management_standards_content_id = sub.management_standards_content_id
            AND sc.start_date < sub.start_date
        )
),
-- 上で取得した保全スケジュールを機器別管理基準内容ID、開始日ごとに取得(同じ値なら最大の更新日時のレコード)
schedule_content AS(
    SELECT
        main.management_standards_content_id,
        main.cycle_year,
        main.cycle_month,
        main.cycle_day,
        main.start_date
    FROM
        schedule_start_date AS main
    WHERE
        NOT EXISTS(
            SELECT
                *
            FROM
                schedule_start_date AS sub
            WHERE
                main.management_standards_content_id = sub.management_standards_content_id
            AND main.start_date = sub.start_date
            AND main.update_datetime < sub.update_datetime
        )
),
--循環対象の拡張データを取得
item AS ( 
    SELECT
        structure_id
        , ie.extension_data 
    FROM
        v_structure si 
        INNER JOIN ms_item_extension ie 
            ON si.structure_item_id = ie.item_id 
            AND si.structure_group_id = 1920 
            AND ie.sequence_no = 1
)
,
--点検種別毎管理の拡張データを取得
item_maintainance AS ( 
    SELECT
        structure_id
        , ie.extension_data 
    FROM
        v_structure si 
        INNER JOIN ms_item_extension ie 
            ON si.structure_item_id = ie.item_id 
            AND si.structure_group_id = 2030 
            AND ie.sequence_no = 1
) 
,
-- 使用する構成グループの構成IDを絞込、工場の指定に用いる
structure_factory AS(
    SELECT
         structure_id
        ,location_structure_id AS factory_id
    FROM
        v_structure_item
    WHERE
        structure_group_id IN(1180, 1220, 1890, 1240)
    AND language_id = @LanguageId
)
,
machine AS(
    SELECT
        * 
    FROM
        mc_machine AS machine 
    WHERE
        NOT EXISTS ( 
            SELECT
                * 
            FROM
                hm_history_management history       -- 変更管理
                LEFT JOIN hm_mc_machine hmmachine   -- 機器台帳変更管理
                    ON history.key_id = hmmachine.machine_id 
                    AND history.history_management_id = hmmachine.history_management_id 
                LEFT JOIN ms_structure status_ms    -- 構成マスタ(申請状況)
                    ON history.application_status_id = status_ms.structure_id 
                LEFT JOIN ms_item_extension status_ex -- アイテムマスタ拡張(申請状況)
                    ON status_ms.structure_item_id = status_ex.item_id 
                    AND status_ex.sequence_no = 1 
                LEFT JOIN ms_structure division_ms  --構成マスタ(申請区分)
                    ON history.application_division_id = division_ms.structure_id 
                LEFT JOIN ms_item_extension division_ex -- アイテムマスタ拡張(申請区分)
                    ON division_ms.structure_item_id = division_ex.item_id 
                    AND division_ex.sequence_no = 1 
            WHERE
                hmmachine.machine_id = machine.machine_id 
                -- 「申請データ作成中」「承認依頼中」「差戻中」のデータのみ
                AND status_ex.extension_data IN ('10', '20', '30') 
                -- 「1：機器台帳」のデータのみ
                AND history.application_conduct_id = 1 
                AND division_ex.extension_data = '30' --削除申請
        )

)
,
-- 翻訳対象の一覧
target AS(
SELECT
    machine.machine_id,
    machine.machine_no,
    com.inspection_site_structure_id,
    con.inspection_content_structure_id,
    con.budget_amount,
    con.schedule_type_structure_id,
    schedule.cycle_year,
    schedule.cycle_month,
    schedule.cycle_day,
    schedule.start_date,
    con.maintainance_kind_structure_id,
    -- 非表示項目
    -- 排他チェック用
    con.management_standards_content_id,
    con.update_serialid AS update_serialid_content,
    -- ソート用
    machine.machine_name,
    com.management_standards_component_id,
    dbo.get_target_layer_id(machine.location_structure_id, 1) AS factory_id
FROM
    ms_con AS con
    INNER JOIN
        ms_com AS com
    ON  (
            con.management_standards_component_id = com.management_standards_component_id
        )
    INNER JOIN
        machine
    ON  (
            com.machine_id = machine.machine_id
        )
    LEFT OUTER JOIN
        schedule_content AS schedule
    ON  (
            con.management_standards_content_id = schedule.management_standards_content_id
        )
WHERE
    1 = 1
/*@LocationStructureIdList
-- 地区
    AND machine.location_structure_id IN @LocationStructureIdList
@LocationStructureIdList*/
/*@JobStructureIdList
-- 職種
    AND machine.job_structure_id IN @JobStructureIdList
@JobStructureIdList*/
/*@EquipmentLevelStructureId
-- 機器レベル
    AND machine.equipment_level_structure_id = @EquipmentLevelStructureId
@EquipmentLevelStructureId*/
/*@DateOfInstallationFrom
-- 設置日(From)
    AND machine.date_of_installation >= @DateOfInstallationFrom
@DateOfInstallationFrom*/
/*@DateOfInstallationTo
-- 設置日(To)
    AND machine.date_of_installation <= @DateOfInstallationTo
@DateOfInstallationTo*/
/*@MachineNo
-- 機器番号
    AND machine.machine_no LIKE '%'+ @MachineNo +'%'
    @MachineNo*/
/*@MachineName
-- 機器名称
    AND machine.machine_name LIKE '%'+ @MachineName+'%'
@MachineName*/
/*@ScheduleTypeStructureId
-- スケジュール管理
    AND con.schedule_type_structure_id = @ScheduleTypeStructureId
@ScheduleTypeStructureId*/
/*@InspectionSiteStructureId
-- 保全部位
    AND com.inspection_site_structure_id = @InspectionSiteStructureId
@InspectionSiteStructureId*/
/*@InspectionContentStructureId
-- 保全項目
    AND con.inspection_content_structure_id = @InspectionContentStructureId
@InspectionContentStructureId*/
AND EXISTS(
        SELECT
            *
        FROM
            mc_equipment AS eq
        LEFT JOIN 
            item 
        ON CAST(eq.circulation_target_flg AS varchar) = item.extension_data 
        LEFT JOIN
            item_maintainance
        ON CAST(eq.maintainance_kind_manage AS varchar) = item_maintainance.extension_data 
        WHERE
            machine.machine_id = eq.machine_id
        -- 使用区分
            AND eq.use_segment_structure_id IS NULL
        /*@CirculationTargetStructureId
        -- 循環対象
            AND item.structure_id = @CirculationTargetStructureId
        @CirculationTargetStructureId*/
        /*@FixedAssetNo
        -- 固定資産番号
            AND eq.fixed_asset_no LIKE '%'+ @FixedAssetNo+'%'
        @FixedAssetNo*/
        /*@ManufacturerStructureId
        -- メーカー
            AND eq.manufacturer_structure_id = @ManufacturerStructureId
        @ManufacturerStructureId*/
        /*@ManufacturerType
        -- メーカー型式
            AND eq.manufacturer_type LIKE '%'+ ManufacturerType+'%'
        @ManufacturerType*/
        /*@ModelNo
        -- 型式コード
            AND eq.model_no LIKE '%'+ @ModelNo+'%'
        @ModelNo*/
        /*@SerialNo
        -- シリアル番号
            AND eq.serial_no LIKE '%'+ @SerialNo+'%'
        @SerialNo*/
        /*@DateOfManufactureFrom
        -- 製造日(From)
            AND eq.date_of_manufacture >= @DateOfManufactureFrom 
        @DateOfManufactureFrom*/
        /*@DateOfManufactureTo
        -- 製造日(To)
            AND eq.date_of_manufacture <= @DateOfManufactureTo
        @DateOfManufactureTo*/
        /*@MaintainanceKindManageStructureId
        -- 点検種別毎管理
            AND item_maintainance.structure_id = @MaintainanceKindManageStructureId
        @MaintainanceKindManageStructureId*/
    )
)