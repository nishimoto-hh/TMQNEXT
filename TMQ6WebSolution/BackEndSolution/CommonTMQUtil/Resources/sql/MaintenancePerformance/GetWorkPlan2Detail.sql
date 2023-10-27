--******************************************************************
--詳細(作業計画性分類)1取得(全行空行、javascriptにより算出)
--******************************************************************

SELECT null as stop_system_nm, null as machine, null as electricity, null as instrumentation, null as other --計画作業率当月

UNION ALL 
   SELECT null as stop_system_nm, null as machine, null as electricity, null as instrumentation, null as other --計画作業率半期

UNION ALL 
   SELECT null as stop_system_nm, null as machine, null as electricity, null as instrumentation, null as other --計画作業率累計

UNION ALL 
   SELECT null as stop_system_nm, null as machine, null as electricity, null as instrumentation, null as other --予防保全作業率当月

UNION ALL 
   SELECT null as stop_system_nm, null as machine, null as electricity, null as instrumentation, null as other --予防保全作業率半期  

UNION ALL 
   SELECT null as stop_system_nm, null as machine, null as electricity, null as instrumentation, null as other --予防保全作業率累計 

UNION ALL 
   SELECT null as stop_system_nm, null as machine, null as electricity, null as instrumentation, null as other --突発作業率当月

UNION ALL 
   SELECT null as stop_system_nm, null as machine, null as electricity, null as instrumentation, null as other --突発作業率半期

UNION ALL 
   SELECT null as stop_system_nm, null as machine, null as electricity, null as instrumentation, null as other --突発作業率累計
