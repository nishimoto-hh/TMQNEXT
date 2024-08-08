-- 保全スケジュールテーブルのシーケンスを更新
declare @sql nvarchar(max);
set @sql = 'ALTER SEQUENCE seq_mc_maintainance_schedule_maintainance_schedule_id RESTART WITH ' + CAST(@Seq AS nvarchar(max));

EXEC(@sql);