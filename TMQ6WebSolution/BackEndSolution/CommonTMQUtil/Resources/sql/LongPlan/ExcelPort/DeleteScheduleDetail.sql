/*
 * 長期計画 機器別管理基準 ExcelPortアップロード 保全スケジュール詳細情報の削除
 */
DELETE FROM
    mc_maintainance_schedule_detail

WHERE
    maintainance_schedule_id IN (
        SELECT 
            ms.maintainance_schedule_id
        FROM
             mc_maintainance_schedule ms
        WHERE
            management_standards_content_id = @ManagementStandardsContentId)

AND
    complition != 1 -- 未完了
AND
    FORMAT(schedule_date, 'yyyyMM') BETWEEN @ScheduleDateFrom AND @ScheduleDateTo