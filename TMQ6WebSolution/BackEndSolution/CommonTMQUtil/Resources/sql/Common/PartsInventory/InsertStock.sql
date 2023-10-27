/*
* 在庫データ登録
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
VALUES(
     NEXT VALUE FOR seq_pt_location_stock_inventory_control_id
    ,@PartsId
    ,@LotControlId
    ,@PartsLocationId
    ,@PartsLocationDetailNo
    ,@InoutQuantity
    ,@UpdateSerialid
    ,@InsertDatetime
    ,@InsertUserId
    ,@UpdateDatetime
    ,@UpdateUserId
)