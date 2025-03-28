-- 在庫データで削除対象の棚が使用されているかどうかデータ件数を取得する
SELECT
    count(*) 
FROM
    pt_location_stock stock 
    LEFT JOIN ms_structure ms 
        ON stock.parts_location_id = ms.structure_id 
WHERE
    parts_location_id = @StructureId 
    AND ms.structure_layer_no = 3
