--******************************************************************
--詳細(作業性格分類)取得
--******************************************************************

--予防保全作業件数当月
   SELECT * from dbo.get_summary_workcnt_personality('10',@TargetYear,@TargetYear,@locationId,@LanguageId)--拡張データ、指定対象年月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--予防保全作業件数半期
UNION ALL
   SELECT * from dbo.get_summary_workcnt_personality('10',@StartMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--予防保全作業件数累計
UNION ALL
   SELECT * from dbo.get_summary_workcnt_personality('10',@BeginningMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--故障修理件数当月
UNION ALL
   SELECT * from dbo.get_summary_workcnt_personality('20',@TargetYear,@TargetYear,@locationId,@LanguageId)--拡張データ、指定対象年月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--故障修理件数半期
UNION ALL
   SELECT * from dbo.get_summary_workcnt_personality('20',@StartMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--故障修理件数累計
UNION ALL
   SELECT * from dbo.get_summary_workcnt_personality('20',@BeginningMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--その他作業件数当月
UNION ALL
   SELECT * from dbo.get_summary_workcnt_personality('30',@TargetYear,@TargetYear,@locationId,@LanguageId)--拡張データ、指定対象年月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--その他作業件数半期
UNION ALL
   SELECT * from dbo.get_summary_workcnt_personality('30',@StartMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--その他作業件数累計
UNION ALL
   SELECT * from dbo.get_summary_workcnt_personality('30',@BeginningMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--製造関連作業件数当月
UNION ALL
   SELECT * from dbo.get_summary_workcnt_personality('40',@TargetYear,@TargetYear,@locationId,@LanguageId)--拡張データ、指定対象年月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--製造関連作業件数半期
UNION ALL
   SELECT * from dbo.get_summary_workcnt_personality('40',@StartMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--製造関連作業件数累計
UNION ALL
   SELECT * from dbo.get_summary_workcnt_personality('40',@BeginningMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID

UNION ALL 
   SELECT null as mq, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --作業総件数当月、構成ID(カンマ区切り)

UNION ALL 
   SELECT null as mq, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --作業総件数半期、構成ID(カンマ区切り)

UNION ALL 
   SELECT null as mq, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --作業総件数累計、構成ID(カンマ区切り)
