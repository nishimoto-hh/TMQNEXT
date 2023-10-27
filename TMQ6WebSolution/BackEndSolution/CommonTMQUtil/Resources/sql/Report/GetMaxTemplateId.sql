SELECT
    MAX(ISNULL(ot.template_id, 0)) AS max_template_id
FROM
    ms_output_template ot
WHERE
    factory_id = @FactoryId
AND
    report_id = @ReportId
