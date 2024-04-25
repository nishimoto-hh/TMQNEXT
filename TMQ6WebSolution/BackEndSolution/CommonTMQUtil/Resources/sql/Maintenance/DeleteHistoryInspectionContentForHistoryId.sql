DELETE ma_history_inspection_content 
FROM
    ma_history_machine hm 
    INNER JOIN ma_history_inspection_site his 
        ON hm.history_machine_id = his.history_machine_id 
    INNER JOIN ma_history_inspection_content hic 
        ON his.history_inspection_site_id = hic.history_inspection_site_id 
WHERE
    hm.history_id = @HistoryId
