/*
 * ワークフローNO取得SQL
 */

select *
  from workflow_header
 where wf_no = (select max(wf_no)
                  from workflow_header
                 where wf_division = /*WfDivision*/'%'
                   and slip_no = /*SlipNo*/'%'
/*IF SlipBranchNo1 != null && SlipBranchNo1 != ''*/
                   and slip_branch_no1 = /*SlipBranchNo1*/
/*END*/
/*IF SlipBranchNo2 != null && SlipBranchNo2 != ''*/
                   and slip_branch_no2 = /*SlipBranchNo2*/
/*END*/
                   and del_flg = 0
               )
