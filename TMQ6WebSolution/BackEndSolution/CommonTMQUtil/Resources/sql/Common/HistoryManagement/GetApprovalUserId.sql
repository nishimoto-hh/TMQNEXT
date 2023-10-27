SELECT
    ex.extension_data AS ExData 
FROM
    ms_structure ms 
    LEFT JOIN ms_item_extension ex 
        ON ms.structure_item_id = ex.item_id 
        AND ex.sequence_no = 4 
WHERE
    ms.structure_group_id = 1000 
    AND ms.structure_layer_no = 1 
    AND EXISTS ( 
        SELECT
            1 
        FROM
            hm_history_management history 
            INNER JOIN STRING_SPLIT(@HistoryManagementIdList, ',') id_list 
                ON history.history_management_id = id_list.value 
        WHERE
            ms.structure_id = history.factory_id
    )
