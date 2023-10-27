SELECT 
    item_id                                 -- 項目id
FROM
    ms_output_report_item_define orid 
WHERE
    orid.factory_id = @FactoryId                       -- 工場ID
AND
    orid.report_id = @ReportId                         -- 帳票ID
AND
    orid.sheet_no = @SheetNo                           -- シートNo
AND
    orid.column_name IN (
        (
            SELECT Text FROM dbo.get_splitText(@ColumnNameList,default,default) -- カラム名
        )
    )
