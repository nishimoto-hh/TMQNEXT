/*
* 棚卸 棚卸、棚差調整データよりロット情報を登録
* InsertLot.sql
*/
INSERT INTO pt_lot(
     lot_control_id
    ,parts_id
    ,lot_no
    ,old_new_structure_id
    ,department_structure_id
    ,account_structure_id
    ,receiving_datetime
    ,unit_structure_id
    ,unit_price
    ,currency_structure_id
    ,update_serialid
    ,insert_datetime
    ,insert_user_id
    ,update_datetime
    ,update_user_id
) output inserted.lot_control_id
SELECT
     NEXT VALUE FOR seq_pt_lot_lot_control_id
    ,inv.parts_id
    ,NEXT VALUE FOR seq_pt_lot_lot_no
    ,inv.old_new_structure_id
    ,inv.department_structure_id
    ,inv.account_structure_id
    ,@InoutDatetime
    ,inv.unit_structure_id
    ,dif.unit_price
    ,inv.currency_structure_id
    ,@UpdateSerialid
    ,@InsertDatetime
    ,@InsertUserId
    ,@UpdateDatetime
    ,@UpdateUserId
FROM
    pt_inventory AS inv
    INNER JOIN
        pt_inventory_difference AS dif
    ON  (
            inv.inventory_id = dif.inventory_id
        )
WHERE
    inv.inventory_id = @InventoryId
AND dif.inventory_difference_id = @InventoryDifferenceId