SELECT
    ex.extension_data AS application_status_code_from_machine_id
FROM
    hm_history_management history
    LEFT JOIN
        hm_mc_machine hmachine
    ON  history.history_management_id = hmachine.history_management_id
    LEFT JOIN
        hm_mc_management_standards_component component
    ON  history.history_management_id = component.history_management_id
    LEFT JOIN
        ms_structure ms
    ON  history.application_status_id = ms.structure_id
    AND ms.structure_group_id = 2090
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
    AND ex.sequence_no = 1
WHERE
    (
        hmachine.machine_id = @MachineId
    OR  component.machine_id = @MachineId
    )
AND ex.extension_data IN('20', '30')