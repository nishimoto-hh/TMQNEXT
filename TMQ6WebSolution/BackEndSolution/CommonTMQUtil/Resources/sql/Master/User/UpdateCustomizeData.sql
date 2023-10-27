UPDATE
    cm_control_user_customize
SET
    display_flg = 1
WHERE
    user_id = @UserId
AND program_id = @ProgramId
AND form_no = @FormNo
AND control_group_id = @ControlGroupId
AND control_no IN @ControlNoList