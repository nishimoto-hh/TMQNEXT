select
    *
from
    workflow_header
where
    wf_division = /*WfDivision*/'0'
and del_flg = 0
order by
    active_date
limit 1
