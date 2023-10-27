SELECT COUNT(*)
FROM ms_output_pattern op 
WHERE
    op.factory_id = @FactoryId                       -- 工場ID
AND
    op.report_id = @ReportId                         -- 帳票ID
AND
    op.template_id = @TemplateId                     -- テンプレートID
AND
    op.output_pattern_name = @OutputPatternName      -- 出力パターン名
