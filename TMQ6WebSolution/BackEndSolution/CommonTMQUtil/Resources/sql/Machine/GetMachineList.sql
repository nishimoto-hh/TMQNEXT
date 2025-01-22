
SELECT 
 mc.machine_id
,eq.equipment_id
-- 機器番号
/*@MachineNo
,mc.machine_no
@MachineNo*/
-- 機器名称
/*@MachineName
,mc.machine_name
@MachineName*/
-- 機器レベル
/*@EquipmentLevel
--,mc.equipment_level_structure_id
,( 
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.equipment_level_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.equipment_level_structure_id
) AS equipment_level
@EquipmentLevel*/
--場所階層
--,mc.location_structure_id
--,mc.location_district_structure_id AS district_id
--,mc.location_factory_structure_id AS factory_id
--,mc.location_plant_structure_id AS plant_id
--,mc.location_series_structure_id AS series_id
--,mc.location_stroke_structure_id AS stroke_id
--,mc.location_facility_structure_id AS facility_id
-- 重要度
/*@ImportanceName
--,mc.importance_structure_id
,( 
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.importance_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.importance_structure_id
) AS importance_name
@ImportanceName*/
-- 保全方式
/*@InspectionSiteConservationName
--,mc.conservation_structure_id AS inspection_site_conservation_structure_id
,( 
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.conservation_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.conservation_structure_id
) AS inspection_site_conservation_name
@InspectionSiteConservationName*/
-- 設置場所
/*@InstallationLocation
,mc.installation_location
@InstallationLocation*/
-- 設置台数
/*@NumberOfInstallation
,mc.number_of_installation
@NumberOfInstallation*/
-- 設置年月
/*@DateOfInstallation
,mc.date_of_installation
@DateOfInstallation*/
-- 機番メモ
/*@MachineNote
,mc.machine_note
@MachineNote*/
--職種機種
--,mc.job_structure_id
--,mc.job_kind_structure_id AS job_id
--,mc.job_large_classfication_structure_id AS large_classfication_id
--,mc.job_middle_classfication_structure_id AS middle_classfication_id
--,mc.job_small_classfication_structure_id AS small_classfication_id
-- 循環対象
/*@CirculationTargetFlg
,eq.circulation_target_flg
@CirculationTargetFlg*/
-- メーカー
/*@ManufacturerName
--,eq.manufacturer_structure_id
,( 
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = eq.manufacturer_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = eq.manufacturer_structure_id
) AS manufacturer_name
@ManufacturerName*/
-- メーカー型式
/*@ManufacturerType
,eq.manufacturer_type
@ManufacturerType*/
-- 型式コード
/*@ModelNo
,eq.model_no
@ModelNo*/
-- 製造番号
/*@SerialNo
,eq.serial_no
@SerialNo*/
-- 製造年月
/*@DateOfManufacture
,eq.date_of_manufacture
@DateOfManufacture*/
-- 納期
/*@DeliveryDate
,eq.delivery_date
@DeliveryDate*/
-- 使用区分
/*@UseSegmentName
--,eq.use_segment_structure_id
,( 
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = eq.use_segment_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = eq.use_segment_structure_id
) AS use_segment_name
@UseSegmentName*/
-- 固定資産番号
/*@FixedAssetNo
,eq.fixed_asset_no
@FixedAssetNo*/
-- 点検種別毎管理
/*@MaintainanceKindManage
,eq.maintainance_kind_manage
@MaintainanceKindManage*/
-- 機器メモ
/*@EquipmentNote
,eq.equipment_note
@EquipmentNote*/

