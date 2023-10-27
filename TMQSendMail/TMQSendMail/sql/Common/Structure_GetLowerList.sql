WITH rec(structure_layer_no, structure_id, parent_structure_id) AS (
SELECT structure_layer_no,  structure_id, parent_structure_id 
FROM ms_structure
WHERE structure_id in /*StructureIdList*/(11)
AND delete_flg = 0
UNION ALL
SELECT b.structure_layer_no,  b.structure_id, b.parent_structure_id
FROM rec a,
ms_structure b
WHERE a.structure_id = b.parent_structure_id
AND b.delete_flg = 0)
SELECT structure_id FROM rec
