select
	 attname as ColName
	,typname as TypeName
from 
	 pg_class
	,pg_attribute
	,pg_type

where relkind = 'r'
and relname = /*TableName*/'com_trns_mst'
and attrelid = (select relid from pg_stat_all_tables where relname = /*TableName*/'com_trns_mst')
and attnum > 0
and pg_type.oid = atttypid
order by attnum
