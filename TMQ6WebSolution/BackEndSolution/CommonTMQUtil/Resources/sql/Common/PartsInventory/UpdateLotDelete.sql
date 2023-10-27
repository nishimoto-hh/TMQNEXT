/*
* ロット情報削除SQL(論理削除なのでUpdate)
* UpdateLotDelete.sql
*/
UPDATE
    pt_lot
SET
     update_serialid = update_serialid + 1
    ,update_datetime = @UpdateDatetime
    ,update_user_id = @UpdateUserId
    ,delete_flg = 1
WHERE
    lot_control_id = @LotControlId