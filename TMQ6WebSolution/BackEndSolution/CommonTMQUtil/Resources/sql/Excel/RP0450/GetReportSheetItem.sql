-- 系停止回数やMQ分類名などのマッピングの開始行・条件付き書式の適用行を取得する
SELECT
    * 
FROM
    ms_output_report_item_define mo 
WHERE
    mo.factory_id = @ReportTargetFactoryId 
    AND mo.report_id = 'RP0450' 
    AND mo.sheet_no = 3 
    AND mo.column_name = @ColumnName
ORDER BY
    mo.item_id