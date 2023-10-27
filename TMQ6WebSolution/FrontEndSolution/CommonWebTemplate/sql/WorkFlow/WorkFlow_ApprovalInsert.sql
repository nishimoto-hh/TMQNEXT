insert into workflow_template_detail( 
    wf_template_no
    , active_date
    , seq
    , approval_user_id
    , all_approval_flg
    , input_date
    , input_user_id
    , update_date
    , update_user_id
) 
values ( 
    /*WfTemplateNo*/ ::numeric 
    , /*ActiveDate*/ ::timestamp
    , /*Seq*/ ::numeric 
    , /*ApprovalUserId*/
    , /*AllApprovalFlg*/ ::numeric 
    , /*InputDate*/
    , /*InputUserId*/
    , /*UpdateDate*/
    , /*UpdateUserId*/
) returning wf_template_no

