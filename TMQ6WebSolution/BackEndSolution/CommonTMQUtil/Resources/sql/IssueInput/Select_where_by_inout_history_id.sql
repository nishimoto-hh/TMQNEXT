--******************************************************************
--登録された受払履歴IDより新旧区分、部門、勘定科目を取得
--******************************************************************
SELECT DISTINCT
    plt.old_new_structure_id
    , pih.department_structure_id
    , pih.account_structure_id 
FROM
    pt_inout_history pih 
    LEFT JOIN pt_lot plt 
        ON pih.lot_control_id = plt.lot_control_id 
WHERE
    pih.inout_history_id IN (  --受払履歴ID   
        SELECT
            * 
        FROM
            dbo.get_splitText(@IdList, default, default)
    )
