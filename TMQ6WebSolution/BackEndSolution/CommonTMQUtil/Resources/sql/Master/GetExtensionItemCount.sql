SELECT
    COUNT(*) AS VAL
FROM
    ms_structure AS st 
    INNER JOIN ms_item_extension AS ie 
        ON st.structure_item_id = ie.item_id
        AND ie.extension_data = @ExData1
WHERE st.structure_group_id = @StructureGroupId
  AND ie.sequence_no = 1
  AND st.factory_id = @FactoryId
  AND st.structure_id <> @StructureId
