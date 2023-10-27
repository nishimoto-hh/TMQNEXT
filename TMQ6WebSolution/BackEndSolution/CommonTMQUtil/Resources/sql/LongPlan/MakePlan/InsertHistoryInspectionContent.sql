/*
 * MakePlanとMakeMaintainanceのこのファイルは同一
 * 変更時は注意、処理での都合で機能でフォルダを分けているため。
*/
INSERT INTO ma_history_inspection_content(
    history_inspection_content_id,
    history_inspection_site_id,
    inspection_content_structure_id,
    update_serialid,
    insert_datetime,
    insert_user_id,
    update_datetime,
    update_user_id
)
SELECT
    NEXT VALUE FOR seq_ma_history_inspection_content_history_inspection_content_id,
    @HistoryInspectionSiteId,
    con.inspection_content_structure_id,
    @UpdateSerialid,
    @InsertDatetime,
    @InsertUserId,
    @UpdateDatetime,
    @UpdateUserId
FROM
    mc_management_standards_content AS con
WHERE
    con.long_plan_id = @LongPlanId
AND con.management_standards_content_id = @ManagementStandardsContentId
