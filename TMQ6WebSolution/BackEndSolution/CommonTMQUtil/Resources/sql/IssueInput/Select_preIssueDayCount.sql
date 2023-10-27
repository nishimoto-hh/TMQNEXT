--***************************************************************************************
--受払履歴ID、ロット管理IDより対象データ以降に受払の発生しているデータ件数を取得
--***************************************************************************************
WITH maxdate AS ( 
    SELECT
        inout_history_id
    FROM
        pt_inout_history 
    WHERE
        lot_control_id = @LotControlId
    AND delete_flg = 0
    AND inout_history_id > @InoutHistoryId
) 
, thisdate AS ( 
    SELECT
        inout_history_id
    FROM
        pt_inout_history 
    WHERE
        inout_history_id = @InoutHistoryId
) 
SELECT
    COUNT(*) AS count
FROM
    maxdate mde 
    LEFT JOIN thisdate tde 
        ON mde.inout_history_id = tde.inout_history_id