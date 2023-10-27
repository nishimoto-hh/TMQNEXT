SELECT
    ex.extension_data AS ex_data
FROM
    hm_history_management history
    LEFT JOIN
        ms_structure ms
    ON  history.application_status_id = ms.structure_id
    AND ms.structure_group_id = 2090
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
    AND ex.sequence_no = 1
WHERE
    history.history_management_id = @HistoryManagementId