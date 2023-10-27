
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
) AS equipment_level,
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
eq.equipment_note,
REPLACE((SELECT STR(applicable_laws_structure_id) + '|'
 FROM mc_applicable_laws ma
 WHERE ma.machine_id = mc.machine_id
 ORDER BY ma.applicable_laws_structure_id
 FOR XML PATH('')),' ','') AS applicable_laws_structure_id, 
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
FOR XML PATH('')),' ',''))-1) AS applicable_laws_name,
dbo.get_file_download_info(1600,mc.machine_id) AS file_link_machine,
dbo.get_file_download_info(1610,eq.equipment_id) AS file_link_equip 
FROM (SELECT mc.*,dbo.get_target_layer_id(mc.location_structure_id, 1)as factoryId FROM mc_machine mc) mc
LEFT JOIN mc_equipment eq
ON mc.machine_id = eq.machine_id
