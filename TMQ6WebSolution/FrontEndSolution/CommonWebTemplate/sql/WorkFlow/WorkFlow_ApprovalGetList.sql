select
/*IF IsCount == False */
*

    --ELSE count(*)
/*END*/
from
    workflow_template_detail as wf_detail 
where
    wf_detail.active_date is not null 

/*IF ActiveDate != null && ActiveDate != ''*/
     and wf_detail.active_date = /*ActiveDate*/ ::timestamp
/*END*/
/*IF WfTemplateNo != null && WfTemplateNo != ''*/
     and wf_detail.wf_template_no = /*WfTemplateNo*/
/*END*/
/*IF IsCount == False */
order by seq asc 
/*END*/

