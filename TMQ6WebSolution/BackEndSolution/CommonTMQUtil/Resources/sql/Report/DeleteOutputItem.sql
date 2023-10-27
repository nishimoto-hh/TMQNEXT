DELETE
FROM
    ms_output_item
WHERE
    factory_id = @FactoryId
AND 
    report_id = @ReportId
AND 
    template_id = @TemplateId
AND 
    output_pattern_id = @OutputPatternId
-- 指定時のみ
/*@SheetNo
AND 
    sheet_no = @SheetNo
@SheetNo*/
-- 指定時のみ
/*@ItemId
AND
    item_id = @ItemId
@ItemId*/
