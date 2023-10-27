--作業Noのロット管理IDと同一で、新しい受払が発生している件数を取得
SELECT
    COUNT(parent.inout_history_id)
FROM
    pt_inout_history parent
WHERE
    parent.lot_control_id IN(
        SELECT
            child.lot_control_id
        FROM
            pt_inout_history child
            LEFT JOIN
                inout_div
            ON  child.inout_division_structure_id = inout_div.inout_id
        WHERE
            child.work_no = @WorkNo
        AND inout_div.inout_code = '1'
    )
AND parent.inout_history_id > (
        SELECT
            MAX(child.inout_history_id)
        FROM
            pt_inout_history child
            LEFT JOIN
                inout_div
            ON  child.inout_division_structure_id = inout_div.inout_id
        WHERE
            child.work_no = @WorkNo
        AND inout_div.inout_code = '1'
    )
AND parent.delete_flg = 0