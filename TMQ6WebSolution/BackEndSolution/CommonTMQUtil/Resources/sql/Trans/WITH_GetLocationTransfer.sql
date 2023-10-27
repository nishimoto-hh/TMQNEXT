WITH parts AS(
    --予備品倉庫
    SELECT
        translation_text,
        structure_id
    FROM
        v_structure_item
    WHERE
        v_structure_item.structure_group_id = 1040
    AND v_structure_item.language_id = 'ja'
    AND structure_layer_no = 0
),
location AS(
    --棚
    SELECT
        translation_text,
        structure_id
    FROM
        v_structure_item
    WHERE
        v_structure_item.structure_group_id = 1040
    AND v_structure_item.language_id = 'ja'
    AND structure_layer_no = 1
)
