SELECT
    ROW_NUMBER() OVER(ORDER BY ISNULL(rd.display_order, 2147483647), cdt.menu_order) AS ROWNO,
    tr.translation_text as report_name,
    rd.factory_id as factory_id,
    rd.program_id as program_id,
    rd.report_id as report_id,
    ROW_NUMBER() OVER(ORDER BY ISNULL(rd.display_order, 2147483647), cdt.menu_order) AS sort_order 
FROM
    ms_output_report_define rd
    INNER JOIN
        cm_conduct cdt
        ON  cdt.conduct_id = rd.report_id
    INNER JOIN
        ms_translation tr
        ON  rd.report_name_translation_id = tr.translation_id
        AND tr.language_id = @LanguageId
        AND tr.location_structure_id = rd.factory_id
WHERE 
    cdt.conduct_group_id = @ReportGroupId
AND 
    cdt.delete_flg = 0
AND 
    rd.delete_flg = 0
AND
    tr.delete_flg = 0
