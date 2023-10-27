/*
* UpdateMaintainanceScheduleDetail.sql
* スケジュール確定ボタン押下時、保全スケジュール詳細を更新するSQL
*/
UPDATE
    detail
SET
     detail.schedule_date = dateadd(MONTH, @AddMonth, detail.schedule_date)
    ,detail.update_serialid = detail.update_serialid + 1
    ,detail.update_datetime = @UpdateDatetime
    ,detail.update_user_id = @UpdateUserId
FROM
    mc_maintainance_schedule AS head
    INNER JOIN mc_maintainance_schedule_detail AS detail
    ON  (
            head.maintainance_schedule_id = detail.maintainance_schedule_id
        )
WHERE
    head.management_standards_content_id = @ManagementStandardsContentId
--AND detail.maintainance_schedule_id = @MaintainanceScheduleId
AND detail.schedule_date BETWEEN @MonthStartDate AND @MonthEndDate
AND detail.complition != 1
