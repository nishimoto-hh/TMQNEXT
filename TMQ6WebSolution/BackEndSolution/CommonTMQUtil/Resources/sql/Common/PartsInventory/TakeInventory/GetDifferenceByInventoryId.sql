/*
* 棚卸 棚卸IDにより棚差調整データを取得
* GetDifferenceByInventoryId.sql
*/
SELECT
    dif.*
FROM
    pt_inventory_difference AS dif
WHERE
    dif.inventory_id = @InventoryId