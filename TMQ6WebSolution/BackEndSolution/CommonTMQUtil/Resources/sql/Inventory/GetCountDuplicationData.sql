WITH duplication_count AS ( 
    SELECT
        COUNT(pl.lot_control_id) AS data_count 
    FROM
        pt_lot pl 
        LEFT JOIN pt_location_stock pls 
            ON pl.lot_control_id = pls.lot_control_id
    WHERE
        pl.parts_id = @PartsId 
        /* 予備品id */
        AND pls.parts_location_id = @PartsLocationId 
        /* 棚id */
        AND pls.parts_location_detail_no = @PartsLocationDetailNo 
        /* 棚枝番 */
        AND pl.old_new_structure_id = @OldNewStructureId 
        /* 新旧区分id */
        AND pl.department_structure_id = @DepartmentStructureId 
        /* 部門id */
        AND pl.account_structure_id = @AccountStructureId 
    UNION ALL
    SELECT
        COUNT(pi.inventory_id) AS data_count 
    FROM
        pt_inventory pi 
    WHERE
        pi.parts_id = @PartsId 
        /* 予備品id */
        AND pi.parts_location_id = @PartsLocationId 
        /* 棚id */
        AND pi.parts_location_detail_no = @PartsLocationDetailNo 
        /* 棚枝番 */
        AND pi.old_new_structure_id = @OldNewStructureId 
        /* 新旧区分id */
        AND pi.department_structure_id = @DepartmentStructureId 
        /* 部門id */
        AND pi.account_structure_id = @AccountStructureId
) 
SELECT
    SUM(data_count) 
FROM
    duplication_count