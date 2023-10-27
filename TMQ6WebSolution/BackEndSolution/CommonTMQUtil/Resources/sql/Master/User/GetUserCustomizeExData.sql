SELECT
    ex1.extension_data AS program_id,
    ex2.extension_data AS control_group_id,
    ex3.extension_data AS form_no,
    ex4.extension_data AS control_no
FROM
    v_structure_item_all item
    LEFT JOIN
        ms_item_extension ex1
    ON  item.structure_item_id = ex1.item_id
    AND ex1.sequence_no = 1
    LEFT JOIN
        ms_item_extension ex2
    ON  item.structure_item_id = ex2.item_id
    AND ex2.sequence_no = 2
    LEFT JOIN
        ms_item_extension ex3
    ON  item.structure_item_id = ex3.item_id
    AND ex3.sequence_no = 3
    LEFT JOIN
        ms_item_extension ex4
    ON  item.structure_item_id = ex4.item_id
    AND ex4.sequence_no = 4
WHERE
    item.structure_group_id = 2180