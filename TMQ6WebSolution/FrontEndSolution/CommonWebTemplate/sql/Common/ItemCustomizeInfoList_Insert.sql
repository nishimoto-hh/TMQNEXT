INSERT 
INTO cm_control_user_customize(
      [user_id]
    , [program_id]
    , [form_no]
    , [control_group_id]
    , [control_no]
    , [data_division]
    , [display_flg]
    , [display_order]
    , [delete_flg]
    , [insert_datetime]
    , [insert_user_id] 
    , [update_datetime]
    , [update_user_id] 
) 
VALUES ( 
      /*UserId*/0
    , /*ProgramId*/'MC0001'
    , /*FormNo*/0
    , /*ControlGroupId*/''
    , /*ControlNo*/1
    , /*DataDivision*/0
    , /*DisplayFlg*/1  
    , /*DisplayOrder*/1
    , 0
    , GETDATE()
    , /*UserId*/0
    , GETDATE()
    , /*UserId*/0
)
