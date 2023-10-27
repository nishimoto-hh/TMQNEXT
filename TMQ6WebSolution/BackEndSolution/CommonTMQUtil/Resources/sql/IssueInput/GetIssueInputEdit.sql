--******************************************************************
--受払履歴IDより受払日、出庫数合計、出庫区分、作業Noを取得
--******************************************************************
SELECT
     inout_datetime
     ,SUM(pih.inout_quantity) AS number_shipments
     ,shipping_division_structure_id
     ,work_no
FROM
    pt_inout_history pih 
WHERE
    pih.inout_history_id IN (SELECT * FROM dbo.get_splitText(@IdList, default, default))
GROUP BY inout_datetime ,shipping_division_structure_id,work_no
