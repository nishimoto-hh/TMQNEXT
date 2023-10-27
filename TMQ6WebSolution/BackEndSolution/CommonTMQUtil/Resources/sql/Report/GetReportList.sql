SELECT
-- 出力パターン指定時のみ
/*@OutputPatternId
    ISNULL(oi.display_order, 2147483647) AS display_order,
@OutputPatternId*/
-- 出力パターン未指定時のみ
/*@NoConditionOutputPatternId
    1 AS display_order,
@NoConditionOutputPatternId*/
    tr.translation_text AS item_name,
    rid.factory_id AS factory_id,
    rid.report_id AS report_id,
    rd.output_item_type AS output_item_type,
-- テンプレート名指定時のみ
/*@TemplateId
    ot.template_file_path AS template_file_path,
    ot.template_file_name AS template_file_name,
    ot.template_id AS template_id,
@TemplateId*/
-- テンプレート名未指定時のみ
/*@NoConditoinTemplateId
    '' AS template_file_path,
    '' AS template_file_name,
    -1 AS template_id,
@NoConditoinTemplateId*/

-- 出力パターン指定時のみ
/*@OutputPatternId
    oi.output_pattern_id AS output_pattern_id, 
    oi.item_id AS output_item_id, 
    CONVERT(DATETIME, CONVERT(VARCHAR(24) , oi.update_datetime ,20)) AS update_datetime,
@OutputPatternId*/
-- 出力パターン未指定時のみ
/*@NoConditionOutputPatternId
    -1 AS output_pattern_id, 
    rid.item_id AS output_item_id, 
@NoConditionOutputPatternId*/
    rid.sheet_no AS sheet_no, 
    rid.item_id AS item_id,
-- 出力パターン指定時のみ
/*@OutputPatternId
    ROW_NUMBER() OVER(ORDER BY ISNULL(oi.display_order, 2147483647), rid.item_id) AS sort_order 
@OutputPatternId*/
-- 出力パターン未指定時のみ
/*@NoConditionOutputPatternId
    ROW_NUMBER() OVER(ORDER BY rid.item_id) AS sort_order 
@NoConditionOutputPatternId*/
FROM
    ms_output_report_sheet_define rsd
    INNER JOIN ms_output_report_item_define rid
        ON rid.factory_id = rsd.factory_id
        AND rid.report_id = rsd.report_id
        AND rid.sheet_no = rsd.sheet_no
    INNER JOIN ms_output_report_define rd
        ON rd.factory_id = rsd.factory_id
        AND rd.report_id = rsd.report_id
    INNER JOIN
        ms_translation tr
        ON  rid.control_id = tr.translation_id
        AND tr.language_id = @LanguageId
        AND tr.location_structure_id = rsd.factory_id
-- テンプレート名指定時のみ
/*@TemplateId
    LEFT OUTER JOIN ms_output_template ot
        ON ot.factory_id = rsd.factory_id
        AND ot.report_id = rsd.report_id
        AND ot.template_id = @TemplateId
@TemplateId*/

-- 出力パターン指定時のみ
/*@OutputPatternId
    LEFT OUTER JOIN ms_output_item oi
        ON oi.factory_id = rsd.factory_id
        AND oi.report_id = rsd.report_id
        AND oi.template_id = ot.template_id
        AND oi.sheet_no = rsd.sheet_no
        AND oi.item_id = rid.item_id
        AND oi.output_pattern_id = @OutputPatternId
@OutputPatternId*/

WHERE 
    1 = 1
AND 
    rsd.delete_flg = 0
AND 
    rid.delete_flg = 0
AND
    tr.delete_flg = 0
AND
    rsd.factory_id = @FactoryId
AND
    rsd.report_id = @ReportId
AND 
    rd.program_id = @ProgramId
AND
    rsd.search_condition_flg = 0
GROUP BY 

-- 出力パターン指定時のみ
/*@OutputPatternId
    oi.display_order,
@OutputPatternId*/

    tr.translation_text,
    rid.factory_id,
    rid.report_id,
    rd.output_item_type, 

-- テンプレート名指定時のみ
/*@TemplateId
    ot.template_file_path,
    ot.template_file_name,
    ot.template_id,
@TemplateId*/

-- 出力パターン指定時のみ
/*@OutputPatternId
    oi.output_pattern_id, 
    oi.item_id,
    oi.update_datetime,
@OutputPatternId*/

-- 出力パターン未指定時のみ
/*@NoConditionOutputPatternId
    rid.item_id,
@NoConditionOutputPatternId*/

    rid.sheet_no, 
    rid.item_id
