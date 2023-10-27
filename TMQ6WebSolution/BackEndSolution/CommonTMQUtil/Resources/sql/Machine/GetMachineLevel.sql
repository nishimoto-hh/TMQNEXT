SELECT ie.extension_data
from (SELECT structure_id, structure_item_id FROM v_structure_item_all v WHERE structure_group_id = 1170 AND language_id = @LanguageId) v,
	 ms_item_extension ie
WHERE v.structure_id = @MachineLevel
AND v.structure_item_id = ie.item_id
AND ie.sequence_no = 1