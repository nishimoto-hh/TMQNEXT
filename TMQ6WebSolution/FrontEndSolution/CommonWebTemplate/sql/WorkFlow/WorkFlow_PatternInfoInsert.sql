insert into workflow_template_header( 

      active_date
    , wf_name
    , wf_division
    , notice_division
    
    ,request_push_flg
    ,request_mail_flg
    ,request_cancel_push_flg
    ,request_cancel_mail_flg
    ,approval_push_flg
    ,approval_mail_flg
    ,approval_cancel_push_flg
    ,approval_cancel_mail_flg
    ,reject_push_flg
    ,reject_mail_flg
    ,complete_push_flg
    ,complete_mail_flg
    
    , editable_flg
    , input_date
    , input_user_id
    , update_date
    , update_user_id
    , del_flg
) 
values ( 

      /*ActiveDate*/ ::timestamp
    , /*WfName*/
    , /*WfDivision*/
    , /*NoticeDivision*/ ::numeric
    
    , /*RequestPushFlg*/ ::numeric
    , /*RequestMailFlg*/ ::numeric
    , /*RequestCancelPushFlg*/ ::numeric
    , /*RequestCancelMailFlg*/ ::numeric
    , /*ApprovalPushFlg*/ ::numeric
    , /*ApprovalMailFlg*/ ::numeric
    , /*ApprovalCancelPushFlg*/ ::numeric
    , /*ApprovalCancelMailFlg*/ ::numeric
    , /*RejectPushFlg*/ ::numeric
    , /*RejectMailFlg*/ ::numeric
    , /*CompletePushFlg*/ ::numeric
    , /*CompleteMailFlg*/ ::numeric

    , /*EditableFlg*/ ::numeric
    , /*InputDate*/
    , /*InputUserId*/
    , /*UpdateDate*/
    , /*UpdateUserId*/
    , /*DelFlg*/ ::numeric
) returning wf_template_no
