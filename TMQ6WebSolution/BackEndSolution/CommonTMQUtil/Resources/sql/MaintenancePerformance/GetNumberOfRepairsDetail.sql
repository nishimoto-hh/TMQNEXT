--******************************************************************
--詳細(故障修理件数)取得
--******************************************************************

--TBM故障当月
  SELECT * from dbo.get_summary_workcnt_mq('1',@TargetYear,@TargetYear,@locationId,@LanguageId)--拡張データ、指定対象年月、指定対象年月、構成ID(カンマ区切り)、言語ID

--TBM故障半期
UNION ALL
  SELECT * from dbo.get_summary_workcnt_mq('1',@StartMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--TBM故障累計
UNION ALL
   SELECT * from dbo.get_summary_workcnt_mq('1',@BeginningMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--故障1 CBM想定外故障当月
UNION ALL
   SELECT * from dbo.get_summary_workcnt_mq('2',@TargetYear,@TargetYear,@locationId,@LanguageId)--拡張データ、指定対象年月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--故障1 CBM想定外故障半期
UNION ALL
   SELECT * from dbo.get_summary_workcnt_mq('2',@StartMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--故障1 CBM想定外故障累計
UNION ALL 
   SELECT * from dbo.get_summary_workcnt_mq('2',@BeginningMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
UNION ALL 
   SELECT null as failure, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --故障1計

UNION ALL 
   SELECT null as failure, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --故障1計

UNION ALL 
   SELECT null as failure, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --故障1計
   
--故障2 CBM想定外故障当月
UNION ALL
   SELECT * from dbo.get_summary_workcnt_mq('3',@TargetYear,@TargetYear,@locationId,@LanguageId)--拡張データ、指定対象年月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--故障2 CBM想定外故障半期
UNION ALL
   SELECT * from dbo.get_summary_workcnt_mq('3',@StartMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--故障2 CBM想定外故障累計
UNION ALL
   SELECT * from dbo.get_summary_workcnt_mq('3',@BeginningMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--BDM故障当月
UNION ALL
   SELECT * from dbo.get_summary_workcnt_mq('4',@TargetYear,@TargetYear,@locationId,@LanguageId)--拡張データ、指定対象年月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--BDM故障半期
UNION ALL
   SELECT * from dbo.get_summary_workcnt_mq('4',@StartMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--BDM故障累計
UNION ALL
   SELECT * from dbo.get_summary_workcnt_mq('4',@BeginningMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
UNION ALL 
   SELECT null as failure, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --故障2計

UNION ALL 
   SELECT null as failure, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --故障2計

UNION ALL 
   SELECT null as failure, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --故障2計
   
--予知修理当月
UNION ALL
   SELECT * from dbo.get_summary_workcnt_mq('5',@TargetYear,@TargetYear,@locationId,@LanguageId)--拡張データ、指定対象年月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--予知修理半期
UNION ALL
   SELECT * from dbo.get_summary_workcnt_mq('5',@StartMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--予知修理累計
UNION ALL
   SELECT * from dbo.get_summary_workcnt_mq('5',@BeginningMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--計(故障修理件数当月)
UNION ALL
   SELECT * from dbo.get_summary_workcnt_personality('20',@TargetYear,@TargetYear,@locationId,@LanguageId)--拡張データ、指定対象年月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--計(故障修理件数半期)
UNION ALL
   SELECT * from dbo.get_summary_workcnt_personality('20',@StartMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
   
--計(故障修理件数累計)
UNION ALL
   SELECT * from dbo.get_summary_workcnt_personality('20',@BeginningMonth,@TargetYear,@locationId,@LanguageId)--拡張データ、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
