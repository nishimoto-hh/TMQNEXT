SELECT COUNT(*)
FROM ms_output_template ot 
WHERE
    factory_id = @FactoryId            -- 工場ID
AND
    report_id = @ReportId              -- 帳票ID
-- 更新時、自身を対象外とする場合に指定する条件
/*@TemplateId
AND
    template_id <> @TemplateId
@TemplateId*/
AND
    template_name = @TemplateName      -- テンプレート名
