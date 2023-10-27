DELETE 
FROM cm_control_user_customize
WHERE
    user_id = /*UserId*/0
AND program_id = /*ProgramId*/'MC0001'
AND form_no = /*FormNo*/0
AND control_group_id = /*ControlGroupId*/''
/*IF DataDivision == 0*/ 
AND data_division = /*DataDivision*/0
/*END*/
