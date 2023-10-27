--******************************************************************
--詳細(系の停止)取得
--******************************************************************

--保全要因当月
   SELECT * from dbo.get_summary_workcnt('10',@TargetYear,@TargetYear,@locationId,@LanguageId)--拡張データ(保全要因)、指定対象年月、指定対象年月、場所階層ID(カンマ区切り)、言語ID
--保全要因半期
UNION ALL 
    SELECT * from dbo.get_summary_workcnt('10',@StartMonth,@TargetYear,@locationId,@LanguageId)--拡張データ(保全要因)、期の開始月、指定対象年月、場所階層ID(カンマ区切り)、言語ID
    
--保全要因累計
UNION ALL 
    SELECT * from dbo.get_summary_workcnt('10',@BeginningMonth,@TargetYear,@locationId,@LanguageId)--拡張データ(保全要因)、期首月、指定対象年月、場所階層ID(カンマ区切り)、言語ID
    
--製造要因当月
UNION ALL 
    SELECT * from dbo.get_summary_workcnt('20',@TargetYear,@TargetYear,@locationId,@LanguageId)--拡張データ(製造要因)、指定対象年月、指定対象年月、場所階層ID(カンマ区切り)、言語ID
    
--製造要因半期
UNION ALL 
    SELECT * from dbo.get_summary_workcnt('20',@StartMonth,@TargetYear,@locationId,@LanguageId)--拡張データ(製造要因)、期の開始月、指定対象年月、場所階層ID(カンマ区切り)、言語ID
    
--製造要因累計
UNION ALL  
    SELECT * from dbo.get_summary_workcnt('20',@BeginningMonth,@TargetYear,@locationId,@LanguageId)--拡張データ(製造要因)、期首月、指定対象年月、場所階層ID(カンマ区切り)、言語ID
    
UNION ALL 
   SELECT null as stop_system_nm, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --当月計 

UNION ALL 
   SELECT null as stop_system_nm, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --半期計 

UNION ALL 
   SELECT null as stop_system_nm, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --累計計 
   
--系の停止時間(保全要因)当月
UNION ALL 
    SELECT * from dbo.get_summary_stoptime(@TargetYear,@TargetYear,@locationId,@LanguageId)--指定対象年月、指定対象年月、場所階層ID(カンマ区切り)、言語ID
    
--系の停止時間(保全要因)半期
UNION ALL 
    SELECT * from dbo.get_summary_stoptime(@StartMonth,@TargetYear,@locationId,@LanguageId)--期の開始月、指定対象年月、場所階層ID(カンマ区切り)、言語ID
    
--系の停止時間(保全要因)累計
UNION ALL 
    SELECT * from dbo.get_summary_stoptime(@BeginningMonth,@TargetYear,@locationId,@LanguageId)--期首月、指定対象年月、場所階層ID(カンマ区切り)、言語ID

