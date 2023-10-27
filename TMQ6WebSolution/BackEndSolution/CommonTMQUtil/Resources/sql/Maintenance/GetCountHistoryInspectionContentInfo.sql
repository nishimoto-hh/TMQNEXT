SELECT
    COUNT(history_inspection_content_id)
FROM
    ma_history_inspection_content
WHERE
    history_inspection_site_id = @HistoryInspectionSiteId
