--******************************************************************
--MQ指標実績表（年俸）の集計結果
--******************************************************************
SELECT 
	targetDate, 
	rowNo, 
	machine, 
	electricity, 
	instrumentation, 
	other, 
	total 
FROM [dbo].[get_rep_mq_actual_evaluation](
	@StartMonth, --集計開始年月日
	@LocationId, --場所階層ID(カンマ区切り) ※場所階層ツリー＋場所階層追加
	@StructureIdList, --場所階層ID(カンマ区切り) ※場所階層ツリーの値リスト
	@LanguageId --言語ID
)
ORDER BY rowNo, targetDate