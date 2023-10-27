/*
 * MakePlanとMakeMaintainanceのこのファイルは同一
 * 変更時は注意、処理での都合で機能でフォルダを分けているため。
*/
INSERT INTO ma_request(
    request_id,
    summary_id,
    issue_date,
    update_serialid,
    insert_datetime,
    insert_user_id,
    update_datetime,
    update_user_id
)
VALUES(
    NEXT VALUE FOR seq_ma_request_request_id,
    @SummaryId,
    @InsertDatetime,
    @UpdateSerialid,
    @InsertDatetime,
    @InsertUserId,
    @UpdateDatetime,
    @UpdateUserId
)
