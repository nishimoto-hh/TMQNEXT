/*
* 受払履歴の削除
*/
DELETE
    pt_inout_history
WHERE
    work_no = @WorkNo
AND delete_flg = 0