-- 適用法規
/*@ApplicableLawsName
--,REPLACE((SELECT STR(applicable_laws_structure_id) + '|'
-- FROM mc_applicable_laws ma
-- WHERE ma.machine_id = mc.machine_id
-- ORDER BY ma.applicable_laws_structure_id
-- FOR XML PATH('')),' ','') AS applicable_laws_structure_id
,TRIM(',' FROM
(SELECT tra.translation_text + ','
FROM
	v_structure_item_all AS tra ,
	mc_applicable_laws ml
WHERE
	tra.language_id = @LanguageId
	AND tra.location_structure_id = ( 
		SELECT
			MAX(st_f.factory_id) 
		FROM
			#temp_structure_factory AS st_f 
		WHERE
			st_f.structure_id = ml.applicable_laws_structure_id
			AND st_f.factory_id IN (0, factoryId)
	) 
AND tra.structure_id = ml.applicable_laws_structure_id
AND ml.machine_id = mc.machine_id
ORDER BY ml.applicable_laws_structure_id
FOR XML PATH(''))) AS applicable_laws_name
@ApplicableLawsName*/
--TRIMに変更
--SUBSTRING((SELECT tra.translation_text + ','
--FROM
--	v_structure_item_all AS tra ,
--	mc_applicable_laws ml
--WHERE
--	tra.language_id = @LanguageId
--	AND tra.location_structure_id = ( 
--		SELECT
--			MAX(st_f.factory_id) 
--		FROM
--			#temp_structure_factory AS st_f 
--		WHERE
--			st_f.structure_id = ml.applicable_laws_structure_id
--			AND st_f.factory_id IN (0, factoryId)
--	) 
--AND tra.structure_id = ml.applicable_laws_structure_id
--AND ml.machine_id = mc.machine_id
--ORDER BY ml.applicable_laws_structure_id
--FOR XML PATH('')),1,LEN((SELECT tra.translation_text + ','
--								FROM
--									v_structure_item_all AS tra ,
--									mc_applicable_laws ml
--								WHERE
--									tra.language_id = @LanguageId
--									AND tra.location_structure_id = ( 
--										SELECT
--											MAX(st_f.factory_id) 
--										FROM
--											#temp_structure_factory AS st_f 
--										WHERE
--											st_f.structure_id = ml.applicable_laws_structure_id
--											AND st_f.factory_id IN (0, factoryId)
--									) 
--								AND tra.structure_id = ml.applicable_laws_structure_id
--								AND ml.machine_id = mc.machine_id
--								ORDER BY ml.applicable_laws_structure_id
--FOR XML PATH('')))-1) AS applicable_laws_name,

--dbo.get_file_download_info(1600,mc.machine_id) AS file_link_machine,
--dbo.get_file_download_info(1610,eq.equipment_id) AS file_link_equip,
-- 機番添付有無
/*@FileLinkMachine
,REPLACE((
        SELECT
            dbo.get_file_download_info_row(att_temp.file_name, att_temp.attachment_id, att_temp.function_type_id, att_temp.key_id, att_temp.extension_data)
        FROM
            #temp_attachment as att_temp
        WHERE
            mc.machine_id = att_temp.key_id
        AND att_temp.function_type_id = 1600
        ORDER BY
            document_no FOR xml path('')
    ), ' ', '')AS file_link_machine
@FileLinkMachine*/
-- 機器添付有無
/*@FileLinkEquip
,REPLACE((
        SELECT
            dbo.get_file_download_info_row(att_temp.file_name, att_temp.attachment_id, att_temp.function_type_id, att_temp.key_id, att_temp.extension_data)
        FROM
            #temp_attachment as att_temp
        WHERE
            eq.equipment_id = att_temp.key_id
        AND att_temp.function_type_id = 1610
        ORDER BY
            document_no FOR xml path('')
    ), ' ', '')AS file_link_equip
@FileLinkEquip*/
-- 場所階層の翻訳
/*@DistrictName
,( 
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.location_district_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.location_district_structure_id
) AS district_name
@DistrictName*/
/*@FactoryName
,( 
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.location_factory_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.location_factory_structure_id
) AS factory_name
@FactoryName*/
/*@PlantName
,( 
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.location_plant_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.location_plant_structure_id
) AS plant_name
@PlantName*/
/*@SeriesName
,( 
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.location_series_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.location_series_structure_id
) AS series_name
@SeriesName*/
/*@StrokeName
,( 
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.location_stroke_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.location_stroke_structure_id
) AS stroke_name
@StrokeName*/
/*@FacilityName
,( 
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.location_facility_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.location_facility_structure_id
) AS facility_name
@FacilityName*/
-- 職種機種の翻訳
/*@JobName
,( 
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.job_kind_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.job_kind_structure_id
) AS job_name
@JobName*/
/*@LargeClassficationName
,( 
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.job_large_classfication_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.job_large_classfication_structure_id
) AS large_classfication_name
@LargeClassficationName*/
/*@MiddleClassficationName
,( 
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.job_middle_classfication_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.job_middle_classfication_structure_id
) AS middle_classfication_name
@MiddleClassficationName*/
/*@SmallClassficationName
,( 
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.job_small_classfication_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.job_small_classfication_structure_id
) AS small_classfication_name
@SmallClassficationName*/
FROM (SELECT mc.*,location_factory_structure_id as factoryId FROM mc_machine mc) mc
LEFT JOIN mc_equipment eq
ON mc.machine_id = eq.machine_id

LEFT JOIN (
    SELECT
        machine.machine_id
        , REPLACE ( 
            ( 
                SELECT
                    STR(applicable_laws_structure_id) + '|' 
                FROM
                    mc_applicable_laws ma 
                WHERE
                    ma.machine_id = machine.machine_id 
                ORDER BY
                    ma.applicable_laws_structure_id FOR XML PATH ('')
            ) 
            , ' '
            , ''
        ) AS applicable_laws_structure_id 
    FROM
        mc_machine machine
    ) ma
ON mc.machine_id = ma.machine_id
