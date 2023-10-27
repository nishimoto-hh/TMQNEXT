
SELECT 
mc.machine_id,
mc.machine_no,
mc.machine_name,
mc.equipment_level_structure_id,
mc.location_structure_id,
mc.importance_structure_id,
mc.conservation_structure_id AS inspection_site_conservation_structure_id,
mc.installation_location,
mc.number_of_installation,
mc.date_of_installation,
mc.machine_note,
mc.job_structure_id,
mc.update_serialid AS mc_update_serialid,
eq.equipment_id,
eq.circulation_target_flg,
eq.manufacturer_structure_id,
eq.manufacturer_type,
eq.model_no,
eq.serial_no,
eq.date_of_manufacture,
eq.delivery_date,
eq.use_segment_structure_id,
eq.fixed_asset_no,
eq.maintainance_kind_manage,
eq.equipment_note,
eq.update_serialid AS eq_update_serialid,
REPLACE((SELECT STR(applicable_laws_structure_id) + '|'
 FROM mc_applicable_laws ma
 WHERE ma.machine_id = mc.machine_id
 ORDER BY ma.applicable_laws_structure_id
 FOR XML PATH('')),' ','') AS applicable_laws_structure_id,
dbo.get_file_download_info(1600,mc.machine_id) AS file_link_machine,
dbo.get_file_download_info(1610,eq.equipment_id) AS file_link_equip 
