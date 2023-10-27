SELECT ie.extension_data
from ms_structure s,
	 ms_item_extension ie
WHERE s.structure_id = @MachineLevel
AND s.structure_item_id = ie.item_id
AND ie.sequence_no = 1