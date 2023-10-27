--******************************************************************
--受払履歴IDよりロット情報を取得
--******************************************************************
SELECT
    plt.lot_control_id
    , plt.old_new_structure_id
    , pih.department_structure_id
    , pih.account_structure_id 
FROM
    pt_inout_history pih 
    LEFT JOIN pt_lot plt 
        ON pih.lot_control_id = plt.lot_control_id 
WHERE
    pih.inout_history_id = @InoutHistoryId
