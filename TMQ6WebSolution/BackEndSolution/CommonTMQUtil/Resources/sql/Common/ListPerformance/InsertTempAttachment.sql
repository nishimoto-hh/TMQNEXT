INSERT INTO #temp_attachment
SELECT
    ac.key_id
    ,ac.function_type_id
    ,ac.file_name
    ,ac.attachment_id
    ,ac.document_no
    ,ex.extension_data
FROM
    attachment ac
    LEFT JOIN
        ms_structure item
    ON  ac.attachment_type_structure_id = item.structure_id
    AND item.delete_flg = 0
    LEFT JOIN
        ms_item_extension ex
    ON  item.structure_item_id = ex.item_id
    AND ex.sequence_no = 1
WHERE ac.function_type_id IN @FunctionTypeIdList;