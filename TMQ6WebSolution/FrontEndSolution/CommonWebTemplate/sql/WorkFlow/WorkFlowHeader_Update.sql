/*
 * ワークフローヘッダ更新処理
 */

update workflow_header
   set wf_template_no = /*WfTemplateNo*/'%',
       active_date = /*ActiveDate*/'%',
       wf_name = /*WfName*/'%',
       wf_division = /*WfDivision*/'%',
       slip_no = /*SlipNo*/'%',
       slip_branch_no1 = /*SlipBranchNo1*/'%',
       slip_branch_no2 = /*SlipBranchNo2*/'%',
       request_user_cd = /*RequestUserCd*/'%',
       notice_division = /*NoticeDivision*/'%',
       request_comments = /*RequestComments*/'%',
       status = /*Status*/'%',
       input_date = /*InputDate*/'%',
       inputor_cd = /*InputUserId*/'%',
       update_date = /*UpdateDate*/'%',
       updator_cd = /*UpdateUserId*/'%',
       del_flg = /*DelFlg*/'%'
 where wf_no = /*WfNo*/
