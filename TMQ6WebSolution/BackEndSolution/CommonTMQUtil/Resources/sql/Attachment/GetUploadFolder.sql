SELECT
    ex.extension_data AS folder
FROM
    ms_structure ms
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
WHERE
    ms.structure_group_id = 9080