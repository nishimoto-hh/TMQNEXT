/*
 * ファイル入出力項目定義情報取得SQL
 */
select
    it.item_id                                                      -- 項目ID
    , COALESCE(NULLIF(it.column_name,''), co.column_name) AS column_name    -- カラム名
    , it.output_method                                              -- 出力方式
    , it.continuous_output_interval                                 -- 連続出力間隔
    --, tr.translation_text as item_name                              -- 項目名
    ,[dbo].[get_rep_translation_text](@FactoryId, it.control_id, @LanguageId) AS item_name
    , it.default_cell_row_no                                        -- デフォルトセル行No
    , it.default_cell_column_no                                     -- デフォルトセル列No
    , oi.default_cell_row_no as output_default_cell_row_no          -- 出力デフォルトセル行No
    , oi.default_cell_column_no as output_default_cell_column_no    -- 出力デフォルトセル列No
    , oi.display_order                                              -- 表示順
    , co.data_type                                                  -- データ種別
    ,[dbo].[get_rep_translation_text](@FactoryId, co.format_translation_id , @LanguageId) AS format_text    -- 書式文字列
from
    ms_output_report_item_define it         -- 出力帳票項目定義
    inner join
        ms_output_item oi                   -- 出力項目
    on
        it.factory_id = oi.factory_id       -- 工場ID
    and it.report_id = oi.report_id         -- 帳票ID
    and it.sheet_no = oi.sheet_no           -- シート番号
    and it.item_id = oi.item_id             -- 項目ID
    inner join
        cm_control_define co                -- コントロール定義
    on
        it.control_id = co.control_id       -- コントロールID
    and it.control_type = co.control_type   -- コントロールタイプ
--    left outer join
--        ms_translation tr                   -- 翻訳
--    on
--        it.control_id = tr.translation_id   -- コントロールID = 翻訳ID
where
    it.factory_id = @FactoryId              -- 工場ID
and it.report_id = @ReportId                -- 帳票ID
and it.sheet_no = @SheetNo                  -- シート番号
and oi.template_id = @TemplateId            -- テンプレートID
and oi.output_pattern_id = @OutputPatternId -- 出力パターンID
-- and tr.location_structure_id = @FactoryId   -- 工場ID
-- and tr.language_id = 'ja'                   -- 言語ID
order by
    oi.display_order                        -- 表示順

