-- 機器添付有無用
/*@FileLinkEquip
DROP TABLE IF EXISTS #eq_att; 
CREATE TABLE #eq_att(
    long_plan_id bigint
    ,machine_id bigint
    ,equipment_id bigint
);
@FileLinkEquip*/
DROP TABLE IF EXISTS #max_dt; 
CREATE TABLE #max_dt(
    long_plan_id_dt bigint
    ,mc_man_st_con_update_datetime datetime
    ,sche_detail_update_datetime datetime
    ,attachment_update_datetime datetime
);
-- 準備対象用
DROP TABLE IF EXISTS #prepare_target_narrow; 
CREATE TABLE #prepare_target_narrow(
    long_plan_id bigint
);