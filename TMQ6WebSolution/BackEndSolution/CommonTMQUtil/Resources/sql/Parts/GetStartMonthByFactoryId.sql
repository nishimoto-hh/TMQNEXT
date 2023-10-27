SELECT
    FORMAT(CONVERT(int,ex.extension_data),'00') AS start_month
FROM
    v_structure_all item
    LEFT JOIN
        ms_item_extension ex
    ON  item.structure_item_id = ex.item_id
    AND ex.sequence_no = 1
WHERE
item.structure_group_id = 2020
AND item.factory_id = (
        SELECT
            parts.factory_id
        FROM
            pt_parts parts
        WHERE
            parts.parts_id = @PartsId
    )
