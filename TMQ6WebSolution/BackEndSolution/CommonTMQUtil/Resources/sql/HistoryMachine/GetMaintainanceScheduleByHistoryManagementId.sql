SELECT
    schedule.hm_maintainance_schedule_id
FROM
    hm_mc_maintainance_schedule schedule
WHERE
    schedule.history_management_id = @HistoryManagementId