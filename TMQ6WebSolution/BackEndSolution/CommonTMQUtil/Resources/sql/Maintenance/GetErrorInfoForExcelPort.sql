SELECT
    info.row_no
    , info.column_no
    , info.error as error_info
    , info.data_direction
    , info.proc_div
FROM
    excel_port_error_info2 info