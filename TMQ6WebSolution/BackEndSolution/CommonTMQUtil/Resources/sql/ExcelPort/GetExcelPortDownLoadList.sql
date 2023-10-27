SELECT DISTINCT
	parent.translation_text AS conductCategoryName
	-- 機能分類名
	,item.translation_text AS conductName
	-- 機能名
	,item_ex1.extension_data AS hide_conduct_id
	-- 対象ロジック機能ID(拡張項目)
	,item_ex2.extension_data AS hide_sheet_no
	-- ExcelPort対象シート番号(拡張項目)
	,item_ex3.extension_data AS add_condition
	-- 追加条件区分(拡張項目)
	,item.structure_id
    -- 構成ID(ソートするため)
FROM
	v_structure_item_all AS item
	LEFT OUTER JOIN
		ms_item_extension AS item_ex1
	ON	item.structure_item_id = item_ex1.item_id
	AND	item_ex1.sequence_no = 1
	LEFT OUTER JOIN
		ms_item_extension AS item_ex2
	ON	item.structure_item_id = item_ex2.item_id
	AND	item_ex2.sequence_no = 2
	LEFT OUTER JOIN
		ms_item_extension AS item_ex3
	ON	item.structure_item_id = item_ex3.item_id
	AND	item_ex3.sequence_no = 3
	LEFT JOIN
	ms_user_conduct_authority AS userAuth
	ON	userAuth.conduct_id = item_ex1.extension_data
	LEFT JOIN
	parent
	ON	item.parent_structure_id = parent.structure_id
	LEFT JOIN
	ms_item_extension
	ON	item.structure_item_id = ms_item_extension.item_id
WHERE
	item.structure_group_id = 2110
AND	item.structure_layer_no = 1
AND	item.language_id = @LanguageId
AND item_ex1.extension_data IN (
         SELECT 
            conduct_id 
         FROM 
            ms_user_conduct_authority 
         WHERE 
            user_id = @UserId)
ORDER BY 
	item.structure_id