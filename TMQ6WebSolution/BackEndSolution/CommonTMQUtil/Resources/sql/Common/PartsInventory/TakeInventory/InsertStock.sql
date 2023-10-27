/*
* 棚卸 棚卸、棚差調整データより在庫データを登録
* InsertStock.sql
*/
INSERT INTO pt_location_stock(
     inventory_control_id
    ,parts_id
    ,lot_control_id
    ,parts_location_id
    ,parts_location_detail_no
    ,stock_quantity
    ,update_serialid
    ,insert_datetime
    ,insert_user_id
    ,update_datetime
    ,update_user_id
) output inserted.inventory_control_id
SELECT
     NEXT VALUE FOR seq_pt_location_stock_inventory_control_id
    ,inv.parts_id
    ,@LotControlId
    ,inv.parts_location_id
    ,inv.parts_location_detail_no
    ,dif.inout_quantity
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