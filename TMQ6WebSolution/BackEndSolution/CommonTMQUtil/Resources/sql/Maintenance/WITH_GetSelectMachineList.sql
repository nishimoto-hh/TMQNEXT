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
DROP TABLE IF EXISTS #temp_job_stcucture_id; 

CREATE TABLE #temp_job_stcucture_id(job_stcucture_id int); 

INSERT 
INTO #temp_job_stcucture_id 
SELECT
    * 
FROM
    STRING_SPLIT(@StrJobStcuctureIdList, ',');

WITH item AS ( 
    --循環対象の拡張データを取得
    SELECT
        structure_id
        , ie.extension_data 
    FROM
        ms_structure ms 
        INNER JOIN ms_item_extension ie 
            ON ms.structure_item_id = ie.item_id 
            AND ms.structure_group_id = 1920 
            AND ie.sequence_no = 1
)
, item_maintainance AS ( 
    --点検種別毎管理の拡張データを取得
    SELECT
        structure_id
        , ie.extension_data 
    FROM
        ms_structure ms 
        INNER JOIN ms_item_extension ie 
            ON ms.structure_item_id = ie.item_id 
            AND ms.structure_group_id = 2030 
            AND ie.sequence_no = 1
) 
, translate_target AS ( 
    -- 翻訳対象
    SELECT DISTINCT
        ma.machine_no
        , ma.machine_name
        , ma.job_structure_id
        , ma.location_structure_id
        , ma.importance_structure_id
    /*@SelectManagementStandards
        , com.inspection_site_structure_id
        , con.inspection_content_structure_id
    @SelectManagementStandards*/
        , ma.machine_id
        , eq.equipment_id 
        , ma.equipment_level_structure_id
        , ma.conservation_structure_id
        , dbo.get_target_layer_id(ma.location_structure_id, 1) AS factoryId --このレコードの工場ID
    FROM
        mc_machine ma 
        LEFT JOIN mc_equipment eq 
            ON ma.machine_id = eq.machine_id
    /*@SelectManagementStandards
        LEFT JOIN mc_management_standards_component com 
            ON ma.machine_id = com.machine_id 
        LEFT JOIN mc_management_standards_content con 
            ON com.management_standards_component_id = con.management_standards_component_id 
    @SelectManagementStandards*/
        LEFT JOIN item 
            ON CAST(eq.circulation_target_flg AS varchar) = item.extension_data 
        LEFT JOIN item_maintainance 
            ON CAST(eq.maintainance_kind_manage AS varchar) = item_maintainance.extension_data 
    WHERE
    -- 使用区分
        eq.use_segment_structure_id IS NULL
    /*@LocationSelected
    -- 地区
        AND ma.location_structure_id IN ( 
            SELECT
                location_stcucture_id 
            FROM
                #temp_location_stcucture_id
        )
    @LocationSelected*/

    /*@JobSelected
    -- 職種
        AND ma.job_structure_id IN ( 
            SELECT
                job_stcucture_id 
            FROM
                #temp_job_stcucture_id
        )
    @JobSelected*/

    /*@EquipmentLevelStructureId
    -- 機器レベル
        AND ma.equipment_level_structure_id = @EquipmentLevelStructureId
    @EquipmentLevelStructureId*/
    /*@DateOfInstallationFrom
    -- 設置日(From)
        AND ma.date_of_installation >= @DateOfInstallationFrom
    @DateOfInstallationFrom*/
    /*@DateOfInstallationTo
    -- 設置日(To)
        AND ma.date_of_installation <= @DateOfInstallationTo
    @DateOfInstallationTo*/
    /*@MachineNo
    -- 機器番号
        AND ma.machine_no LIKE '%'+ @MachineNo +'%'
    @MachineNo*/
    /*@MachineName
    -- 機器名称
        AND ma.machine_name LIKE '%'+ @MachineName+'%'
    @MachineName*/
    
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
    /*@InspectionSiteStructureId
    -- 保全部位
        AND com.inspection_site_structure_id = @InspectionSiteStructureId
    @InspectionSiteStructureId*/
    /*@InspectionContentStructureId
    -- 保全項目
        AND con.inspection_content_structure_id = @InspectionContentStructureId
    @InspectionContentStructureId*/
)
, structure_factory AS ( 
    -- 使用する構成グループの構成IDを絞込、工場の指定に用いる
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN ( 
            1200
            , 1180
            , 1220
        ) 
        AND language_id = @LanguageId
) 