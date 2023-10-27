SELECT
    FORMAT(receiving_datetime, 'yyyy/MM/dd') AS receiving_datetime,
    --入庫日
    pt_lot.lot_no,
    --ロットNo
    location.translation_text AS location_id,
    --棚番
    old_new.translation_text AS old_new_nm,
    --新旧区分id
    CAST(unit_price AS VARCHAR) + currency_name AS unit_price,
    --入庫単価 + 金額単位名称
    CAST(pt_location.stock_quantity AS VARCHAR) + unit_name AS unit,
    --在庫数 + 数量管理単位
     department_code.extension_data + ' ' + department.translation_text AS department_nm,
    --部門コード + 部門名
    subject_code.extension_data + ' ' + subject.translation_text AS subject_nm,
    --勘定科目コード + 勘定科目
    management_no,
    --管理No
    management_division,
    --管理区分
    pt_lot.parts_id,
    --予備品ID
    work_no,
    --作業No
    parts_ware.structure_id  AS ware_house_id,
    --予備品倉庫
    location.structure_id AS parts_location_id,
    --棚番
    pt_location.parts_location_detail_no,
    --棚枝番
    pt_lot.unit_name,
    --数量管理単位
    pt_lot.currency_name
    --金額管理単位
    
FROM
    pt_lot
    LEFT JOIN
        pt_location
    --在庫データテーブル
ON  pt_lot.parts_id = pt_location.parts_id
--予備品IDで結合
AND pt_lot.lot_control_id = pt_location.lot_control_id
    --ロットNoで結合
LEFT JOIN
    location
--棚番
ON  pt_location.parts_location_id = location.structure_id
LEFT JOIN
    parts_ware
--予備品倉庫
ON  pt_location.parts_location_id = parts_ware.structure_id
LEFT JOIN
    old_new
--新旧区分
ON  pt_lot.old_new_structure_id = old_new.structure_id
LEFT JOIN
    department
--部門
ON  pt_lot.department_structure_id = department.structure_id
LEFT JOIN
    subject
--勘定科目
ON  pt_lot.account_structure_id = subject.structure_id
LEFT JOIN
department_code
--部門コード
ON department.structure_item_id = department_code.item_id
LEFT JOIN
subject_code
--勘定科目コード
ON subject.structure_item_id = subject_code.item_id
LEFT JOIN
history
--受払履歴
ON pt_lot.lot_control_id = history.lot_control_id
LEFT JOIN
iot
ON  history.inout_division_structure_id = iot.structure_id
WHERE pt_lot.parts_id = @PartsId
