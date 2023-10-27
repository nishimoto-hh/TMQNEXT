SELECT
    SUM(inout_quantity)
FROM
    pt_inventory_difference 
WHERE
    inventory_id = @InventoryId
    AND inout_division_structure_id = @InoutDivisionStructureId