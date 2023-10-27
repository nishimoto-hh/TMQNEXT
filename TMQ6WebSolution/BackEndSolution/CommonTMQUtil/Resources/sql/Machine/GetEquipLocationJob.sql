SELECT
vi.factory_id,
dbo.get_all_layer_id(mc.job_structure_id) as job_structure_id_all
FROM mc_machine mc
LEFT JOIN mc_equipment eq
ON mc.machine_id = eq.machine_id,
     ms_structure vi
WHERE mc.location_structure_id = vi.structure_id
AND eq.equipment_id = @EquipmentId

 
