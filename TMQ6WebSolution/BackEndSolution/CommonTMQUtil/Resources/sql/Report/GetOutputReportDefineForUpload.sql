/*
 * ファイル入出力項目定義情報取得SQL
 */
SELECT
    it.item_id                                                      -- 項目ID
    , it.column_name                                                -- カラム名
    , it.output_method                                              -- 出力方式
    , it.continuous_output_interval                                 -- 連続出力間隔
    , [dbo].[get_rep_translation_text](@FactoryId, it.control_id , @LanguageId) AS item_name
    , it.default_cell_row_no                                        -- デフォルトセル行No
    , it.default_cell_column_no                                     -- デフォルトセル列No
    , null as output_default_cell_row_no                            -- 出力デフォルトセル行No
    , null as output_default_cell_column_no                         -- 出力デフォルトセル列No
    , 1 as display_order
    , co.data_type                                                  -- データ種別
    , ROW_NUMBER() OVER(ORDER BY it.item_id) as sort_order          -- 表示順
FROM
    ms_output_report_item_define it                                 -- 出力帳票項目定義
    INNER JOIN cm_control_define co                                 -- コントロール定義
        ON it.control_id = co.control_id                            -- コントロールID
        AND it.control_type = co.control_type                       -- コントロールタイプ
WHERE
    it.factory_id = @FactoryId                                      -- 工場ID
AND 
    it.report_id = @ReportId                                        -- 帳票ID
AND 
    it.sheet_no = @SheetNo                                          -- シート番号

