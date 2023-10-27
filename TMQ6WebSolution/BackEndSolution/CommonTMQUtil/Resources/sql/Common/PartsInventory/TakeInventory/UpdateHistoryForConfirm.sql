/*
* 棚卸 確定 棚卸確定日時で受払履歴を更新
* UpdateHistoryForConfirm.sql
*/
UPDATE
    pt_inout_history
SET
     inventory_datetime = @InoutDatetime
    ,update_serialid = update_serialid + 1
    ,update_datetime = @UpdateDatetime
    ,update_user_id = @UpdateUserId
WHERE
    inventory_datetime IS NULL
AND inout_datetime <= @InoutDatetime
AND delete_flg = 0