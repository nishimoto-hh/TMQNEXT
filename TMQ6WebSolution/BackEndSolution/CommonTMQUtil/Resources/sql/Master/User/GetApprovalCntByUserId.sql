/*
 指定されたユーザを承認者にしている工場の件数を取得
*/

SELECT
    count(*) 
FROM
    ms_structure ms 
    LEFT JOIN ms_item_extension ex 
        ON ms.structure_item_id = ex.item_id 
        AND ex.sequence_no = 4 
WHERE
    ms.structure_group_id = 1000 
    AND ms.structure_layer_no = 1 
    AND ex.extension_data = CAST(@UserId as VARCHAR)
