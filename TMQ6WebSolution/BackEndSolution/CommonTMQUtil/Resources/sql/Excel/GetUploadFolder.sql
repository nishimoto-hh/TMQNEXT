SELECT
    ex.extension_data AS folder
FROM
    v_structure_item_all item
    LEFT JOIN
        ms_item_extension ex
    ON  item.structure_item_id = ex.item_id
WHERE
    structure_group_id = 9180
AND item.language_id = @LanguageId
