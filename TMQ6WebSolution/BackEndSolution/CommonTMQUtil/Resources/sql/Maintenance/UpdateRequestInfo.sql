UPDATE ma_request 
SET
    [issue_date] = @IssueDate
    , [urgency_structure_id] = @UrgencyStructureId
    , [discovery_methods_structure_id] = @DiscoveryMethodsStructureId
    , [desired_start_date] = @DesiredStartDate
    , [desired_end_date] = @DesiredEndDate
    , [request_content] = @RequestContent
    , [request_department_clerk_id] = @RequestDepartmentClerkId
    , [request_personnel_id] = @RequestPersonnelId
    , [request_personnel_name] = CASE 
        WHEN @RequestPersonnelId IS NULL 
            THEN NULL 
        ELSE COALESCE( 
            ( 
                SELECT
                    display_name 
                FROM
                    ms_user 
                WHERE
                    user_id = @RequestPersonnelId
            ) 
            , [request_personnel_name]
        ) 
        END
    , [request_personnel_tel] = @RequestPersonnelTel
    , [request_department_chief_id] = @RequestDepartmentChiefId
    , [request_department_chief_name] = CASE 
        WHEN @RequestDepartmentChiefId IS NULL 
            THEN NULL 
        ELSE COALESCE( 
            ( 
                SELECT
                    display_name 
                FROM
                    ms_user 
                WHERE
                    user_id = @RequestDepartmentChiefId
            ) 
            , [request_department_chief_name]
        ) 
        END
    , [request_department_manager_id] = @RequestDepartmentManagerId
    , [request_department_manager_name] = CASE 
        WHEN @RequestDepartmentManagerId IS NULL 
            THEN NULL 
        ELSE COALESCE( 
            ( 
                SELECT
                    display_name 
                FROM
                    ms_user 
                WHERE
                    user_id = @RequestDepartmentManagerId
            ) 
            , [request_department_manager_name]
        ) 
        END
    , [request_department_foreman_id] = @RequestDepartmentForemanId
    , [request_department_foreman_name] = CASE 
        WHEN @RequestDepartmentForemanId IS NULL 
            THEN NULL 
        ELSE COALESCE( 
            ( 
                SELECT
                    display_name 
                FROM
                    ms_user 
                WHERE
                    user_id = @RequestDepartmentForemanId
            ) 
            , [request_department_foreman_name]
        ) 
        END
    , [maintenance_department_clerk_id] = @MaintenanceDepartmentClerkId
    , [request_reason] = @RequestReason
    , [examination_result] = @ExaminationResult
    , [construction_division_structure_id] = @ConstructionDivisionStructureId
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
WHERE
    [request_id] = @RequestId
