

SELECT
    machine.machine_no,                                                               -- 機器番号
    machine.machine_name,                                                             -- 機器名称
    machine.equipment_level_structure_id,                                             -- 機器レベル
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
               st_f.structure_id = machine.equipment_level_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = machine.equipment_level_structure_id
) AS equipment_level,
    machine.job_structure_id,                                                         -- 職種機種ID
    machine.location_structure_id,                                                    -- 場所階層ID
    machine.importance_structure_id,                                                  -- 機器重要度
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
               st_f.structure_id = machine.importance_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = machine.importance_structure_id
) AS importance_name,
    machine.conservation_structure_id AS inspection_site_conservation_structure_id,   -- 保全方式
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
               st_f.structure_id = machine.conservation_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = machine.conservation_structure_id
) AS inspection_site_conservation_name ,
    equipment.serial_no,                                                              -- 製造番号
    equipment.manufacturer_structure_id,                                              -- メーカー
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
               st_f.structure_id = equipment.manufacturer_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = equipment.manufacturer_structure_id
) AS manufacturer_name,
    equipment.manufacturer_type,                                                      -- メーカー型式
    equipment.date_of_manufacture,                                                    -- 製造年月
    equipment.use_segment_structure_id,                                               -- 使用区分
    machine.date_of_installation,                                                     -- 設置年月
    machine.machine_id                                                                -- 機番ID
FROM
    (SELECT mc.*,dbo.get_target_layer_id(mc.location_structure_id, 1)as factoryId FROM mc_machine mc) machine
    LEFT JOIN
        mc_equipment equipment
    ON  machine.machine_id = equipment.machine_id
    LEFT JOIN 
        item 
    ON CAST(equipment.circulation_target_flg AS varchar) = item.extension_data 
WHERE
    -- 自分に紐付く機器は表示しない
    machine.machine_id NOT IN(
        -- ループに紐付く機器を取得
        SELECT
            machine.machine_id
        FROM
            mc_loop_info machine
        WHERE
            machine.loop_moto_id = (
                SELECT
                    loop.loop_id
                FROM
                    mc_loop_info loop
                WHERE
                    loop.machine_id = @MachineId
            )
    )

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

/*@UseSegmentStructureId
-- 使用区分
    AND equipment.use_segment_structure_id=@UseSegmentStructureId
@UseSegmentStructureId*/

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
        
