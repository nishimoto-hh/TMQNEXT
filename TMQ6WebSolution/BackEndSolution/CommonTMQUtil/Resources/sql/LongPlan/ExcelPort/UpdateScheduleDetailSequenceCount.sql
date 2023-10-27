/*
* UpdateScheduleDetailSequenceCount.sql
* 長期計画 機器別管理基準　ExcelPortアップロード　保全スケジュール詳細 繰り返し回数を更新するSQL
*/
UPDATE
    detail
SET
     detail.sequence_count = @SequenceCount
    ,detail.update_serialid = detail.update_serialid + 1
    ,detail.update_datetime = @UpdateDatetime
    ,detail.update_user_id = @UpdateUserId
FROM
    mc_maintainance_schedule_detail AS detail

WHERE
    detail.maintainance_schedule_detail_id = @MaintainanceScheduleDetailId
