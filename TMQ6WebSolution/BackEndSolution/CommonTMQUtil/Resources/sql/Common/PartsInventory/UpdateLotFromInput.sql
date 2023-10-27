/*
* ロット情報更新SQL(入庫入力)
* UpdateLotFromInput.sql
*/
UPDATE
    lot
SET
     lot.old_new_structure_id = @OldNewStructureId
    ,lot.department_structure_id = @DepartmentStructureId
    ,lot.account_structure_id = @AccountStructureId
    ,lot.management_division = @ManagementDivision
    ,lot.management_no = @ManagementNo
    ,lot.receiving_datetime = @ReceivingDatetime
    ,lot.unit_structure_id = parts.unit_structure_id
    ,lot.unit_price = @UnitPrice
    ,lot.currency_structure_id = parts.currency_structure_id
    ,lot.vender_structure_id = @VenderStructureId
    ,lot.update_serialid = lot.update_serialid + 1
    ,lot.update_datetime = @UpdateDatetime
    ,lot.update_user_id = @UpdateUserId
FROM
     pt_lot AS lot
    INNER JOIN
        pt_parts AS parts
    ON  (
            lot.parts_id = parts.parts_id
        )
WHERE
    lot.lot_control_id = @LotControlId
