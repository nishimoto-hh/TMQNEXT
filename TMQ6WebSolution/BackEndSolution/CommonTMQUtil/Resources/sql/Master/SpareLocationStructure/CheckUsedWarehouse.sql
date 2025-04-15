-- 在庫データで削除対象の倉庫を親とする棚が使用されているかどうかデータ件数を取得する
SELECT
    count(*) 
FROM
    pt_location_stock stock 
WHERE
    parts_location_id IN ( 
        SELECT
            ms.structure_id 
        FROM
            ms_structure ms 
        WHERE
            ms.structure_group_id = 1040 
            AND ms.structure_layer_no = 3 
            AND ms.parent_structure_id = @StructureId
    )
