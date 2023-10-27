SELECT
    FORMAT(history.inout_datetime, 'yyyy/MM/dd') AS relocation_date,
    --移庫日
    parts.translation_text AS storage_locationId,
    --予備品倉庫
    location.translation_text AS locarion_id,
    --棚
    parts_location_detail_no,
    --棚枝番
    history.inout_quantity AS transfer_count,
    --移庫数
    --unit_name,--数量単位名称
    pt_lot.unit_price AS transfer_amount
    --移庫金額
    --currency_name--金額単位名称
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
    parts
--予備品倉庫
ON  parts.structure_id = stock.parts_location_id
LEFT JOIN
    location
--棚
ON  parts.structure_id = stock.parts_location_id
WHERE work_no = @WorkNo
