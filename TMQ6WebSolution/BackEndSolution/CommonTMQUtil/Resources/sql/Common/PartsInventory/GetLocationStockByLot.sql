/*
* 在庫データ取得(部門移庫入力)
* GetLocationStockByLot.sql
* 予備品ID＋ロット管理IDで在庫データを取得
*/
SELECT
    st.*
FROM
    pt_location_stock AS st
WHERE
    st.delete_flg = 0
AND st.stock_quantity > 0
AND EXISTS(
        SELECT
            *
        FROM
            pt_lot AS lt
        WHERE
            lt.lot_control_id = @LotControlId
        AND lt.parts_id = st.parts_id
        AND lt.lot_control_id = st.lot_control_id
    )