SELECT
    COUNT(inventory_id) 
FROM
    pt_inventory 
WHERE
    parts_id = @PartsId                         -- 予備品id
    AND parts_location_id = @PartsLocationId    -- 棚id

/*@PartsLocationDetailNo
    AND parts_location_detail_no = @PartsLocationDetailNo -- 棚枝番
@PartsLocationDetailNo*/
/*@PartsLocationDetailNoIsNull
    AND parts_location_detail_no IS NULL -- 棚枝番
@PartsLocationDetailNoIsNull*/
       
    AND old_new_structure_id = @OldNewStructureId -- 新旧区分id
    AND department_structure_id = @DepartmentStructureId -- 部門id
    AND account_structure_id = @AccountStructureId -- 勘定科目id
    AND target_month = @TargetMonth --対象年月
