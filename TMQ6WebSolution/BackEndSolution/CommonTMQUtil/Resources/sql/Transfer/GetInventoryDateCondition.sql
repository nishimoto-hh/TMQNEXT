SELECT DISTINCT
    lot.parts_id,
    lot.old_new_structure_id,
    lot.department_structure_id,
    stock.parts_location_id,
    stock.parts_location_detail_no,
    lot.account_structure_id
FROM
    pt_inout_history history
    LEFT JOIN
        pt_lot lot
    ON  history.lot_control_id = lot.lot_control_id
    LEFT JOIN
        pt_location_stock stock
    ON  history.inventory_control_id = stock.inventory_control_id
    LEFT JOIN
        inout_div
    ON  history.inout_division_structure_id = inout_div.inout_id
WHERE
    history.work_no = @WorkNo
AND inout_div.inout_code = '1'