 drop table if exists #excel_port_error_info;
create table #excel_port_error_info ( 
  [row_no] int
  , [column_no] int
  , [error] nvarchar(1000) COLLATE Japanese_CI_AS_KS
  , [data_direction] int
  , [proc_div] nvarchar(50) COLLATE Japanese_CI_AS_KS
  , [proc_div_name] nvarchar(50) COLLATE Japanese_CI_AS_KS
)
