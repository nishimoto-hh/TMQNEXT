SELECT equipment_spec_id
      ,equipment_id
      ,spec_id
      ,spec_value
      ,spec_structure_id
      ,spec_num
      ,spec_num_min
      ,spec_num_max
      ,update_serialid
      ,insert_datetime
      ,insert_user_id
      ,update_datetime
      ,update_user_id
FROM mc_equipment_spec
WHERE equipment_id = @EquipmentId
