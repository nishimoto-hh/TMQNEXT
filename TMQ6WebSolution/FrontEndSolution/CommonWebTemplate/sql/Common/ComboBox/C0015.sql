/*
 * 予備品管理 出力帳票　コンボ用データリスト用SQL
 * 
 */
SELECT
    rd.report_id as 'values',
    tr.translation_text as labels
FROM
    ms_output_report_define rd
    INNER JOIN
        cm_conduct cdt
        ON  cdt.conduct_id = rd.report_id
    INNER JOIN
        ms_translation tr
        ON  rd.report_name_translation_id = tr.translation_id
        AND tr.language_id = /*languageId*/'ja' 
        AND tr.location_structure_id = rd.factory_id
WHERE 
    cdt.conduct_group_id = /*param1*/''
AND 
    cdt.delete_flg = 0
AND 
    rd.delete_flg = 0
AND
    tr.delete_flg = 0
order by ISNULL(rd.display_order, 2147483647), rd.report_id
