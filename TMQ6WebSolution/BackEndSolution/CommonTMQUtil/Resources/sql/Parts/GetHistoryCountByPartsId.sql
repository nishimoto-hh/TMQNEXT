SELECT
    COUNT(lot.parts_id) AS cnt
FROM
    pt_inout_history history
    LEFT JOIN
        pt_lot lot
    ON  history.lot_control_id = lot.lot_control_id
WHERE
    lot.parts_id = @PartsId
