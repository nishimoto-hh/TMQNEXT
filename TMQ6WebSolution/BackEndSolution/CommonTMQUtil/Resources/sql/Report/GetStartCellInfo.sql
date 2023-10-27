SELECT
    factory_id, 
    report_id, 
    sheet_no, 
    output_method,
    MIN(default_cell_row_no) AS default_cell_row_no,
    MIN(default_cell_column_no) AS default_cell_column_no
FROM
    ms_output_report_item_define orid
WHERE
    orid.factory_id = @FactoryId
AND
    orid.report_id = @ReportId
AND
    orid.sheet_no = @SheetNo
AND
    orid.column_name NOT IN (
        (
            SELECT Text FROM dbo.get_splitText(@ExColumnNameList,default,default) -- 対象外カラム名
        )
    )
AND
    orid.output_method = 2 -- べた表のため、出力方式　2：縦方向連続　のみとする
GROUP BY 
    factory_id, report_id, sheet_no, output_method
ORDER BY 
    factory_id, report_id, sheet_no, output_method
