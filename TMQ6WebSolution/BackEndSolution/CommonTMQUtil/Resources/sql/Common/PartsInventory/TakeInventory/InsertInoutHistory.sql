/*
* 棚卸 受払履歴登録
* InsertInoutHistory.sql
*/
INSERT INTO pt_inout_history(
     inout_history_id
    ,inout_division_structure_id
    ,work_division_structure_id
    ,work_no
    ,lot_control_id
    ,inventory_control_id
    ,department_structure_id
    ,account_structure_id
    ,management_division
    ,management_no
    ,inout_datetime
    ,inout_quantity
    ,inventory_datetime
    ,shipping_division_structure_id
    ,update_serialid
    ,insert_datetime
    ,insert_user_id
    ,update_datetime
    ,update_user_id
) output inserted.inout_history_id
SELECT
     NEXT VALUE FOR seq_pt_inout_history_inout_history_id
    ,dif.inout_division_structure_id
    ,dif.work_division_structure_id
    ,@WorkNo
    ,@LotControlId
    ,@InventoryControlId
    ,dif.department_structure_id
    ,dif.account_structure_id
    ,dif.management_division
    ,dif.management_no
    ,@InoutDatetime
    ,dif.inout_quantity
    ,@InsertDatetime
    ,dif.creation_division_structure_id
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