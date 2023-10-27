--****************************************************************************
--登録された受払履歴IDより同一の作業Noを持つ受払履歴IDを取得(予備品詳細画面用)
--****************************************************************************
SELECT
    inout_history_id AS IdList
FROM
    pt_inout_history 
WHERE
    work_no = ( 
        SELECT
            work_no 
        FROM
            pt_inout_history 
        WHERE
            inout_history_id = @IdList
    )
