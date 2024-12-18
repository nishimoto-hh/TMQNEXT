--画面項目定義(共通レイアウト)
SELECT
	 fcd.location_structure_id AS LOCATION_LAYER_ID
	,fd.program_id AS PGMID
	,fd.form_no AS FORMNO
	,fd.control_group_id AS CTRLID
	,fcd.define_type AS DEFINETYPE
	,fcd.control_no AS ITEMNO
	,fcd.control_type AS CELLTYPE
	,fcd.control_id AS ITEMID
	,fcd.display_division AS DISPKBN
	,fcd.row_no AS ROWNO
	,fcd.column_no AS COLNO
	,fcd.row_span AS ROWSPAN
	,fcd.column_span AS COLSPAN
	,fcd.header_row_span AS HEADER_ROWSPAN
	,fcd.header_column_span AS HEADER_COLSPAN
	,fcd.position AS POSITION
	,fcd.column_width AS COLWIDTH
	,fcd.from_to_division AS FROMTOKBN
	,fcd.control_count AS ITEM_CNT
	,fcd.initial_value AS INITVAL
	,fcd.required_division AS NULLCHKKBN
	,fcd.text_auto_complete_division AS TXT_AUTOCOMPKBN
	,fcd.button_control_id AS BTN_CTRLID
	,fcd.button_action_division AS BTN_ACTIONKBN
	,fcd.button_authority_division AS BTN_AUTHCONTROLKBN
	,fcd.button_after_execution_division AS BTN_AFTEREXECKBN
	,fcd.button_message AS BTN_MESSAGE
	,fcd.dat_transition_pattern AS DAT_TRANSITION_PATTERN
	,fcd.dat_transition_action_division AS DAT_TRANSITION_ACTION_DIVISION
	,fcd.relation_id AS RELATIONID
	,fcd.relation_parameters AS RELATIONPARAM
	,fcd.option_information AS OPTIONINFO
	,fcd.unchangeable_division AS UNCHANGEABLEKBN
	,fcd.column_fixed_division AS COLFIXKBN
	,fcd.filter_use_division AS FILTERUSEKBN
	,ISNULL(fcd.sort_division, 0) AS SORT_DIVISION
	,fcd.detailed_search_division AS DETAILED_SEARCH_DIVISION
	,ISNULL(fcd.detailed_search_control_type, fcd.control_type) AS DETAILED_SEARCH_CELLTYPE
	,fcd.control_customize_flg AS ITEM_CUSTOMIZE_FLG
	,fcd.css_name AS CSSNAME
	,fcd.expansion_key_name AS EXP_KEY_NAME
	,fcd.expansion_table_name AS EXP_TABLE_NAME
	,fcd.expansion_column_name AS EXP_COL_NAME
	,fcd.expansion_parameters_name AS EXP_PARAM_NAME
	,fcd.expansion_alias_name AS EXP_ALIAS_NAME
	,fcd.expansion_like_pattern AS EXP_LIKE_PATTERN
	,fcd.expansion_in_clause_division AS EXP_IN_CLAUSE_KBN
	,fcd.expansion_lock_type AS EXP_LOCK_TYPE
	,fcd.delete_flg AS DELFLG
	,fcd.control_id AS ITEMNAME
	,cd.minimum_value AS MINVAL
	,cd.maximum_value AS MAXVAL
	,CASE WHEN cd.format_translation_id IS NULL THEN null ELSE CONVERT(nvarchar, cd.format_translation_id) END AS FORMAT
	,ISNULL(cd.maximum_length, 0) AS MAXLENGTH
	,CASE WHEN cd.text_placeholder_translation_id IS NULL THEN null ELSE CONVERT(nvarchar, cd.text_placeholder_translation_id) END AS TXT_PLACEHOLDER
	,CASE WHEN cd.tooltip_translation_id IS NULL THEN null ELSE CONVERT(nvarchar, cd.tooltip_translation_id) END AS TOOLTIP
    ,fcd.column_no AS DISPLAY_ORDER
    ,CONVERT(BIT, 1) AS DISPLAY_FLG

	-- マッピング情報に不足している項目
    ,fd.group_no as GrpNo
    ,cd.column_name as ColOrgName
    ,cd2.column_name as DetailSearchColOrgName
    ,ISNULL(cd2.data_type, cd.data_type) as DetailSearchDataType
    ,cd.data_type as DataType
    ,fcd.expansion_lock_table_name as LockTblName

FROM cm_form_define fd
LEFT JOIN cm_form_control_define fcd
ON fd.common_form_no = fcd.form_no
LEFT JOIN cm_control_define cd
ON fcd.control_id = cd.control_id
AND fcd.control_type = cd.control_type
LEFT JOIN cm_control_unused cu
ON fcd.location_structure_id = cu.location_structure_id
AND fcd.control_id = cu.control_id
AND fcd.control_type = cu.control_type
--マッピング情報用
LEFT JOIN cm_control_define cd2
ON fcd.control_id = cd2.control_id
AND fcd.detailed_search_control_type = cd2.control_type

WHERE
    fcd.control_group_id = 'CommonCtrl'
AND fcd.delete_flg != 1
AND cu.control_id IS NULL
