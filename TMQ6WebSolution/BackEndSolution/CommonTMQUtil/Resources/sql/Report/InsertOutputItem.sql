INSERT INTO ms_output_item( 
    [factory_id],               -- 工場ID
    [report_id],                -- 帳票ID
    [template_id],              -- テンプレートID
    [output_pattern_id],        -- 出力パターンid
    [sheet_no],                 -- シート番号
    [item_id],                  -- 項目id
    [display_order],            -- 表示順
    [default_cell_row_no],      -- デフォルトセル行no
    [default_cell_column_no],   -- デフォルトセル列no
    [update_serialid],          -- 更新シリアルID
    [insert_datetime],          -- 登録日時
    [insert_user_id],           -- 登録ユーザー
    [update_datetime],          -- 更新日時
    [update_user_id]            -- 更新ユーザー
) 
VALUES ( 
    @FactoryId,                 -- 工場ID
    @ReportId,                  -- 帳票ID
    @TemplateId,                -- テンプレートID
    @OutputPatternId,           -- 出力パターンID
    @SheetNo,                   -- シート番号
    @ItemId,                    -- 項目ID
    @DisplayOrder,              -- 表示順
    @DefaultCellRowNo,          -- デフォルトセル行NO
    @DefaultCellColumnNo,       -- デフォルトセル列NO
    @UpdateSerialid,            -- 更新シリアルID
    @InsertDatetime,            -- 登録日時
    @InsertUserId,              -- 登録ユーザー
    @UpdateDatetime,            -- 更新日時
    @UpdateUserId               -- 更新ユーザー
)
