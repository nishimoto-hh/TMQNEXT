DECLARE @RC int
DECLARE @ErrorColNo int
DECLARE @ErrorInfo nvarchar(800)

-- 方法: パラメーター値をここに指定します。

EXECUTE @RC = [dbo].[pro_regist_excel_port] 
  @ConductId         -- 機能ID
  ,@DateTime         -- 登録日時
  ,@UserId           -- ユーザーID
  ,@LanguageId       -- 言語ID
  ,@ReportId         -- テンプレートファイルID
  ,@SheetNo          -- シート番号
  ,@ControlGroupId   -- コントロールグループID
  ,@ErrorColNo OUTPUT-- エラー有無列番号
  ,@ErrorInfo OUTPUT -- エラーメッセージ

SELECT @RC AS result, @ErrorColNo AS error_col, @ErrorInfo AS error_info        -- 処理結果(0：正常終了、-1/-2/-999：異常終了)、エラー有無列番号、エラーメッセージ
