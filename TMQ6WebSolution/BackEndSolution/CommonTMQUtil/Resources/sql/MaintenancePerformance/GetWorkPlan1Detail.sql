--******************************************************************
--詳細(作業計画性分類)1取得
--******************************************************************

--計画作業件数当月(MQ分類：10)
    SELECT * from dbo.get_summary_workcnt_plan(@TargetYear,@TargetYear,@locationId,@LanguageId)--指定対象年月、指定対象年月、構成ID(カンマ区切り)、言語ID
    
--計画作業件数半期(MQ分類：10)
UNION ALL
    SELECT * from dbo.get_summary_workcnt_plan(@StartMonth,@TargetYear,@locationId,@LanguageId)--期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
    
--計画作業件数累計(MQ分類：10)
UNION ALL
    SELECT * from dbo.get_summary_workcnt_plan(@BeginningMonth,@TargetYear,@locationId,@LanguageId)--期首月、指定対象年月、構成ID(カンマ区切り)、言語ID
    
--計画作業件数当月(MQ分類：10以外)
UNION ALL
    SELECT * from dbo.get_summary_workcnt_plan_mq(@TargetYear,@TargetYear,@locationId,@LanguageId)--指定対象年月、指定対象年月、構成ID(カンマ区切り)、言語ID
    
--計画作業件数半期(MQ分類：10以外)
UNION ALL
    SELECT * from dbo.get_summary_workcnt_plan_mq(@StartMonth,@TargetYear,@locationId,@LanguageId)--期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
    
--計画作業件数累計(MQ分類：10以外)
UNION ALL
    SELECT * from dbo.get_summary_workcnt_plan_mq(@BeginningMonth,@TargetYear,@locationId,@LanguageId)--期首月、指定対象年月、構成ID(カンマ区切り)、言語ID
    
UNION ALL 
   SELECT null as sudden_class, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --当月小計 

UNION ALL 
   SELECT null as sudden_class, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --半期小計  

UNION ALL 
   SELECT null as sudden_class, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --累計小計  
--突発作業件数当月
UNION ALL
    SELECT * from dbo.get_summary_workcnt_sudden('30',@TargetYear,@TargetYear,@locationId,@LanguageId)--突発計画区分、指定対象年月、指定対象年月、構成ID(カンマ区切り)、言語ID
    
--突発作業件数半期
UNION ALL 
    SELECT * from dbo.get_summary_workcnt_sudden('30',@StartMonth,@TargetYear,@locationId,@LanguageId)--突発計画区分、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
    
--突発作業件数累計
UNION ALL 
    SELECT * from dbo.get_summary_workcnt_sudden('30',@BeginningMonth,@TargetYear,@locationId,@LanguageId)--突発計画区分、期首月、指定対象年月、構成ID(カンマ区切り)、言語ID
    
--計画外作業件数当月
UNION ALL
    SELECT * from dbo.get_summary_workcnt_sudden('20',@TargetYear,@TargetYear,@locationId,@LanguageId)--突発計画区分、指定対象年月、指定対象年月、構成ID(カンマ区切り)、言語ID
    
--計画外作業件数半期
UNION ALL
    SELECT * from dbo.get_summary_workcnt_sudden('20',@StartMonth,@TargetYear,@locationId,@LanguageId)--突発計画区分、期の開始月、指定対象年月、構成ID(カンマ区切り)、言語ID
    
--計画外作業件数累計
UNION ALL
    SELECT * from dbo.get_summary_workcnt_sudden('20',@BeginningMonth,@TargetYear,@locationId,@LanguageId)--突発計画区分、期首月、指定対象年月、構成ID(カンマ区切り)、言語ID

UNION ALL 
   SELECT null as sudden_class, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --当月小計 

UNION ALL 
   SELECT null as sudden_class, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --半期小計  

UNION ALL 
   SELECT null as sudden_class, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --累計小計  

UNION ALL 
  SELECT null as sudden_class, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --当月作業総件数

UNION ALL 
   SELECT null as sudden_class, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --半期作業総件数  

UNION ALL 
   SELECT null as stop_system_nm, null as machine, null as electricity, null as instrumentation, null as other, null as cnt --累計作業総件数 
