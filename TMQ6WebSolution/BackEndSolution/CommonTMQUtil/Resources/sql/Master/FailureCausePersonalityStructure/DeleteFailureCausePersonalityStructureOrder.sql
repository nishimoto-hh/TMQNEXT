DELETE 
FROM
    ms_structure_order 
WHERE
    structure_group_id = @StructureGroupId 
    AND factory_id = @FactoryId
    AND structure_id IN ( 
        SELECT
            st.structure_id 
        FROM
            ms_structure AS st 
        WHERE
            st.structure_group_id = @StructureGroupId
            AND structure_layer_no = @StructureLayerNo
            AND parent_structure_id = @ParentStructureId
    )
