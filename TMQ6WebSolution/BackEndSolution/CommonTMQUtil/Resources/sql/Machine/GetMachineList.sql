
SELECT 
mc.machine_id,
eq.equipment_id,
mc.machine_no,
mc.machine_name,
mc.equipment_level_structure_id,
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.equipment_level_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.equipment_level_structure_id
) AS equipment_level,
mc.location_structure_id,
mc.location_district_structure_id AS district_id,
mc.location_factory_structure_id AS factory_id,
mc.location_plant_structure_id AS plant_id,
mc.location_series_structure_id AS series_id,
mc.location_stroke_structure_id AS stroke_id,
mc.location_facility_structure_id AS facility_id,
mc.importance_structure_id,
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.importance_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.importance_structure_id
) AS importance_name,
mc.conservation_structure_id AS inspection_site_conservation_structure_id,
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.conservation_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.conservation_structure_id
) AS inspection_site_conservation_name ,
mc.installation_location,
mc.number_of_installation,
mc.date_of_installation,
mc.machine_note,
mc.job_structure_id,
mc.job_kind_structure_id AS job_id,
mc.job_large_classfication_structure_id AS large_classfication_id,
mc.job_middle_classfication_structure_id AS middle_classfication_id,
mc.job_small_classfication_structure_id AS small_classfication_id,
eq.circulation_target_flg,
eq.manufacturer_structure_id,
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = eq.manufacturer_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = eq.manufacturer_structure_id
) AS manufacturer_name,
eq.manufacturer_type,
eq.model_no,
eq.serial_no,
eq.date_of_manufacture,
eq.delivery_date,
eq.use_segment_structure_id,
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = eq.use_segment_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = eq.use_segment_structure_id
) AS use_segment_name,
eq.fixed_asset_no,
eq.maintainance_kind_manage,
eq.equipment_note,

REPLACE((SELECT STR(applicable_laws_structure_id) + '|'
 FROM mc_applicable_laws ma
 WHERE ma.machine_id = mc.machine_id
 ORDER BY ma.applicable_laws_structure_id
 FOR XML PATH('')),' ','') AS applicable_laws_structure_id, 

TRIM(',' FROM
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
FOR XML PATH(''))) AS applicable_laws_name,
--TRIMÇ…ïœçX
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
REPLACE((
        SELECT
            dbo.get_file_download_info_row(att_temp.file_name, att_temp.attachment_id, att_temp.function_type_id, att_temp.key_id, att_temp.extension_data)
        FROM
            #temp_attachment as att_temp
        WHERE
            mc.machine_id = att_temp.key_id
        AND att_temp.function_type_id = 1600
        ORDER BY
            document_no FOR xml path('')
    ), ' ', '')AS file_link_machine,
REPLACE((
        SELECT
            dbo.get_file_download_info_row(att_temp.file_name, att_temp.attachment_id, att_temp.function_type_id, att_temp.key_id, att_temp.extension_data)
        FROM
            #temp_attachment as att_temp
        WHERE
            eq.equipment_id = att_temp.key_id
        AND att_temp.function_type_id = 1610
        ORDER BY
            document_no FOR xml path('')
    ), ' ', '')AS file_link_equip,

-- èÍèääKëwÇÃñ|ñÛ
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.location_district_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.location_district_structure_id
) AS district_name,
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.location_factory_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.location_factory_structure_id
) AS factory_name,
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.location_plant_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.location_plant_structure_id
) AS plant_name,
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.location_series_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.location_series_structure_id
) AS series_name,
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.location_stroke_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.location_stroke_structure_id
) AS stroke_name,
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.location_facility_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.location_facility_structure_id
) AS facility_name,
-- êEéÌã@éÌäKëwÇÃñ|ñÛ
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.job_kind_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.job_kind_structure_id
) AS job_name,
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.job_large_classfication_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.job_large_classfication_structure_id
) AS large_classfication_name,
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.job_middle_classfication_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.job_middle_classfication_structure_id
) AS middle_classfication_name,
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
               #temp_structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.job_small_classfication_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.job_small_classfication_structure_id
) AS small_classfication_name
FROM (SELECT mc.*,location_factory_structure_id as factoryId FROM mc_machine mc) mc
LEFT JOIN mc_equipment eq
ON mc.machine_id = eq.machine_id
