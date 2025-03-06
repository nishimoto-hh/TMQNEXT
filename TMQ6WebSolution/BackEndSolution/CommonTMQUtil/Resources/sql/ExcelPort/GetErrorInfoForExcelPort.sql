SELECT DISTINCT
    row_no
    , column_no
    , error
    , data_direction
    , proc_div 
    , proc_div_name
FROM
    #excel_port_error_info 
ORDER BY
    row_no
    , column_no
