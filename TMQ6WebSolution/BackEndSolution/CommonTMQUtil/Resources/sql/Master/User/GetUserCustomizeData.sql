SELECT
    @UserId AS user_id,
    cfcd.program_id,
    cfcd.form_no,
    cfcd.control_group_id,
    cfcd.control_no,
    1 AS data_division,
    0 AS display_flg,
    cfcd.control_no AS display_order,
    0 AS delete_flg,
    SYSDATETIME() AS insert_datetime,
    @UserId AS insert_user_id,
    SYSDATETIME() AS update_datetime,
    @UserId AS update_user_id
FROM
    cm_form_control_define cfcd
WHERE
    cfcd.program_id = @ProgramId
AND cfcd.control_group_id = @ControlGroupId
AND cfcd.form_no = @FormNo
AND define_type = 1
AND (
        control_customize_flg = 1
    OR  control_customize_flg IS NULL
    )
AND display_division <> - 1
AND display_division <> 2