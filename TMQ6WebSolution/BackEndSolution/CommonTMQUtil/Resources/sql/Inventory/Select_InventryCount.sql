--*****************************************
--準備リストが出力されているかを見るSQL
--*****************************************
SELECT
    preparation_datetime
    , fixed_datetime 
FROM
    pt_inventory 
WHERE
    parts_id = @PartsId 
    AND old_new_structure_id = @OldNewStructureId --新旧区分ID
    AND department_structure_id = @DepartmentStructureId --部門ID
    AND account_structure_id = @AccountStructureId --勘定科目ID
    AND parts_location_id = @PartsLocationId --棚ID
    AND COALESCE(parts_location_detail_no, '') = @PartsLocationDetailNo --棚枝番
    AND target_month = @TargetYearMonth 
