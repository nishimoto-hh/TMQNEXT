--******************************************************************
--受払履歴IDより棚IDと棚番を取得する
--******************************************************************
SELECT DISTINCT
    pls.parts_location_id
    , pls.parts_location_detail_no 
FROM
    pt_location_stock pls 
    LEFT JOIN pt_inout_history pih 
        ON pls.inventory_control_id = pih.inventory_control_id 
WHERE
    pih.inout_history_id IN ( 
        --受払履歴ID
        SELECT
            * 
        FROM
            dbo.get_splitText(@IdList, default, default)
    )
