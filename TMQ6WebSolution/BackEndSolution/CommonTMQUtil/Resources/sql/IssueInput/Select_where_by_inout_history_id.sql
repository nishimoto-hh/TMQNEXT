--******************************************************************
--登録された受払履歴IDより出庫を行った新旧区分、部門、勘定科目、棚番、棚枝番を取得
--******************************************************************
SELECT DISTINCT
    plt.old_new_structure_id
    , pih.department_structure_id
    , pih.account_structure_id
    , stock.parts_location_id
    , stock.parts_location_detail_no
    , stock.parts_id
FROM
    pt_inout_history pih 
    LEFT JOIN pt_lot plt 
        ON pih.lot_control_id = plt.lot_control_id
    LEFT JOIN pt_location_stock stock
        ON pih.inventory_control_id = stock.inventory_control_id
WHERE
    pih.work_no IN (
        SELECT
            history.work_no
        FROM
            pt_inout_history history
        WHERE
            history.inout_history_id IN (  --受払履歴ID   
            SELECT
                * 
            FROM
                dbo.get_splitText(@IdList, default, default)
    )
)