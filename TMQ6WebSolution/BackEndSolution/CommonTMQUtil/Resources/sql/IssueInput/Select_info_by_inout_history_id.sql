--*****************************************************
--受払履歴IDよりロット管理ID、受払日時、更新日時を取得
--*****************************************************
SELECT
    lot_control_id,
    inout_datetime,
    update_datetime
FROM
    pt_inout_history 
WHERE
    inout_history_id = @InoutHistoryId
