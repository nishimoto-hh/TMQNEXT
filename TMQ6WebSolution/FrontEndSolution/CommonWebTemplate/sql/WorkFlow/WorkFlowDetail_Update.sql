/*
 * ワークフロー詳細更新処理
 */

update workflow_detail
   set all_approval_flg = /*AllApprovalFlg*/'%',
       status = /*Status*/'%',
       approval_date = /*ApprovalDate*/'%',
       comments = /*Comments*/'%',
       input_date = /*InputDate*/'%',
       inputor_cd = /*InputUserId*/'%',
       update_date = /*UpdateDate*/'%',
       updator_cd = /*UpdateUserId*/'%'
 where wf_no = /*WfNo*/
   and seq = /*Seq*/'%'
   and approval_user_id = /*ApprovalUserId*/'%'
