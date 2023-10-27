SELECT
    pl.lot_control_id
    , pls.inventory_control_id
    , pl.department_structure_id
    , pl.account_structure_id
    , pl.management_division
    , pl.management_no
    , pl.unit_price
    , pls.stock_quantity 
FROM
    pt_lot pl 
    LEFT JOIN pt_location_stock pls 
        ON pl.lot_control_id = pls.lot_control_id 
WHERE
    pl.parts_id = @PartsId 
    AND pl.old_new_structure_id = @OldNewStructureId 
    AND pl.department_structure_id = @DepartmentStructureId 
    AND pl.account_structure_id = @AccountStructureId 
    AND pls.parts_location_id = @PartsLocationId 
    AND COALESCE(pls.parts_location_detail_no, '') = COALESCE(@PartsLocationDetailNo, '')
ORDER BY
    pl.receiving_datetime ASC
    , pl.lot_no ASC
