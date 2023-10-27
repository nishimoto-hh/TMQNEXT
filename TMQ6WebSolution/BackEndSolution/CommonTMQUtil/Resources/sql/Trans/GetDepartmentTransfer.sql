SELECT
    FORMAT(history.inout_datetime, 'yyyy/MM/dd') AS relocation_date,
    --移庫日
    pt_lot.unit_price,
    --移庫単価
    stock_quantity AS transfer_count,
    --移庫数

    --移庫金額
    department_code.extension_data + ' ' + department.translation_text AS department_id,
    --部門コード + 部門名
    subject_code.extension_data + ' ' + subject.translation_text AS subject_nm,
    --勘定科目コード + 勘定科目
    pt_lot.management_division,
    --管理区分
    pt_lot.management_no
--管理No
FROM
    pt_lot
    --ロット情報
    LEFT JOIN
        pt_location_stock AS stock
    --在庫データ
ON  pt_lot.lot_control_id = stock.lot_control_id
LEFT JOIN
    pt_inout_history AS history
--受払履歴
ON  pt_lot.lot_control_id = history.lot_control_id
LEFT JOIN
    department
--部門
ON  pt_lot.department_structure_id = department.structure_id
LEFT JOIN
    department_code
--部門コード
ON  department.structure_item_id = department_code.item_id
LEFT JOIN
    subject
--勘定科目
ON  pt_lot.account_structure_id = subject.structure_id
LEFT JOIN
    subject_code
--勘定科目コード
ON  subject.structure_item_id = subject_code.item_id
