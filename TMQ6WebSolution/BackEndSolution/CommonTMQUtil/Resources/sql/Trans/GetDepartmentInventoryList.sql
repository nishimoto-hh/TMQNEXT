SELECT
    FORMAT(receiving_datetime, 'yyyy/MM/dd') AS receiving_datetime,
    --入庫日
    pt_lot.lot_no,
    --ロットNo?
    old_new.translation_text AS old_new_nm,
    --新旧区分
    CAST(unit_price AS VARCHAR) + currency_name AS unit_price,
    --入庫単価 + 金額単位名称
    CAST(stock_quantity AS VARCHAR) + unit_name AS unit,
    --在庫数
    department_code.extension_data + ' ' + department.translation_text AS department_nm,
    --部門コード + 部門名
    subject_code.extension_data + ' ' + subject.translation_text AS subject_nm,
    --勘定科目コード + 勘定科目
    management_no,
    --管理No
    management_division
--管理区分
FROM
    pt_lot
    LEFT JOIN
        pt_location_stock
    --在庫データテーブル
ON  pt_lot.parts_id = pt_location_stock.parts_id
LEFT JOIN
    old_new
--新旧区分
ON  pt_lot.old_new_structure_id = old_new.structure_id
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
LEFT JOIN
    history
--受払履歴
ON  pt_lot.lot_control_id = history.lot_control_id
LEFT JOIN
    iot
--受払区分
ON  history.inout_division_structure_id = iot.structure_id
WHERE
    iot.structure_id = 412
