SELECT DISTINCT
    lotInfo.parts_id,
    lotInfo.old_new_structure_id,
    lotInfo.department_structure_id,
    lotInfo.account_structure_id,
    stock.parts_location_id,
    stock.parts_location_detail_no
FROM
    pt_location_stock stock
    INNER JOIN
        (
            SELECT
                lot.lot_control_id,
                lot.parts_id,
                lot.old_new_structure_id,
                lot.department_structure_id,
                lot.account_structure_id
            FROM
                pt_lot lot
            WHERE
                lot.parts_id = @PartsId
            AND lot.old_new_structure_id = @OldNewStructureId
            AND lot.department_structure_id = @DepartmentStructureId
            AND lot.account_structure_id = @AccountStructureId
        ) lotInfo
    ON  stock.lot_control_id = lotinfo.lot_control_id