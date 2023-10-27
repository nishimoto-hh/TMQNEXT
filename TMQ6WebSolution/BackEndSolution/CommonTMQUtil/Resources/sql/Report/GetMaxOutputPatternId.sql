SELECT
    MAX(ISNULL(op.output_pattern_id, 0)) AS max_output_pattern_id
FROM
    ms_output_pattern op
WHERE
    factory_id = @FactoryId
AND
    report_id = @ReportId
AND
    template_id = @TemplateId
