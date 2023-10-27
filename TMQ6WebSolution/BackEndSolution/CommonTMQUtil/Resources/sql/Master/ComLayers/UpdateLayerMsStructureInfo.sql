UPDATE ms_structure
SET delete_flg = 1
   ,update_serialid = update_serialid + 1
   ,update_datetime = @UpdateDatetime
   ,update_user_id = @UpdateUserId
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
