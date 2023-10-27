SELECT
    sequence_no,
    extension_data AS directory_name
from
    v_structure_item_all item
    LEFT JOIN
        ms_item_extension ex
    ON  item.structure_item_id = ex.item_id
WHERE
    structure_group_id = 9190
ORDER BY
    sequence_no
