INSERT 
INTO #rf_read_date( 
    rftag_id
    , parts_no
    , department_structure_id
    , account_structure_id
    , read_datetime
) 
VALUES ( 
    @RftagId
    , @PartsNo
    , @DepartmentStructureId
    , @AccountStructureId
    , @ReadDatetime
)
