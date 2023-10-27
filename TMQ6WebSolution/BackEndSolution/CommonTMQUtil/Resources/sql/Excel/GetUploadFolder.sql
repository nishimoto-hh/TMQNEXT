SELECT
    ex.extension_data AS folder
FROM
    v_structure_all item
    LEFT JOIN
        ms_item_extension ex
    ON  item.structure_item_id = ex.item_id
WHERE
    structure_group_id = 9180
