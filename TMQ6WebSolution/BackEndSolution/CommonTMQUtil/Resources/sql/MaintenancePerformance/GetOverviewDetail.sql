--******************************************************************
--概要取得(全行空行、javascriptにより算出)
--******************************************************************

SELECT null as item, null as stopcnt, null as stoptime, null as failure1cnt, null as Failure2cnt, null as predictiverepaircnt, null as  totalworkcnt, null as plannedworkrate, null as callscnt --当月

UNION ALL 
   SELECT null as item, null as stopcnt, null as stoptime, null as failure1cnt, null as Failure2cnt, null as predictiverepaircnt, null as  totalworkcnt, null as plannedworkrate, null as callscnt --半期

UNION ALL 
   SELECT null as item, null as stopcnt, null as stoptime, null as failure1cnt, null as Failure2cnt, null as predictiverepaircnt, null as  totalworkcnt, null as plannedworkrate, null as callscnt --累計
