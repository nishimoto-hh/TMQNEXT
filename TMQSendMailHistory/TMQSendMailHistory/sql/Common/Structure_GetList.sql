SELECT
     vs.structure_id AS structureId
    ,vs.factory_id AS factoryId
    ,vs.structure_group_id AS structureGroupId
    ,vs.structure_layer_no AS structureLayerNo
    ,vs.parent_structure_id AS parentStructureId
    ,vs.structure_item_id AS structureItemId
    ,vs.translation_text AS translationText
FROM
    v_structure_item vs
    
WHERE
    vs.language_id = /*LanguageId*/'ja'

	/*IF FactoryIdList.Count!=0 */
    and vs.factory_id IN /*FactoryIdList*/(0)
	/*END*/
	/*IF StructureGroupIdList.Count!=0 */
    and vs.structure_group_id IN /*StructureGroupIdList*/(1000)
	/*END*/	

UNION ALL

SELECT
     vs.structure_id AS structureId
    ,vs.factory_id AS factoryId
    ,ie.extension_data AS structureGroupId
    ,vs.structure_layer_no AS structureLayerNo
    ,vs.parent_structure_id AS parentStructureId
    ,vs.structure_item_id AS structureItemId
    ,vs.translation_text AS translationText
FROM
    v_structure_item vs
LEFT JOIN
    (SELECT 
         item_id
        ,CASE WHEN ISNUMERIC(extension_data) = 1 THEN CAST(extension_data AS int) ELSE NULL END AS extension_data
     FROM ms_item_extension
     WHERE sequence_no = 1
    ) ie
ON vs.structure_item_id = ie.item_id
  
WHERE
    vs.language_id = /*LanguageId*/'ja'

	/*IF FactoryIdList.Count!=0 */
    and vs.factory_id IN /*FactoryIdList*/(0)
	/*END*/
	/*IF StructureGroupIdList.Count!=0 */
    and ie.extension_data IN /*StructureGroupIdList*/(1000)
	/*END*/	

ORDER BY structure_group_id, factory_id, structure_layer_no, structureId
