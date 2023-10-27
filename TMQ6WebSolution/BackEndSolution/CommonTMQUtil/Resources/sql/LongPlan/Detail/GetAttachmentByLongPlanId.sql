/*
 * 長計件名に紐づく添付情報のキーIDを取得する
 */
SELECT
    att.attachment_id,
    att.function_type_id,
    att.key_id,
    att.attachment_type_structure_id,
    att.file_path,
    att.file_name,
    att.document_type_structure_id,
    att.document_no,
    att.attachment_note,
    att.attachment_user_id,
    att.attachment_date,
    att.attachment_user_name,
    att.update_serialid,
    att.insert_datetime,
    att.insert_user_id,
    att.update_datetime,
    att.update_user_id
FROM
    ln_long_plan AS lp
    INNER JOIN
        mc_management_standards_content AS ms_con
    ON  (
            lp.long_plan_id = ms_con.long_plan_id
        )
    INNER JOIN
        mc_management_standards_component AS ms_com
    ON  (
            ms_com.management_standards_component_id = ms_con.management_standards_component_id
        )
    INNER JOIN
        mc_machine AS machine
    ON  (
            ms_com.machine_id = machine.machine_id
        )
    INNER JOIN
        mc_equipment AS eq
    ON  (
            machine.machine_id = eq.machine_id
        )
    INNER JOIN
        attachment AS att
    ON  (
            eq.equipment_id = att.key_id
        AND att.function_type_id = 1640
        )
WHERE
    lp.long_plan_id = @LongPlanId
