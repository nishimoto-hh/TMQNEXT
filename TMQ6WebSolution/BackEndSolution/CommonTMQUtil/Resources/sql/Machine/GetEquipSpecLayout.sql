 SELECT msr.machine_spec_relation_id,msr.location_structure_id,msr.job_structure_id,msr.spec_id,msr.display_order,
       ms.spec_type_id,ms.spec_unit_type_id,ms.spec_unit_id,ms.spec_num_decimal_places,ms.translation_id,
	   mt.translation_text,
	   --vi.translation_text,
	   ie.extension_data,
	   mt.location_structure_id
FROM ms_machine_spec_relation msr,
     ms_spec ms,
	 ms_translation mt,
	 (SELECT 
	   max(mt.location_structure_id) location_structure_id,
	   ms.translation_id
	   FROM ms_spec ms
	   left join ms_translation mt
	   on ms.translation_id = mt.translation_id
	   where  mt.location_structure_id in (0,@FactoryId)
	   group by ms.translation_id
	   ) mts,
	 v_structure_all vi,
     ms_item_extension ie
WHERE msr.spec_id = ms.spec_id
AND ms.translation_id = mt.translation_id
AND ms.translation_id = mts.translation_id
AND mt.translation_id = mts.translation_id
AND mt.location_structure_id = mts.location_structure_id
AND ms.spec_type_id = vi.structure_id
AND vi.structure_item_id = ie.item_id
AND mt.language_id = @LanguageId
AND ie.sequence_no = 1
AND msr.location_structure_id = @FactoryId -- 工場ID
--AND (msr.job_structure_id IN (@JobId)      -- 職種ID
--     OR msr.job_structure_id IS NULL)
--ORDER BY msr.job_structure_id,msr.display_order
 
