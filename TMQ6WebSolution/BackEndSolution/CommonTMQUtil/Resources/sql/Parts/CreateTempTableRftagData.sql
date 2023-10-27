DROP TABLE IF EXISTS #rf_read_date; 
CREATE TABLE #rf_read_date(
    rftag_id nvarchar(200)
    , parts_no nvarchar(200)
    , department_structure_id bigint
    , account_structure_id bigint
    , read_datetime datetime
);