-- 機器別管理基準部位テーブルのシーケンスを更新
declare @sql nvarchar(max);
set @sql = 'ALTER SEQUENCE seq_mc_management_standards_component_management_standards_component_id RESTART WITH ' + CAST(@Seq AS nvarchar(max));

EXEC(@sql);