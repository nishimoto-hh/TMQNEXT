DELETE FROM
    ms_output_pattern 
WHERE
    factory_id = @FactoryId
AND
    report_id = @ReportId
AND 
    template_id = @TemplateId
AND 
    output_pattern_id = @OutputPatternId
