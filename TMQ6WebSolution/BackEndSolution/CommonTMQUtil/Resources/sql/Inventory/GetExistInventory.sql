SELECT
    inventory_id 
    , fixed_datetime
FROM
    pt_inventory 
WHERE
    inventory_id IN @InventoryIdList 
