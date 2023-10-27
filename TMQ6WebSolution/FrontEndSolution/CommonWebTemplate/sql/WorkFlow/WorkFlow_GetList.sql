select
/*IF IsCount == False */
    * 
    --ELSE count(*)
/*END*/
from
    ( 
        select
            wf_header.wf_template_no
            , wf_detail.approval_user_id
            , wf_division
            , wf_name
            , wf_header.notice_division
            , request_push_flg
            , request_mail_flg
            , request_cancel_push_flg
            , request_cancel_mail_flg
            , approval_push_flg
            , approval_mail_flg
            , approval_cancel_push_flg
            , approval_cancel_mail_flg
            , reject_push_flg
            , reject_mail_flg
            , complete_push_flg
            , complete_mail_flg
            , editable_flg
            , wf_header.active_date
            , wf_header.del_flg
            , wf_detail.seq
            , row_number() over (partition by wf_header.wf_template_no) as row_num
            , vn.name01 as division_name
            , vn2.name01 as status_name
            , vn3.name01 as notice_name
            , vn4.name01 as editable_name
            
        from
            workflow_template_header as wf_header 
            left join workflow_template_detail as wf_detail 
                on wf_header.wf_template_no ::integer = wf_detail.wf_template_no ::integer 
            left join v_names vn 
                on wf_header.wf_division = vn.name_cd 
                and vn.name_division = 'WFDV' 
            left join v_names vn2 
                on wf_header.del_flg = vn2.name_cd ::numeric 
                and vn2.name_division = 'WFST' 
            left join v_names vn3 
                on wf_header.notice_division = vn3.name_cd ::numeric 
                and vn3.name_division = 'WFND' 
            left join v_names vn4 
                on wf_header.editable_flg = vn4.name_cd ::numeric 
                and vn4.name_division = 'WFED' 
        where
        0 = 0
        /*IF ApprovalUserId != null && ApprovalUserId != ''*/
            and wf_detail.approval_user_id like /*ApprovalUserId*/'%test%' 
        /*END*/
    ) temp 
where
0 = 0
/*IF IsCount == False */
    and row_num = 1
    /*END*/
/*IF WfDivision != null && WfDivision != ''*/
    and wf_division like /*WfDivision*/'0' 
/*END*/
/*IF WfName != null && WfName != ''*/
    and wf_name like /*WfName*/'demo' 
/*END*/
/*IF ActiveDate != null && ActiveDate != ''*/
    and active_date <= /*ActiveDate*/'2222/01/01' ::timestamp
    --ELSE and active_date <= current_timestamp
/*END*/
/*IF DelFlg != null && DelFlg != '' && DelFlg != 0*/
    and temp.del_flg = /*DelFlg*/'2' ::numeric 
/*END*/
/*IF IsCount == False */
order by
    wf_template_no 
/*END*/
