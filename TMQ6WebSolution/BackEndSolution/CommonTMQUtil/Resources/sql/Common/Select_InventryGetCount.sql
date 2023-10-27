--******************************************************************
--予備品ID、更新日時、受払日時より棚卸中のデータ件数を取得
--******************************************************************
SELECT DISTINCT
    Count(*) AS count 
FROM
    pt_inventory pit                            --棚卸データ
    LEFT JOIN pt_location_stock pls             --在庫データ
        ON pit.parts_location_id = pls.parts_location_id 
        AND pit.parts_location_detail_no = pls.parts_location_detail_no 
    LEFT JOIN pt_lot plt                        --ロット情報
        ON pit.old_new_structure_id = plt.old_new_structure_id 
        AND pit.department_structure_id = plt.department_structure_id 
        AND pit.account_structure_id = plt.account_structure_id 
WHERE
    pit.parts_id = @PartsId
    AND @UpdateDatetime <= pit.preparation_datetime 
    AND pit.fixed_datetime IS NULL 
    AND @InoutDatetime <= EOMONTH(pit.target_month)
