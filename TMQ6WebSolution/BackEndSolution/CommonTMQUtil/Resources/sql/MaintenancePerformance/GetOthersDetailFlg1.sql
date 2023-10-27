--******************************************************************
--詳細(その他)取得
--******************************************************************

--呼び出し回数当月(個別工場)当月
 SELECT * from dbo.get_history_callcnt_other(@StructureId,@LocationId,@TargetYear,@TargetYear,@LanguageId)--工場ID、構成ID(カンマ区切り)、指定対象年月、指定対象年月、言語ID

--呼び出し回数半期(個別工場)半期
UNION ALL 
 SELECT * from dbo.get_history_callcnt_other(@StructureId,@LocationId,@StartMonth,@TargetYear,@LanguageId)--工場ID、構成ID(カンマ区切り)、期の開始月、指定対象年月、言語ID

--呼び出し回数累計(個別工場)累計
UNION ALL 
 SELECT * from dbo.get_history_callcnt_other(@StructureId,@LocationId,@BeginningMonth,@TargetYear,@LanguageId)--工場ID、構成ID(カンマ区切り)、期の開始月、指定対象年月、言語ID
