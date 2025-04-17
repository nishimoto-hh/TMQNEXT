-- 条件に指定された場所階層IDはリストで渡した構成IDが2100個以上だとエラーになるため一時テーブルに格納する
DROP TABLE IF EXISTS #temp_location_stcucture_id; 

CREATE TABLE #temp_location_stcucture_id(location_stcucture_id int); 

INSERT 
INTO #temp_location_stcucture_id 
SELECT
    * 
FROM
    STRING_SPLIT(@StrLocationStructureIdList, ',');


-- 条件に指定された職種階層IDはリストで渡した構成IDが2100個以上だとエラーになるため一時テーブルに格納する
/*@JobSelected
DROP TABLE IF EXISTS #temp_job_stcucture_id; 

CREATE TABLE #temp_job_stcucture_id(job_stcucture_id int); 

INSERT 
INTO #temp_job_stcucture_id 
SELECT
    * 
FROM
    STRING_SPLIT(@StrJobStcuctureIdList, ',');
@JobSelected*/

WITH circulation AS ( 
--循環対象の拡張データを取得
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
,maintainance_kind AS ( 
--点検種別毎管理の拡張データを取得
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
,history_management AS ( 
-- 変更管理対象工場を取得
    SELECT
        ms.structure_id 
    FROM
        ms_structure ms
        LEFT JOIN ms_item_extension ex 
            ON ms.structure_item_id = ex.item_id 
            AND ex.sequence_no = 4 
    WHERE
        ms.structure_group_id = 1000 
        AND ms.structure_layer_no = 1 
        AND ex.extension_data IS NULL
)
, management_standards_exists AS (
-- 機器に紐付く機器別管理基準の件数を取得
    SELECT
        machine.machine_id
        , count(comp.management_standards_component_id) cnt
    FROM
        mc_machine machine 
        LEFT JOIN mc_management_standards_component comp 
            ON machine.machine_id = comp.machine_id
    -- 場所階層
    WHERE machine.location_structure_id IN ( 
        SELECT
            location_stcucture_id 
        FROM
            #temp_location_stcucture_id
    )
    GROUP BY
        machine.machine_id
)
, tra_exists AS (
-- 「あり」の翻訳
    SELECT
        tra.translation_text
    FROM
        ms_translation tra
    WHERE
        tra.location_structure_id = 0
    AND tra.translation_id = 131010008
    AND tra.language_id = @LanguageId
)
, tra_not_exists AS (
-- 「なし」の翻訳
    SELECT
        tra.translation_text
    FROM
        ms_translation tra
    WHERE
        tra.location_structure_id = 0
    AND tra.translation_id = 131210001
    AND tra.language_id = @LanguageId
)
, target AS(
SELECT
    machine.machine_id                              -- 機番ID
    , machine.machine_no                            -- 機器番号
    , machine.machine_name                          -- 機器名称
    , machine.location_district_structure_id        -- 地区ID
    , machine.location_factory_structure_id         -- 工場ID
    , machine.location_plant_structure_id           -- プラントID
    , machine.location_series_structure_id          -- 系列ID
    , machine.location_stroke_structure_id          -- 工程ID
    , machine.location_facility_structure_id        -- 設備ID
    , machine.job_kind_structure_id                 -- 職種ID
    , machine.job_large_classfication_structure_id  -- 機種大分類ID
    , machine.job_middle_classfication_structure_id -- 機種中分類ID
    , machine.job_small_classfication_structure_id  -- 機種小分類ID
    , machine.importance_structure_id               -- 重要度ID
    , equip.maintainance_kind_manage                -- 点検種別毎管理
    , CASE 
        WHEN mse.cnt > 0 
            THEN tra_exists.translation_text 
        ELSE tra_not_exists.translation_text 
      END AS is_management_standards_exists -- 機器別管理基準有無
FROM
    mc_machine machine -- 機番情報
    INNER JOIN mc_equipment equip -- 機器情報
        ON machine.machine_id = equip.machine_id

    INNER JOIN history_management hm -- 変更管理対象外の工場
        ON machine.location_factory_structure_id = hm.structure_id

    LEFT JOIN circulation -- 循環対象の拡張項目
        ON CAST(equip.circulation_target_flg AS varchar) = circulation.extension_data

    LEFT JOIN maintainance_kind -- 点検種別の拡張項目
        ON CAST(equip.maintainance_kind_manage AS varchar) = maintainance_kind.extension_data

    LEFT JOIN management_standards_exists mse -- 機器別管理基準有無
       ON machine.machine_id = mse.machine_id

    CROSS JOIN tra_exists -- 「あり」の翻訳

    CROSS JOIN tra_not_exists -- 「なし」の翻訳
WHERE
    1 = 1

-- 場所階層
        AND machine.location_structure_id IN ( 
            SELECT
                location_stcucture_id 
            FROM
                #temp_location_stcucture_id
        )

/*@JobSelected
-- 職種・機種
        AND machine.job_structure_id IN ( 
            SELECT
                job_stcucture_id 
            FROM
                #temp_job_stcucture_id
        )
@JobSelected*/

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

/*@CirculationTargetStructureId
-- 循環対象
    AND circulation.structure_id = @CirculationTargetStructureId
@CirculationTargetStructureId*/

/*@FixedAssetNo
-- 固定資産番号
        AND equip.fixed_asset_no LIKE '%'+ @FixedAssetNo+'%'
@FixedAssetNo*/

/*@MaintainanceKindManageStructureId
-- 点検種別毎管理
        AND maintainance_kind.structure_id = @MaintainanceKindManageStructureId
@MaintainanceKindManageStructureId*/

/*@ManufacturerStructureId
-- メーカー
        AND equip.manufacturer_structure_id = @ManufacturerStructureId
@ManufacturerStructureId*/

/*@ManufacturerType
-- メーカー型式
        AND equip.manufacturer_type LIKE '%'+ @ManufacturerType+'%'
@ManufacturerType*/

/*@ModelNo
-- 型式コード
        AND equip.model_no LIKE '%'+ @ModelNo+'%'
@ModelNo*/

/*@SerialNo
-- シリアル番号
        AND equip.serial_no LIKE '%'+ @SerialNo+'%'
@SerialNo*/

/*@DateOfManufactureFrom
-- 製造日(From)
        AND equip.date_of_manufacture >= @DateOfManufactureFrom 
@DateOfManufactureFrom*/

/*@DateOfManufactureTo
-- 製造日(To)
        AND equip.date_of_manufacture <= @DateOfManufactureTo
@DateOfManufactureTo*/

/*@ImportanceStructureId
-- 重要度
        AND machine.importance_structure_id = @ImportanceStructureId
@ImportanceStructureId*/

/*@UseSegmentStructureId
-- 使用区分
        AND COALESCE(equip.use_segment_structure_id, 0) = @UseSegmentStructureId
@UseSegmentStructureId*/

/*@NotExistsManagementStandards
-- 機器別管理基準が紐付いていない機器を検索対象とする
    AND NOT EXISTS ( 
        SELECT
            * 
        FROM
            mc_management_standards_component comp 
        WHERE
            machine.machine_id = comp.machine_id
    )
@NotExistsManagementStandards*/

/*@ExistsManagementStandards
-- 機器別管理基準が紐付いていない機器を検索対象とする
    AND EXISTS ( 
        SELECT
            * 
        FROM
            mc_management_standards_component comp 
        WHERE
            machine.machine_id = comp.machine_id
    )
@ExistsManagementStandards*/
)