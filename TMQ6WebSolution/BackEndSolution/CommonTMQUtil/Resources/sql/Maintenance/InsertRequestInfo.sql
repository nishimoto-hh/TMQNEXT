INSERT INTO ma_request(
    [request_id]
    ,[summary_id]
    ,[request_no]
    ,[issue_date]
    ,[urgency_structure_id]
    ,[discovery_methods_structure_id]
    ,[desired_start_date]
    ,[desired_end_date]
    ,[request_content]
    ,[request_department_clerk_id]
    ,[request_personnel_id]
    ,[request_personnel_name]
    ,[request_personnel_tel]
    ,[request_department_chief_id]
    ,[request_department_chief_name]
    ,[request_department_manager_id]
    ,[request_department_manager_name]
    ,[request_department_foreman_id]
    ,[request_department_foreman_name]
    ,[maintenance_department_clerk_id]
    ,[request_reason]
    ,[examination_result]
    ,[construction_division_structure_id]
    ,[request_authorizer1_id]
    ,[request_authorizer2_id]
    ,[request_authorizer3_id]
    ,[update_serialid]
    ,[insert_datetime]
    ,[insert_user_id]
    ,[update_datetime]
    ,[update_user_id]
) OUTPUT inserted.request_id
VALUES(
    NEXT VALUE FOR seq_ma_request_request_id
    ,@SummaryId
    ,@RequestNo
    ,@IssueDate
    ,@UrgencyStructureId
    ,@DiscoveryMethodsStructureId
    ,@DesiredStartDate
    ,@DesiredEndDate
    ,@RequestContent
    ,@RequestDepartmentClerkId
    ,@RequestPersonnelId
    , ( 
        select
            display_name 
        from
            ms_user 
        where
            user_id = @RequestPersonnelId
    )
    ,@RequestPersonnelTel
    ,@RequestDepartmentChiefId
    , ( 
        select
            display_name 
        from
            ms_user 
        where
            user_id = @RequestDepartmentChiefId
    )
    ,@RequestDepartmentManagerId
    , ( 
        select
            display_name 
        from
            ms_user 
        where
            user_id = @RequestDepartmentManagerId
    )
    ,@RequestDepartmentForemanId
    , ( 
        select
            display_name 
        from
            ms_user 
        where
            user_id = @RequestDepartmentForemanId
    )
    ,@MaintenanceDepartmentClerkId
    ,@RequestReason
    ,@ExaminationResult
    ,@ConstructionDivisionStructureId
    ,@RequestAuthorizer1Id
    ,@RequestAuthorizer2Id
    ,@RequestAuthorizer3Id
    ,@UpdateSerialid
    ,@InsertDatetime
    ,@InsertUserId
    ,@UpdateDatetime
    ,@UpdateUserId
)
