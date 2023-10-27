SELECT
    COUNT(maintainance_schedule_detail_id)
FROM
    mc_maintainance_schedule_detail
WHERE
    summary_id = @SummaryId
