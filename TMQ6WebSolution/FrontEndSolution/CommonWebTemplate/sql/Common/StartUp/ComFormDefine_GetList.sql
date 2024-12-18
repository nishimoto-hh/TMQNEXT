--COM_FORM_DEFINE
SELECT
	 program_id AS PGMID
	,form_no AS FORMNO
	,control_group_id AS CTRLID
	,control_group_name AS CTRLNAME
	,control_group_type AS CTRLTYPE
	,area_division AS AREAKBN
	,display_order AS DISPORDER
	,dat_form_title AS DAT_FORMTITLE
	,group_no AS CTRLGRPNO
	,group_name AS CTRLGRPNAME
	,tab_no AS TABNO
	,tab_name AS TABNAME
	,dat_transition_pattern AS DAT_TRANSPTN
	,dat_transition_display_pattern AS DAT_TRANSDISPPTN
	,dat_transition_target AS DAT_TRANSTARGET
	,dat_transition_parameters AS DAT_TRANSPARAM
	,dat_edit_pattern AS DAT_EDITPTN
	,dat_direction AS DAT_DIRECTION
	,dat_header_display_division AS DAT_HEADERDISPKBN
	,dat_page_rows AS DAT_PAGEROWS
	,dat_maximum_rows AS DAT_MAXROWS
	,dat_title AS DAT_TITLE
	,dat_switch_division AS DAT_SWITCHKBN
	,dat_row_add_division AS DAT_ROWADDKBN
	,dat_row_delete_division AS DAT_ROWDELKBN
	,dat_row_select_division AS DAT_ROWSELKBN
	,dat_column_select_division AS DAT_COLSELKBN
	,dat_row_sort_division AS DAT_ROWSORTKBN
	,dat_rowno_pattern AS DAT_TRANSICONKBN
	,dat_height AS DAT_HEIGHT
	,control_relation_control_id AS CTR_RELATIONCTRLID
	,control_position_division AS CTR_POSITIONKBN
	,css_name AS CSSNAME
	,tooltip AS TOOLTIP
	,reference_mode_flg AS REFERENCE_MODE
	,common_form_no AS COMM_FORMNO
FROM cm_form_define
WHERE
    delete_flg != 1;
