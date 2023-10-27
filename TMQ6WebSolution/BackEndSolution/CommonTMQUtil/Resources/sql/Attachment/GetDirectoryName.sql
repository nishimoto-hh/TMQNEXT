SELECT
    ex.extension_data AS directory
FROM
    ms_structure ms
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
    AND ex.sequence_no = 3
WHERE
    ms.structure_id = @StructureId
