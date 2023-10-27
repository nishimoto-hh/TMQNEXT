--******************************************************************
--詳細(その他)取得
--******************************************************************

--呼び出し回数当月(個別工場)当月
 SELECT * from dbo.get_history_callcnt_other(@StructureId,@TargetYear,@TargetYear,@LanguageId)--構成ID、指定対象年月、指定対象年月、言語ID

--呼び出し回数半期(個別工場)半期
UNION ALL 
 SELECT * from dbo.get_history_callcnt_other(@StructureId,@StartMonth,@TargetYear,@LanguageId)--構成ID、期の開始月、指定対象年月、言語ID

--呼び出し回数累計(個別工場)累計
UNION ALL 
 SELECT * from dbo.get_history_callcnt_other(@StructureId,@BeginningMonth,@TargetYear,@LanguageId)--構成ID、期の開始月、指定対象年月、言語ID
