UPDATE
    ms_structure
SET
    delete_flg = 1,
	update_serialid = update_serialid + 1,
	update_datetime = @UpdateDatetime,
	update_user_id = @UpdateUserId
WHERE
    structure_id IN ( 
        SELECT
            st.structure_id 
        FROM
            ms_structure AS st 
        WHERE
            (st.structure_group_id = @StructureGroupId
            AND parent_structure_id = @StructureId
            )
            OR structure_id = @StructureId
    )

