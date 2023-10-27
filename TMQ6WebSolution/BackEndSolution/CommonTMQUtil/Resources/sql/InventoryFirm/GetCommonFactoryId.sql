SELECT
    ms.structure_id AS factory_id
FROM
    ms_structure ms
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
    AND ex.sequence_no = 3
WHERE
    ms.structure_id IN(
        SELECT
            mub.location_structure_id
        FROM
            ms_user_belong mub
        WHERE
            mub.user_id = @UserId
    )
AND ms.structure_layer_no = 1
AND ex.extension_data = '1'