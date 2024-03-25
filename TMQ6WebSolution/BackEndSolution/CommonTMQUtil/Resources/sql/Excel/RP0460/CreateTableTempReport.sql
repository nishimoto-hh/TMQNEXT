DROP TABLE IF EXISTS #temp_report
;
CREATE TABLE #temp_report( 
    inventory_control_id INT         -- ç›å…ä«óùID
    , lot_control_id INT             -- ÉçÉbÉgä«óùID
    , stock_quantity DECIMAL (20, 2) -- ç›å…êî
    , amount DECIMAL (20, 2)         -- ç›å…ã‡äz
); 