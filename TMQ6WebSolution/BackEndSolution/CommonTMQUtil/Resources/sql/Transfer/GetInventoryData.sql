SELECT
    COUNT(inventory.inventory_id)
FROM
    pt_inventory inventory
    INNER JOIN
        (
            SELECT
                stock.parts_location_id,
                stock.parts_location_detail_no,
                lot.old_new_structure_id,
                lot.department_structure_id,
                lot.account_structure_id,
                history.update_datetime,
                history.inout_datetime
            FROM
                pt_inout_history history
                LEFT JOIN
                    pt_location_stock stock
                ON  history.inventory_control_id = stock.inventory_control_id
                LEFT JOIN
                    pt_lot lot
                ON  history.lot_control_id = lot.lot_control_id
            WHERE
                history.work_no = @WorkNo
        ) history
    ON  inventory.parts_id = @PartsId
    AND inventory.parts_location_id = history.parts_location_id
    AND inventory.parts_location_detail_no = history.parts_location_detail_no
    AND inventory.old_new_structure_id = history.old_new_structure_id
    AND inventory.department_structure_id = history.department_structure_id
    AND inventory.account_structure_id = history.account_structure_id
WHERE
    history.update_datetime <= inventory.preparation_datetime
AND inventory.fixed_datetime IS NULL
AND history.inout_datetime <= EOMONTH(inventory.target_month)