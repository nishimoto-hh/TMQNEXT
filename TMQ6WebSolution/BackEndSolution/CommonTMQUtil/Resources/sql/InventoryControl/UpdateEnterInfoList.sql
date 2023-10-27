--******************************************************************
--受払履歴 取り消しSQL(論理削除)
--******************************************************************
UPDATE 
pt_inout_history
SET
update_serialid = update_serialid + 1
,delete_flg = 1
,update_datetime = @UpdateDatetime           --更新日時
,update_user_id = @UpdateUserId              --更新ユーザ
WHERE
    lot_control_id = ( 
        SELECT
            lot_control_id 
        FROM
            pt_inout_history 
        WHERE
            work_id = @WorkId
    )
