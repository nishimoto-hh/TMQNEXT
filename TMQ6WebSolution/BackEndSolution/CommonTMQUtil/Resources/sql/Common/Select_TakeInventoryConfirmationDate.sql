--******************************************************************
--予備品ID、新旧区分、部門より棚卸確定日を取得
--******************************************************************
SELECT
    MAX(fixed_datetime) AS fixed_datetime 
FROM
    pt_inventory 
WHERE
    parts_id = @PartsId                                  --予備品ID
    AND old_new_structure_id = @OldNewStructureId        --新旧区分
    AND department_structure_id = @DepartmentStructureId --部門
    AND parts_location_id = @PartsLocationId             --棚ID
    AND parts_location_detail_no = @PartsLocationDetailNo  --棚枝番
    AND account_structure_id = @AccountStructureId       --勘定科目ID
