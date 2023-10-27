SELECT 
    ie.extension_data AS exparam1
    ,ms.structure_id as [values]
    ,ms.structure_group_id as [labels]

FROM 
    ms_structure ms
INNER JOIN 
    ms_item_extension ie
ON  ms.structure_item_id = ie.item_id

WHERE
    ms.structure_group_id = 9210
AND ie.sequence_no = 1