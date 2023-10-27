SELECT
    MIN(default_cell_column_no)
FROM
    ms_output_item
WHERE
    factory_id = @FactoryId
AND report_id = @ReportId
AND template_id = @TemplateId
AND output_pattern_id = @OutputPatternId
AND sheet_no = @SheetNo
