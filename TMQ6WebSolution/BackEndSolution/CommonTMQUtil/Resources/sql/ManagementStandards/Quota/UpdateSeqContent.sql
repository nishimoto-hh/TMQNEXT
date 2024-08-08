-- 機器別管理基準内容テーブルのシーケンスを更新
declare @sql nvarchar(max);
set @sql = 'ALTER SEQUENCE seq_mc_management_standards_content_management_standards_content_id RESTART WITH ' + CAST(@Seq AS nvarchar(max));

EXEC(@sql);