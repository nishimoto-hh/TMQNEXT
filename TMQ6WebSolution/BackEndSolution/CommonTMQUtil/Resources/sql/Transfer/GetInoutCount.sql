SELECT
    COUNT(history.inout_history_id)
FROM
    pt_inout_history history
    INNER JOIN
        (
            SELECT
                history.inventory_control_id,
                history.inout_datetime
            FROM
                pt_inout_history history
                LEFT JOIN
                    inout_div
                ON  history.inout_division_structure_id = inout_div.inout_id
            WHERE
                history.work_no = @WorkNo
            AND inout_div.inout_code = '1'
        ) history_data
    ON  history.inventory_control_id = history_data.inventory_control_id
WHERE
    history.inout_datetime >= history_Data.inout_datetime
AND history.work_no != @WorkNo
