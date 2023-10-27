SELECT
    COALESCE(ex.extension_data, 0) AS currency_digit
FROM
    ms_structure ms
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
    AND ex.sequence_no = 2
WHERE
    ms.structure_group_id = 1740
AND ms.structure_id = @CurrencyStructureId