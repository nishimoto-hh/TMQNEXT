WITH parent AS(
	SELECT
		translation_text,
		structure_id
	FROM
		v_structure_item_all
	WHERE
		structure_group_id = 2110
	AND	v_structure_item_all.structure_layer_no = 0
	AND	language_id = @LanguageId
)