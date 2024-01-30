SELECT
    COALESCE(max(tag.serial_no), 0) + 1 AS serial_no
FROM
    pt_rftag_parts_link tag 
WHERE
    tag.parts_id = @PartsId
    AND tag.department_structure_id = @DepartmentStructureId
    AND tag.account_structure_id = @AccountStructureId
