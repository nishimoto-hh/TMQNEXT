select *
from com_bat_sch
where conductid = /*ConductId*/
order by (exec_date || exec_time) desc, status desc limit 1 -- 最新の１件

