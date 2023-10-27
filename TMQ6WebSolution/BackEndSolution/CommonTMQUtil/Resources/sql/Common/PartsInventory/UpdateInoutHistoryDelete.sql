/*
* 受払履歴の取消
*/
UPDATE
    pt_inout_history
SET
     delete_flg = 1
    ,update_serialid = update_serialid + 1
    ,update_datetime = @UpdateDatetime
    ,update_user_id = @UpdateUserId
WHERE
    work_no = @WorkNo
AND delete_flg = 0