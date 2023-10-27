
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
               structure_factory AS st_f 
           WHERE
               st_f.structure_id = mc.equipment_level_structure_id
               AND st_f.factory_id IN (0, factoryId)
       ) 
       AND tra.structure_id = mc.equipment_level_structure_id
) AS equipment_level_name,
mc.location_structure_id,
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
               structure_factory AS st_f 
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
               structure_factory AS st_f 
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
eq.circulation_target_flg,
(SELECT 
  CASE  eq.circulation_target_flg 
  WHEN 1 THEN (
               SELECT TOP(1) v.translation_text
               FROM v_structure_item_all v,
			        ms_item_extension mi
			   WHERE v.structure_item_id = mi.item_id
			   AND v.structure_group_id = 2130
			   AND mi.sequence_no = 1
			   AND mi.extension_data = 1
			   AND v.language_id = @LanguageId
			   AND v.location_structure_id IN (0, factoryId)
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
               structure_factory AS st_f 
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
               structure_factory AS st_f 
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
               FROM v_structure_item_all v,
			        ms_item_extension mi
			   WHERE v.structure_item_id = mi.item_id
			   AND v.structure_group_id = 2130
			   AND mi.sequence_no = 1
			   AND mi.extension_data = 1
			   AND v.language_id = @LanguageId
			   AND v.location_structure_id IN (0, factoryId)
			   )
  ELSE NULL
  END
) AS maintainance_kind_manage_name,
eq.equipment_note,
SUBSTRING(REPLACE((SELECT STR(applicable_laws_structure_id) + '|'
 FROM mc_applicable_laws ma
 WHERE ma.machine_id = mc.machine_id
 ORDER BY ma.applicable_laws_structure_id
 FOR XML PATH('')),' ',''),1,LEN(REPLACE((SELECT STR(applicable_laws_structure_id) + '|'
 FROM mc_applicable_laws ma
 WHERE ma.machine_id = mc.machine_id
 ORDER BY ma.applicable_laws_structure_id
 FOR XML PATH('')),' ',''))-1) AS applicable_laws_structure_id, 
SUBSTRING(REPLACE((SELECT tra.translation_text + ','
FROM
	v_structure_item_all AS tra ,
	mc_applicable_laws ml
WHERE
	tra.language_id = @LanguageId
	AND tra.location_structure_id = ( 
		SELECT
			MAX(st_f.factory_id) 
		FROM
			structure_factory AS st_f ,
			mc_applicable_laws mal
		WHERE
			st_f.structure_id = mal.applicable_laws_structure_id
			AND mal.machine_id = mc.machine_id
			AND st_f.factory_id IN (0, factoryId)
	) 
AND tra.structure_id = ml.applicable_laws_structure_id
AND ml.machine_id = mc.machine_id
ORDER BY ml.applicable_laws_structure_id
FOR XML PATH('')),' ',''),1,LEN(REPLACE((SELECT tra.translation_text + ','
								FROM
									v_structure_item_all AS tra ,
									mc_applicable_laws ml
								WHERE
									tra.language_id = @LanguageId
									AND tra.location_structure_id = ( 
										SELECT
											MAX(st_f.factory_id) 
										FROM
											structure_factory AS st_f ,
											mc_applicable_laws mal
										WHERE
											st_f.structure_id = mal.applicable_laws_structure_id
											AND mal.machine_id = mc.machine_id
											AND st_f.factory_id IN (0, factoryId)
									) 
								AND tra.structure_id = ml.applicable_laws_structure_id
								AND ml.machine_id = mc.machine_id
								ORDER BY ml.applicable_laws_structure_id
FOR XML PATH('')),' ',''))-1) AS applicable_laws_name
FROM (SELECT mc.*,dbo.get_target_layer_id(mc.location_structure_id, 1)as factoryId FROM mc_machine mc) mc
LEFT JOIN mc_equipment eq
ON mc.machine_id = eq.machine_id
WHERE EXISTS(SELECT * FROM #temp_structure_selected temp WHERE temp.structure_group_id = 1000 AND mc.location_structure_id = temp.structure_id)
AND EXISTS(SELECT * FROM #temp_structure_selected temp WHERE temp.structure_group_id = 1010 AND  mc.job_structure_id = temp.structure_id)