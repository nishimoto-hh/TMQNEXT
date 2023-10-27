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
    [delete_flg],               -- 削除フラグ
    [insert_datetime],          -- 登録日時
    [insert_user_id],           -- 登録ユーザー
    [update_datetime],          -- 更新日時
    [update_user_id]            -- 更新ユーザー
) 
SELECT 
    [factory_id],               -- 工場ID
    [report_id],                -- 帳票ID
    [template_id],              -- テンプレートID
    [output_pattern_id] + 1,    -- 出力パターンid
    [sheet_no],                 -- シート番号
    [item_id],                  -- 項目id
    [display_order],            -- 表示順
    [default_cell_row_no],      -- デフォルトセル行no
    [default_cell_column_no],   -- デフォルトセル列no
    [update_serialid],          -- 更新シリアルID
    [delete_flg],               -- 削除フラグ
    [insert_datetime],          -- 登録日時
    [insert_user_id],           -- 登録ユーザー
    [update_datetime],          -- 更新日時
    [update_user_id]            -- 更新ユーザー
FROM ms_output_item
WHERE
    [factory_id] = @FactoryId
AND
    [report_id] = @ReportId
AND
    [template_id] = @TemplateId
AND
    [output_pattern_id] = @OutputPatternId
AND
    [sheet_no] > 1
