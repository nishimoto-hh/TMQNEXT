SELECT DISTINCT
    lot.parts_id,
    lot.old_new_structure_id,
    lot.department_structure_id,
    stock.parts_location_id,
    stock.parts_location_detail_no,
    lot.account_structure_id
FROM
    pt_lot lot
    LEFT JOIN
        pt_location_stock stock
    ON  lot.lot_control_id = stock.lot_control_id
WHERE
    lot.lot_control_id = @LotControlId