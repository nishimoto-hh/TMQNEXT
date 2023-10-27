/*
* 在庫データ取得(棚番移庫入力)
* GetLocationStockByLocation.sql
* 予備品ID＋ロット管理ID＋棚ID+棚番情報で在庫データを取得
*/
SELECT
    st.*
FROM
    pt_location_stock AS st
WHERE
    st.parts_location_id = @PartsLocationId
AND st.parts_location_detail_no = @PartsLocationDetailNo
AND st.delete_flg = 0
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