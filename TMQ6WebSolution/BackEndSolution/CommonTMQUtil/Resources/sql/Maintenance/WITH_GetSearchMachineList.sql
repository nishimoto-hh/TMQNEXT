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
        machine_no,                                                             -- 機器番号
        machine_name,                                                           -- 機器名称
        equipment_level_structure_id,                                           -- 機器レベル
        job_structure_id,                                                       -- 職種機種ID
        location_structure_id,                                                  -- 場所階層ID
        importance_structure_id,                                                -- 機器重要度
        conservation_structure_id AS inspection_site_conservation_structure_id, -- 保全方式
        serial_no,                                                              -- 製造番号
        manufacturer_structure_id,                                              -- メーカー
        manufacturer_type,                                                      -- メーカー型式
        date_of_manufacture,                                                    -- 製造年月
        use_segment_structure_id,                                               -- 使用区分
        date_of_installation,                                                   -- 設置年月
        machine.machine_id,                                                     -- 機番ID
        dbo.get_target_layer_id(location_structure_id, 1) AS factoryId --このレコードの工場ID
    FROM
        mc_machine machine
        LEFT JOIN
            mc_equipment equipment
        ON  machine.machine_id = equipment.machine_id
        LEFT JOIN 
            item 
        ON CAST(equipment.circulation_target_flg AS varchar) = item.extension_data 
        LEFT JOIN 
            item_maintainance 
        ON CAST(equipment.maintainance_kind_manage AS varchar) = item_maintainance.extension_data 
    WHERE
    -- 使用区分
        equipment.use_segment_structure_id IS NULL

    /*@LocationSelected
    -- 地区
        AND machine.location_structure_id IN ( 
            SELECT
                location_stcucture_id 
            FROM
                #temp_location_stcucture_id
        )
    @LocationSelected*/

    /*@JobSelected
    -- 職種
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
        AND item.structure_id = @CirculationTargetStructureId
    @CirculationTargetStructureId*/
    /*@FixedAssetNo
    -- 固定資産番号
        AND equipment.fixed_asset_no LIKE '%'+ @FixedAssetNo+'%'
    @FixedAssetNo*/
        
    /*@ManufacturerStructureId
     -- メーカー
        AND equipment.manufacturer_structure_id = @ManufacturerStructureId
    @ManufacturerStructureId*/

    /*@ManufacturerType
     -- メーカー型式
         AND equipment.manufacturer_type LIKE '%'+ @ManufacturerType+'%'
    @ManufacturerType*/

    /*@ModelNo
    -- 型式コード
        AND equipment.model_no LIKE '%'+ @ModelNo+'%'
    @ModelNo*/
       
    /*@SerialNo
    -- シリアル番号
        AND equipment.serial_no LIKE '%'+ @SerialNo+'%'
    @SerialNo*/
            
    /*@DateOfManufactureFrom
    -- 製造日(From)
       AND equipment.date_of_manufacture >= @DateOfManufactureFrom 
    @DateOfManufactureFrom*/

    /*@DateOfManufactureTo
    -- 製造日(To)
        AND equipment.date_of_manufacture <= @DateOfManufactureTo
    @DateOfManufactureTo*/

    /*@MaintainanceKindManageStructureId
    -- 点検種別毎管理
        AND item_maintainance.structure_id = @MaintainanceKindManageStructureId
    @MaintainanceKindManageStructureId*/

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
            1170
            , 1200
            , 1030
            , 1150
            , 1210
        ) 
        AND language_id = @LanguageId
) 