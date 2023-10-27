-- 出力帳票定義（共通を複写して登録）
INSERT 
INTO [ms_output_report_define] ( 
    [factory_id]                                -- 工場id
    , [program_id]                              -- プログラムid
    , [report_id]                               -- 帳票id
    , [template_upload_flg]                     -- テンプレートアップロード可否
    , [report_name_translation_id]              -- 帳票名翻訳id
    , [report_name]                             -- 帳票名（開発者向け）
    , [management_type]                         -- 管理種別
    , [output_item_type]                        -- 出力項目種別
    , [display_order]                           -- 表示順
    , [update_serialid]                         -- 更新シリアルID
    , [delete_flg]                              -- 削除フラグ
    , [insert_datetime]                         -- 登録日時
    , [insert_user_id]                          -- 登録ユーザー
    , [update_datetime]                         -- 更新日時
    , [update_user_id]                          -- 更新ユーザー
) 
SELECT
      @FactoryId                                    -- 工場id
    , ord.[program_id]                              -- プログラムid
    , ord.[report_id]                               -- 帳票id
    , 'true'                                        -- テンプレートアップロード可否
    , ord.[report_name_translation_id]              -- 帳票名翻訳id
    , ord.[report_name]                             -- 帳票名（開発者向け）
    , ord.[management_type]                         -- 管理種別
    , ord.[output_item_type]                        -- 出力項目種別
    , ord.[display_order]                           -- 表示順
    , ord.[update_serialid]                         -- 更新シリアルID
    , ord.[delete_flg]                              -- 削除フラグ
    , ord.[insert_datetime]                         -- 登録日時
    , ord.[insert_user_id]                          -- 登録ユーザー
    , ord.[update_datetime]                         -- 更新日時
    , ord.[update_user_id]                          -- 更新ユーザー
FROM
    [ms_output_report_define] ord 
WHERE
    ord.[factory_id] = 0                            -- 工場id
    AND ord.[program_id] = @ProgramId               -- プログラムid
    AND ord.[report_id] = @ReportId                 -- 帳票id
;
-- 出力帳票シート定義（共通を複写して登録）
INSERT 
INTO [ms_output_report_sheet_define] ( 
    [factory_id]                                -- 工場id
    , [report_id]                               -- 帳票id
    , [sheet_no]                                -- シート番号
    , [sheet_name_translation_id]               -- シート名翻訳id
    , [sheet_name]                              -- シート名（開発者向け）
    , [sheet_define_max_row]                    -- シート定義最大行
    , [sheet_define_max_column]                 -- シート定義最大列
    , [search_condition_flg]                    -- 検索条件フラグ
    , [list_flg]                                -- 一覧フラグ
    , [record_count]                            -- レコード行数
    , [target_sql]                              -- 対象sql
    , [target_sql_params]                       -- 対象sqlパラメータ
    , [update_serialid]                         -- 更新シリアルID
    , [delete_flg]                              -- 削除フラグ
    , [insert_datetime]                         -- 登録日時
    , [insert_user_id]                          -- 登録ユーザー
    , [update_datetime]                         -- 更新日時
    , [update_user_id]                          -- 更新ユーザー
) 
SELECT
      @FactoryId                                     -- 工場id
    , orsd.[report_id]                               -- 帳票id
    , orsd.[sheet_no]                                -- シート番号
    , orsd.[sheet_name_translation_id]               -- シート名翻訳id
    , orsd.[sheet_name]                              -- シート名（開発者向け）
    , orsd.[sheet_define_max_row]                    -- シート定義最大行
    , orsd.[sheet_define_max_column]                 -- シート定義最大列
    , orsd.[search_condition_flg]                    -- 検索条件フラグ
    , orsd.[list_flg]                                -- 一覧フラグ
    , orsd.[record_count]                            -- レコード行数
    , orsd.[target_sql]                              -- 対象sql
    , orsd.[target_sql_params]                       -- 対象sqlパラメータ
    , orsd.[update_serialid]                         -- 更新シリアルID
    , orsd.[delete_flg]                              -- 削除フラグ
    , orsd.[insert_datetime]                         -- 登録日時
    , orsd.[insert_user_id]                          -- 登録ユーザー
    , orsd.[update_datetime]                         -- 更新日時
    , orsd.[update_user_id]                          -- 更新ユーザー
FROM
    [dbo].[ms_output_report_sheet_define] orsd 
WHERE
    orsd.[factory_id] = 0                            -- 工場id
    AND orsd.[report_id] = @ReportId                 -- 帳票id
;
-- 出力帳票項目定義（共通を複写して登録）
INSERT 
INTO [ms_output_report_item_define] ( 
    orid.[factory_id]                                -- 工場id
    , orid.[report_id]                               -- 帳票id
    , orid.[sheet_no]                                -- シート番号
    , orid.[item_id]                                 -- 項目id
    , orid.[item_name]                               -- 項目名（開発者向け）
    , orid.[control_id]                              -- コントロールid
    , orid.[control_type]                            -- コントロールタイプ
    , orid.[column_name]                             -- カラム名
    , orid.[output_method]                           -- 出力方式
    , orid.[continuous_output_interval]              -- 連続出力間隔
    , orid.[default_cell_row_no]                     -- デフォルトセル行no
    , orid.[default_cell_column_no]                  -- デフォルトセル列no
    , orid.[default_row_join_count]                  -- デフォルト行結合数
    , orid.[default_column_join_count]               -- デフォルト列結合数
    , orid.[update_serialid]                         -- 更新シリアルID
    , orid.[delete_flg]                              -- 削除フラグ
    , orid.[insert_datetime]                         -- 登録日時
    , orid.[insert_user_id]                          -- 登録ユーザー
    , orid.[update_datetime]                         -- 更新日時
    , orid.[update_user_id]                          -- 更新ユーザー
) 
SELECT
      @FactoryId                                -- 工場id
    , [report_id]                               -- 帳票id
    , [sheet_no]                                -- シート番号
    , [item_id]                                 -- 項目id
    , [item_name]                               -- 項目名（開発者向け）
    , [control_id]                              -- コントロールid
    , [control_type]                            -- コントロールタイプ
    , [column_name]                             -- カラム名
    , [output_method]                           -- 出力方式
    , [continuous_output_interval]              -- 連続出力間隔
    , [default_cell_row_no]                     -- デフォルトセル行no
    , [default_cell_column_no]                  -- デフォルトセル列no
    , [default_row_join_count]                  -- デフォルト行結合数
    , [default_column_join_count]               -- デフォルト列結合数
    , [update_serialid]                         -- 更新シリアルID
    , [delete_flg]                              -- 削除フラグ
    , [insert_datetime]                         -- 登録日時
    , [insert_user_id]                          -- 登録ユーザー
    , [update_datetime]                         -- 更新日時
    , [update_user_id]                          -- 更新ユーザー
FROM
    [ms_output_report_item_define] orid 
WHERE
    orid.[factory_id] = 0                            -- 工場id
    AND orid.[report_id] = @ReportId                 -- 帳票id
;
