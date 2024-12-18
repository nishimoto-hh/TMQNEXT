-- COM_CONDUCT_MST
SELECT
     conduct_id AS CONDUCTID 
    ,conduct_group_id AS CONDUCTGRP 
    ,conduct_name AS NAME 
    ,conduct_name_ryaku AS RYAKU 
    ,process_pattern AS PTN 
    ,menu_order AS MENUORDER 
    ,menu_division AS MENUDISP 
    ,program_id AS PGMID 
    ,start_up_parameters AS BOOTPARAM 
    ,common_conduct_id AS CM_CONDUCTID 
    ,version AS VERSION 
    ,update_information AS CHANGELOG 
FROM cm_conduct
WHERE
    delete_flg != 1;
