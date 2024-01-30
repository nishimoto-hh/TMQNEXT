SELECT
    exd.extension_data AS default_department_code
    , exa.extension_data AS default_account_code 
FROM
    pt_parts parts 
    LEFT JOIN ms_structure msd 
        ON parts.department_structure_id = msd.structure_id 
    LEFT JOIN ms_item_extension exd 
        ON msd.structure_item_id = exd.item_id 
        AND exd.sequence_no = 1 
    LEFT JOIN ms_structure msa 
        ON parts.account_structure_id = msa.structure_id 
    LEFT JOIN ms_item_extension exa 
        ON msa.structure_item_id = exa.item_id 
        AND exa.sequence_no = 1 
WHERE
    parts.parts_id = @PartsId
