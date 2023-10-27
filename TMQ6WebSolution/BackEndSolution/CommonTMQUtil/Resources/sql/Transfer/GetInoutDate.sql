SELECT
    history.inout_datetime
FROM
    pt_inout_history history
    LEFT JOIN
        inout_div
    ON  history.inout_division_structure_id = inout_div.inout_id
WHERE
    history.work_no = @WorkNo
AND inout_div.inout_code = '2'