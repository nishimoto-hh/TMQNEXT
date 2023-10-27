--******************************************************************
--予備品IDより在庫確定日を取得
--******************************************************************
SELECT
    EOMONTH(MAX(pfs.target_month)) AS target_month
FROM
    pt_fixed_stock pfs
    LEFT JOIN pt_parts pps
        ON pfs.parts_id = pps.parts_id
WHERE
    pps.parts_id = @PartsId --予備品ID
