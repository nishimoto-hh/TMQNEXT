SELECT
    ex.sequence_no,
    ex.extension_data AS directory_name
FROM
    ms_structure ms
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
WHERE
    ms.structure_group_id = 9090
ORDER BY
    sequence_no
