SELECT
    SUM(stock_quantity) AS stock_quantity 
FROM
    pt_inventory 
WHERE
    inventory_id in @InventoryIdList
