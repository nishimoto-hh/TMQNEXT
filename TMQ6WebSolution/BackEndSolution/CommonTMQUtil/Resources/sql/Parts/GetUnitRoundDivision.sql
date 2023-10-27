SELECT
    coalesce(ex.extension_data, 1) AS unit_round_division
FROM
    ms_structure ms
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
    AND ex.sequence_no = 1
WHERE
    ms.structure_group_id = 2050
AND ms.factory_id = @PartsFactoryId