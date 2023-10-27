select
	factory_id
	, report_id
	, sheet_no
	, sheet_name_translation_id
	, sheet_name
	, sheet_define_max_row
	, sheet_define_max_column
	, search_condition_flg
	, list_flg
	, record_count
	, target_sql
	, target_sql_params
from
	ms_output_report_sheet_define -- 出力帳票シート定義
where
	factory_id = @FactoryId
and report_id = @ReportId
and delete_flg = 0
