/*
 * MakePlanとMakeMaintainanceのこのファイルは同一
 * 変更時は注意、処理での都合で機能でフォルダを分けているため。
*/
INSERT INTO ma_history_machine(
    history_machine_id,
    history_id,
    machine_id,
    equipment_id,
    update_serialid,
    insert_datetime,
    insert_user_id,
    update_datetime,
    update_user_id
) output inserted.history_machine_id
SELECT
    NEXT VALUE FOR seq_ma_history_machine_history_machine_id,
    @HistoryId,
    com.machine_id,
    eq.equipment_id,
    @UpdateSerialid,
    @InsertDatetime,
    @InsertUserId,
    @UpdateDatetime,
    @UpdateUserId
FROM
    mc_management_standards_content AS con
    INNER JOIN
        mc_management_standards_component AS com
    ON  (
            con.management_standards_component_id = com.management_standards_component_id
        )
    INNER JOIN
        mc_equipment AS eq
    ON  (
            eq.machine_id = com.machine_id
        )
WHERE
    con.long_plan_id = @LongPlanId
AND con.management_standards_content_id = @ManagementStandardsContentId
