--******************************************************************
--新旧区分、部門、勘定科目より棚ID、棚枝番を取得
--******************************************************************
SELECT 
    parts_location_id
    , parts_location_detail_no 
FROM
    pt_location_stock 
WHERE
    lot_control_id IN ( 
        SELECT
            lot_control_id 
        FROM
            pt_lot 
        WHERE
            old_new_structure_id = @OldNewStructureId
            AND department_structure_id = @DepartmentStructureId
            AND account_structure_id = @AccountStructureId
    )
