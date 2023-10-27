
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
) AS equipment_level_name,
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
(SELECT 
  CASE  eq.circulation_target_flg 
  WHEN 1 THEN (
               SELECT TOP(1) v.translation_text
               FROM (SELECT * FROM v_structure_item_all WHERE structure_group_id = 2130 AND language_id = @LanguageId AND location_structure_id IN (0, factoryId))v,
			        ms_item_extension mi
			   WHERE v.structure_item_id = mi.item_id
			   AND mi.sequence_no = 1
			   AND mi.extension_data = '1'
			  )
  ELSE NULL
  END
) AS circulation_target_flg_name,
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
(SELECT 
  CASE  eq.maintainance_kind_manage 
  WHEN 1 THEN (
               SELECT TOP(1) v.translation_text
               FROM (SELECT * FROM v_structure_item_all WHERE structure_group_id = 2130 AND language_id = @LanguageId AND location_structure_id IN (0, factoryId))v,
			        ms_item_extension mi
			   WHERE v.structure_item_id = mi.item_id
			   AND mi.sequence_no = 1
			   AND mi.extension_data = '1'
			  )
  END
) AS maintainance_kind_manage_name,
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
WHERE EXISTS(SELECT * FROM #temp_structure_selected temp WHERE temp.structure_group_id = 1000 AND mc.location_structure_id = temp.structure_id)
AND EXISTS(SELECT * FROM #temp_structure_selected temp WHERE temp.structure_group_id = 1010 AND  mc.job_structure_id = temp.structure_id)