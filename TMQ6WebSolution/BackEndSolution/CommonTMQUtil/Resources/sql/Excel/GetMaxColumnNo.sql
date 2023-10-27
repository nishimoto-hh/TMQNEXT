SELECT
    MAX(it.default_cell_column_no)
FROM
    ms_output_item it                       -- 出力項目
WHERE
    it.factory_id = @FactoryId
AND it.report_id = @ReportId
AND it.template_id = @TemplateId
AND it.output_pattern_id = @OutputPatternId
AND it.sheet_no = @SheetNo
AND it.item_id IN (
    SELECT
        id.item_id
    FROM
        ms_output_report_item_define id     -- 出力帳票項目定義
    WHERE
        id.factory_id = @FactoryId
    AND id.report_id = @ReportId
    AND id.sheet_no = @SheetNo
    AND id.output_method = 2                -- 2:縦方向連続
)
