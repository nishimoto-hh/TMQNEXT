DECLARE @RC int
DECLARE @ErrorInfo nvarchar(800)

-- 方法: パラメーター値をここに指定します。

EXECUTE @RC = [dbo].[pro_management_standards_quota] 
   @ManagementStandardsId
  ,@DateTime
  ,@UserId
  ,@LanguageId
  ,@ErrorInfo OUTPUT

SELECT @RC -- 処理結果(999：正常終了、-999：異常終了)
GO


